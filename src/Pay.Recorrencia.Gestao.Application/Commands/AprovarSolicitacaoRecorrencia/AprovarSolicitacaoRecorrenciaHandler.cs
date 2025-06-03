using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Commands.IncluirAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Interfaces;
using Pay.Recorrencia.Gestao.Application.Query.SolicAutorizacaoRec.Detalhes;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Application.Services;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Helpers;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using Pay.Recorrencia.Gestao.Domain.Services;
using System.Globalization;
using static Pay.Recorrencia.Gestao.Application.Commands.AprovarSolicitacaoRecorrencia.AprovarSolicitacaoRecorrenciaCommand;

namespace Pay.Recorrencia.Gestao.Application.Commands.AprovarSolicitacaoRecorrencia
{
    public class AprovarSolicitacaoRecorrenciaHandler : IRequestHandler<AprovarSolicitacaoRecorrenciaCommand, MensagemPadraoResponse>
    {
        private static int sequenceCounter = 0;

        private ISolicitacaoRecorrenciaRepository _solicitacaoRecorrenciaRepository;
        private IInserirAutorizacaoRecorrenciaService _inserirAutorizacaoRecorrenciaService;
        private IAtualizarAutorizacaoService _atualizarAutorizacaoService;
        private IMapper _mapper { get; }
        private IAtualizarAutorizacaoRecorrenciaService _atualizarAutorizacaoRecorrenciaService;
        private IKafkaProducerService _kafkaProducerService { get; }
        private IConfiguration _config { get; }
        public AprovarSolicitacaoRecorrenciaHandler(
            ISolicitacaoRecorrenciaRepository solicitacaoRecorrenciaRepository,
            IMapper mapper,
            IInserirAutorizacaoRecorrenciaService inserirAutorizacaoRecorrenciaService, 
            IAtualizarAutorizacaoService atualizarAutorizacaoService,
            IAtualizarAutorizacaoRecorrenciaService atualizarAutorizacaoRecorrenciaService,
            IKafkaProducerService kafkaProducerService,
            IConfiguration configuration)
        {
            _solicitacaoRecorrenciaRepository = solicitacaoRecorrenciaRepository;
            _mapper = mapper;
            _inserirAutorizacaoRecorrenciaService = inserirAutorizacaoRecorrenciaService;
            _atualizarAutorizacaoService = atualizarAutorizacaoService;
            _atualizarAutorizacaoRecorrenciaService = atualizarAutorizacaoRecorrenciaService;
            _kafkaProducerService = kafkaProducerService;
            _config = configuration;
        }

        public async Task<MensagemPadraoResponse> Handle(AprovarSolicitacaoRecorrenciaCommand request, CancellationToken cancellationToken)
        {
            DateTime DataAtual = DateTime.Now;

            var objConsultaSolicitacaoAutorizacao = new DetalhesSolicAutorizacaoRecRequest()
            {
                IdSolicRecorrencia = request.IdSolicRecorrencia,
            };

            SolicAutorizacaoRecNonPagination solicitacaoRecorrenciaBanco = _solicitacaoRecorrenciaRepository.GetAsync(objConsultaSolicitacaoAutorizacao).Result;
            
            if (solicitacaoRecorrenciaBanco == null)
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-003", "Solicitação de recorrência não encontrada"));
            //AprovarSolicRecBanco aprovarSolicRecBanco = MapSolicitacaoToCommand(solicitacaoRecorrenciaBanco);

            if (!ValidaSolicitacaoRecorrencia(solicitacaoRecorrenciaBanco.Data, request)) //ARRUMAR
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-006", "Dados enviados pelo canal divergentes dos da tabela SOLICITACAO_AUTORIZACAO_RECORRENCIA"));

            var codMunIBGE = _solicitacaoRecorrenciaRepository.ConsultarCodMunIBGE();
            var inserirAutorizacaoRecorrenciaCommand = ConverteParaInserirAutorizacaoRecorrencia(request, codMunIBGE);
            var responseInsertAutorizacao = await _inserirAutorizacaoRecorrenciaService.Handle(inserirAutorizacaoRecorrenciaCommand);

            if (responseInsertAutorizacao.StatusCode != StatusCodes.Status200OK)
                return responseInsertAutorizacao;

            var atualizarAutorizacaoCommand = ConverteParaAtualizarAutorizacaoRecorrencia(request, inserirAutorizacaoRecorrenciaCommand, DataAtual, responseInsertAutorizacao.IdAutorizacaoResponse);
            var responseUpdateAutorizacao = await _atualizarAutorizacaoService.Handle(atualizarAutorizacaoCommand);

            if (responseUpdateAutorizacao.StatusCode != StatusCodes.Status200OK)
                return responseUpdateAutorizacao;

            var atualizarSolicitacaoRecorrenciaCommand = ConverteParaAtualizarSolicitacaoRecorrencia(request, DataAtual, responseInsertAutorizacao.IdAutorizacaoResponse);
            var atualizarSolicitacaoRecorrencia = await _atualizarAutorizacaoRecorrenciaService.Handle(atualizarSolicitacaoRecorrenciaCommand);
            if (atualizarSolicitacaoRecorrencia.StatusCode != StatusCodes.Status200OK)
                return atualizarSolicitacaoRecorrencia;

            string ispb = "12345678";//Obter o ISPB do Agente Responsável pelo Envio;
            string idInformacaoStatus = IdInformacaoStatusGenerator.Gerar(ispb);

            // CENARIO 9 ENVIO DE EVENTO
            ConfirmacaoAutorizacaoRecorrencia confirmacaoAutorizacaoRecorrencia = ConverteParaPostarConfirmacaoRecorrencia(request, codMunIBGE, solicitacaoRecorrenciaBanco.Data, idInformacaoStatus);

            var topic = _config.GetSection("Kafka:Producer:TopicList:AprovarAutorizacaoRecorrencia").Value;
            await _kafkaProducerService.SendObjectAsync(topic, confirmacaoAutorizacaoRecorrencia);

            return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status200OK, string.Empty, "OK"));

        }

        public AprovarSolicRecBanco MapSolicitacaoToCommand(Domain.Entities.SolicitacaoRecorrencia solicitacao)
        {
            return new AprovarSolicRecBanco
            {
                IdAutorizacao = solicitacao.IdAutorizacao,
                IdSolicRecorrencia = solicitacao.IdSolicRecorrencia,
                IdRecorrencia = solicitacao.IdRecorrencia,
                TipoRecorrencia = solicitacao.TipoRecorrencia,
                TipoFrequencia = solicitacao.TipoFrequencia,
                DataInicialRecorrencia = DateTime.Parse(solicitacao.DataInicialRecorrencia),
                DataFinalRecorrencia = string.IsNullOrEmpty(solicitacao.DataFinalRecorrencia) ? null : DateTime.Parse(solicitacao.DataFinalRecorrencia),
                CodigoMoedaSolicRecorr = solicitacao.CodigoMoedaSolicRecorr,
                ValorFixoSolicRecorrencia = string.IsNullOrEmpty(solicitacao.ValorFixoSolicRecorrencia) ? null : decimal.Parse(solicitacao.ValorFixoSolicRecorrencia),
                //IndicadorValorMin = solicitacao.IndicadorValorMin, Na doc nao fala o tipo de dado.
                ValorMinRecebedorSolicRecorr = string.IsNullOrEmpty(solicitacao.ValorMinRecebedorSolicRecorr) ? null : decimal.Parse(solicitacao.ValorMinRecebedorSolicRecorr),
                NomeUsuarioRecebedor = solicitacao.NomeUsuarioRecebedor,
                CpfCnpjUsuarioRecebedor = solicitacao.CpfCnpjUsuarioRecebedor,
                ParticipanteDoUsuarioRecebedor = solicitacao.ParticipanteDoUsuarioRecebedor,
                CpfCnpjUsuarioPagador = solicitacao.CpfCnpjUsuarioPagador,
                ContaUsuarioPagador = int.Parse(solicitacao.ContaUsuarioPagador),
                AgenciaUsuarioPagador = string.IsNullOrEmpty(solicitacao.AgenciaUsuarioPagador) ? null : int.Parse(solicitacao.AgenciaUsuarioPagador),
                NomeDevedor = solicitacao.NomeDevedor,
                CpfCnpjDevedor = solicitacao.CpfCnpjDevedor,
                NumeroContrato = solicitacao.NumeroContrato,
                DescObjetoContrato = solicitacao.DescObjetoContrato,
                DataHoraCriacaoRecorr = DateTime.Parse(solicitacao.DataHoraCriacaoRecorr),
                DataHoraCriacaoSolicRecorr = DateTime.Parse(solicitacao.DataHoraCriacaoSolicRecorr),
                DataHoraExpiracaoSolicRecorr = DateTime.Parse(solicitacao.DataHoraExpiracaoSolicRecorr),
                DataUltimaAtualizacao = DateTime.Parse(solicitacao.DataUltimaAtualizacao),
                SituacaoSolicRecorrencia = solicitacao.SituacaoSolicRecorrencia
            };
        }

        private AtualizarSolicitacaoRecorrenciaCommand ConverteParaAtualizarSolicitacaoRecorrencia(AprovarSolicitacaoRecorrenciaCommand request, DateTime dataAtual, string novoIdAutorizacao)
        {
            return new AtualizarSolicitacaoRecorrenciaCommand
            {
                IdSolicRecorrencia = request.IdSolicRecorrencia,
                IdAutorizacao = novoIdAutorizacao,
                SituacaoSolicRecorrencia = "APRV",
                DataUltimaAtualizacao = dataAtual, //timestamp da jornada 1, conferir.
            };
        }

        private static ConfirmacaoAutorizacaoRecorrencia ConverteParaPostarConfirmacaoRecorrencia(AprovarSolicitacaoRecorrenciaCommand request, string codigoMunicipio, Domain.Entities.SolicitacaoAutorizacaoRecorrenciaDetalhes solicitacaoRecorrencia, string idInformacaoStatus)
        {
            var dataFinalRecorrencia = "";

            if (solicitacaoRecorrencia.DataFinalRecorrencia != null)
                dataFinalRecorrencia = solicitacaoRecorrencia.DataFinalRecorrencia.ToString();


            ConfirmacaoAutorizacaoRecorrencia confirmacaoAutorizacaoRecorrencia = new ConfirmacaoAutorizacaoRecorrencia
            {
                Status = true,
                IdRecorrencia = solicitacaoRecorrencia.IdRecorrencia,
                IdInformacaoStatus = idInformacaoStatus,
                DataInicialRecorrencia = solicitacaoRecorrencia.DataInicialRecorrencia.ToString(),
                DataFinalRecorrencia = dataFinalRecorrencia,
                ValorFixoSolicRecorrencia = Convert.ToDecimal(solicitacaoRecorrencia.ValorFixoSolicRecorrencia),
                NomeUsuarioRecebedor = solicitacaoRecorrencia.NomeUsuarioRecebedor,
                CpfCnpjUsuarioRecebedor = solicitacaoRecorrencia.CpfCnpjUsuarioRecebedor,
                ParticipanteDoUsuarioRecebedor = solicitacaoRecorrencia.ParticipanteDoUsuarioRecebedor,
                CpfCnpjUsuarioPagador = solicitacaoRecorrencia.CpfCnpjUsuarioPagador,
                ContaUsuarioPagador = solicitacaoRecorrencia.ContaUsuarioPagador.ToString(),
                AgenciaUsuarioPagador = solicitacaoRecorrencia.AgenciaUsuarioPagador.ToString(),
                ParticipanteDoUsuarioPagador = request.ParticipanteDoUsuarioRecebedor,
                CodMunIBGE = codigoMunicipio,
                NomeDevedor = solicitacaoRecorrencia.NomeDevedor,
                CpfCnpjDevedor = solicitacaoRecorrencia.CpfCnpjDevedor,
                TpJornada = request.TpJornada,
                SituacaoRecorrencia = "CFDB",
                NumeroContrato = solicitacaoRecorrencia.NumeroContrato,
                DescObjetoContrato = solicitacaoRecorrencia.DescObjetoContrato,
                DataHoraCriacaoRecorr = solicitacaoRecorrencia.DataHoraCriacaoRecorr.ToString(),
                DataUltimaAtualizacao = DateTime.Now.ToString(),
            };

            confirmacaoAutorizacaoRecorrencia.MapearTipoFrequencia(solicitacaoRecorrencia.TipoFrequencia);
            return confirmacaoAutorizacaoRecorrencia;
        }

        private AlterarAutorizacaoCommand ConverteParaAtualizarAutorizacaoRecorrencia(AprovarSolicitacaoRecorrenciaCommand request, InserirAutorizacaoRecorrenciaCommand inserirAutorizacaoRecorrencia, DateTime dataAtual, string idAutorizacao)
        {
            return new AlterarAutorizacaoCommand
            {
                IdAutorizacao = idAutorizacao,
                IdRecorrencia = inserirAutorizacaoRecorrencia.IdRecorrencia,
                TipoSituacaoRecorrencia = "AUT" + Convert.ToInt32(request.TpJornada),
                DataHoraSituacaoRecorrencia = dataAtual,// timestamp da jornada 1, verificar!
                //TipoRecorrencia = "RCUR",
                SituacaoRecorrencia = "INPR",
                TipoFrequencia = request.TipoFrequencia,
                DataInicialAutorizacaoRecorrencia = dataAtual,
                DataFinalAutorizacaoRecorrencia = request.DataFinalRecorrencia,
                //CodigoMoedaAutorizacaoRecorrencia = "BRL",
                ValorMaximoAutorizado = inserirAutorizacaoRecorrencia.ValorMaximoAutorizado,
                FlagPermiteNotificacao = inserirAutorizacaoRecorrencia.FlagPermiteNotificacao,
                CodigoSituacaoCancelamentoRecorrencia = inserirAutorizacaoRecorrencia.CodigoSituacaoCancelamentoRecorrencia
            };
        }

        private InserirAutorizacaoRecorrenciaCommand ConverteParaInserirAutorizacaoRecorrencia(AprovarSolicitacaoRecorrenciaCommand request, string codMunIBGE)
        {
            var inserirAutorizacaoRecorrenciaCommand = new InserirAutorizacaoRecorrenciaCommand
            {
                IdRecorrencia = request.IdRecorrencia,
                SituacaoRecorrencia = "PDNG",
                TipoRecorrencia = "RCUR",
                TipoFrequencia = request.TipoFrequencia,
                DataInicialAutorizacaoRecorrencia = request.DataInicialRecorrencia,
                NomeUsuarioRecebedor = request.NomeUsuarioRecebedor,
                CpfCnpjUsuarioRecebedor = request.CpfCnpjUsuarioRecebedor,
                ParticipanteDoUsuarioRecebedor = request.ParticipanteDoUsuarioRecebedor,
                ContaUsuarioPagador = request.ContaUsuarioPagador,
                CpfCnpjUsuarioPagador = request.CpfCnpjUsuarioPagador,
                ParticipanteDoUsuarioPagador = request.ParticipanteDoUsuarioPagador,
                NumeroContrato = request.NumeroContrato,
                TipoSituacaoRecorrencia = "CRTN",
                DataHoraCriacaoRecorr = request.DataHoraCriacaoRecorr, // Obrigatório se situacaoRecorrencia for diferente de “LIDO”
                //OS ACIMA SÃO OBRIGATORIOS PARA ENVIAR SEGUNDO A HIST-073--------------------------------------
                NomeDevedor = request.NomeDevedor,
                CpfCnpjDevedor = request.CpfCnpjDevedor,
                DataFinalAutorizacaoRecorrencia = request.DataFinalRecorrencia,
                DataProximoPagamento = request.DataInicialRecorrencia,
                CodigoMoedaAutorizacaoRecorrencia = request.CodigoMoedaSolicRecorr,
                ValorRecorrencia = Convert.ToDecimal(request.ValorFixoSolicRecorrencia),
                ValorMaximoAutorizado = request.ValorMaximoAutorizado,
                CodMunIBGE = codMunIBGE,
                AgenciaUsuarioPagador = request.AgenciaUsuarioPagador,
                DescObjetoContrato = request.DescObjetoContrato,
                FlagPermiteNotificacao = true,
                FlagValorMaximoAutorizado = request.ValorMaximoAutorizado != null ? true : false,
                DataUltimaAtualizacao = request.DataUltimaAtualizacao,
                TpRetentativa = "NAO_PERMITE"
            };
            return inserirAutorizacaoRecorrenciaCommand;
        }


        public static string GerarIdAutorizacao()
        {
            string fixedPart = "AR";
            string datePart = DateTime.Now.ToString("yyyyMMdd");
            string sequencePart = GerarSequenciaUnica();
            //return "AR2025052000000000014";
            return fixedPart + datePart + sequencePart;
        }

        public static string GerarSequenciaUnica()
        {
            Random random = new Random();
            string resultado = "";

            for (int i = 0; i < 11; i++)
            {
                resultado += random.Next(0, 10); // Gera um dígito entre 0 e 9
            }

            return resultado;
        }

        private bool ValidaSolicitacaoRecorrencia(Domain.Entities.SolicitacaoAutorizacaoRecorrenciaDetalhes aprovarSolicRecBanco, AprovarSolicitacaoRecorrenciaCommand request)
        {

            // if (aprovarSolicRecBanco.IdSolicRecorrencia != request.IdSolicRecorrencia.ToString())
            //     return false;

            if (aprovarSolicRecBanco.TipoRecorrencia != request.TipoRecorrencia)
                return false;

            if (aprovarSolicRecBanco.TipoFrequencia != request.TipoFrequencia)
                return false;

            if (aprovarSolicRecBanco.DataInicialRecorrencia != request.DataInicialRecorrencia)
                return false;

            if (request.DataFinalRecorrencia != null)
            {
                if (aprovarSolicRecBanco.DataFinalRecorrencia != request.DataFinalRecorrencia)
                    return false;
            }

            if (request.CodigoMoedaSolicRecorr != null)
            {
                if (aprovarSolicRecBanco.CodigoMoedaSolicRecorr != request.CodigoMoedaSolicRecorr.ToString())
                    return false;
            }

            if (request.ValorFixoSolicRecorrencia != null)
            {
                if (aprovarSolicRecBanco.ValorFixoSolicRecorrencia != request.ValorFixoSolicRecorrencia)
                    return false;
            }

            if (request.IndicadorValorMin != null)
            {
                if (aprovarSolicRecBanco.IndicadorValorMin != request.IndicadorValorMin.ToString())
                    return false;
            }

            if (request.ValorMinRecebedorSolicRecorr != null)
            {
                if (aprovarSolicRecBanco.ValorMinRecebedorSolicRecorr != request.ValorMinRecebedorSolicRecorr)
                    return false;
            }

            if (aprovarSolicRecBanco.NomeUsuarioRecebedor != request.NomeUsuarioRecebedor)
                return false;

            if (aprovarSolicRecBanco.CpfCnpjUsuarioRecebedor != request.CpfCnpjUsuarioRecebedor)
                return false;

            if (aprovarSolicRecBanco.ParticipanteDoUsuarioRecebedor != request.ParticipanteDoUsuarioRecebedor)
                return false;

            if (aprovarSolicRecBanco.CpfCnpjUsuarioPagador != request.CpfCnpjUsuarioPagador)
                return false;

            if (aprovarSolicRecBanco.ContaUsuarioPagador != request.ContaUsuarioPagador)
                return false;


            if (request.AgenciaUsuarioPagador != null)
            {
                if (aprovarSolicRecBanco.AgenciaUsuarioPagador != request.AgenciaUsuarioPagador)
                    return false;
            }

            if (request.NomeDevedor != null)
            {
                if (aprovarSolicRecBanco.NomeDevedor != request.NomeDevedor)
                    return false;
            }

            if (request.CpfCnpjDevedor != null)
            {
                if (aprovarSolicRecBanco.CpfCnpjDevedor != request.CpfCnpjDevedor)
                    return false;
            }

            if (aprovarSolicRecBanco.NumeroContrato != request.NumeroContrato)
                return false;

            if (request.DescObjetoContrato != null)
            {
                if (aprovarSolicRecBanco.DescObjetoContrato != request.DescObjetoContrato)
                    return false;
            }

            if (aprovarSolicRecBanco.DataHoraCriacaoRecorr != request.DataHoraCriacaoRecorr)
                return false;

            if (aprovarSolicRecBanco.DataHoraCriacaoSolicRecorr != request.DataHoraCriacaoSolicRecorr)
                return false;

            if (aprovarSolicRecBanco.DataHoraExpiracaoSolicRecorr != request.DataHoraExpiracaoSolicRecorr)
                return false;

            if (aprovarSolicRecBanco.DataUltimaAtualizacao != request.DataUltimaAtualizacao)
                return false;

            if (aprovarSolicRecBanco.SituacaoSolicRecorrencia != "PDNG")
                return false;

            return true;

        }
        //if (!request.IdSolicRecorrencia.Equals(request.IdSolicRecorrencia))
        //    return false;
        //if (!request.IdRecorrencia.Equals(request.IdSolicRecorrencia))
        //    return false;
        //if (!request.TipoRecorrencia.Equals(request.TipoRecorrencia))
        //    return false;
        //if (!request.TipoFrequencia.Equals(request.TipoFrequencia))
        //    return false;
        ////if (!request.DataInicialRecorrencia.Equals(solicitacaoRecorrenciaBanco.DataInicialRecorrencia))
        ////    return true;
        //if (!request.DataFinalRecorrencia.Equals(request.DataFinalRecorrencia))
        //    return false;

        //if (!request.TipoFrequencia.Equals(request.TipoFrequencia))
        //    return false;

        //if (!request.DataFinalRecorrencia.Equals(request.DataFinalRecorrencia))
        //    return false;
        //if (!request.CodigoMoedaSolicRecorr.Equals(request.CodigoMoedaSolicRecorr))
        //    return false;
        ////if (!request.ValorFixoSolicRecorrencia.Equals(solicitacaoRecorrenciaBanco.ValorFixoSolicRecorrencia))
        ////    return true;
        //if (!request.IndicadorValorMin.Equals(request.IndicadorValorMin))
        //    return false;
        ////if (!request.ValorMinRecebedorSolicRecorr.Equals(solicitacaoRecorrenciaBanco.ValorMinRecebedorSolicRecorr))
        ////    return true;
        //if (!request.NomeUsuarioRecebedor.Equals(request.NomeUsuarioRecebedor))
        //    return false;
        //if (!request.CpfCnpjUsuarioRecebedor.Equals(request.CpfCnpjUsuarioRecebedor))
        //    return false;
        //if (!request.ParticipanteDoUsuarioRecebedor.Equals(request.ParticipanteDoUsuarioRecebedor))
        //    return false;
        //if (!request.CpfCnpjUsuarioPagador.Equals(request.CpfCnpjUsuarioPagador))
        //    return false;
        ////if (!request.ContaUsuarioPagador.Equals(solicitacaoRecorrenciaBanco.ContaUsuarioPagador))
        ////    return true;
        ////if (!request.AgenciaUsuarioPagador.Equals(solicitacaoRecorrenciaBanco.AgenciaUsuarioPagador))
        ////    return true;
        //if (!request.NomeDevedor.Equals(request.NomeDevedor))
        //    return false;
        //if (!request.CpfCnpjDevedor.Equals(request.CpfCnpjDevedor))
        //    return false;
        //if (!request.NumeroContrato.Equals(request.NumeroContrato))
        //    return false;
        //if (!request.DescObjetoContrato.Equals(request.DescObjetoContrato))
        //    return false;
        ////if (!request.DataHoraCriacaoRecorr.Equals(solicitacaoRecorrenciaBanco.DataHoraCriacaoRecorr))
        ////    return true;
        ////if (!request.DataHoraCriacaoSolicRecorr.Equals(solicitacaoRecorrenciaBanco.DataHoraCriacaoSolicRecorr))
        ////    return true;
        ////if (!request.DataHoraExpiracaoSolicRecorr.Equals(solicitacaoRecorrenciaBanco.DataHoraExpiracaoSolicRecorr))
        ////    return true;

        //return true;


    }
}
