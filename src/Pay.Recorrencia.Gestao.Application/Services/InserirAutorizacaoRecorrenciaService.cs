using Microsoft.AspNetCore.Http;
using Pay.Recorrencia.Gestao.Application.Commands.IncluirAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Interfaces;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Services
{
    public class InserirAutorizacaoRecorrenciaService : IInserirAutorizacaoRecorrenciaService
    {
        private IAutorizacaoRecorrenciaRepository _autorizacaoRecorrenciaRepository { get; }

        //public UpdateAtualizacoesHandler(ILogger logger, IAutorizacaoRecorrenciaRepository atualizacoesRepository)
        public InserirAutorizacaoRecorrenciaService(IAutorizacaoRecorrenciaRepository autorizacaoRecorrenciaRepository)
        {
            //Logger = logger;
            _autorizacaoRecorrenciaRepository = autorizacaoRecorrenciaRepository;
        }

        public async Task<MensagemPadraoResponse> Handle(InserirAutorizacaoRecorrenciaCommand request)
        {
            if (ValidaInformacoesRecebidas(request))
            {
                DateTime dataAtual = DateTime.Now;

                var consultaAutorizacaoRecorrencia = await _autorizacaoRecorrenciaRepository.ConsultaAutorizacao(request.IdRecorrencia);
                if (consultaAutorizacaoRecorrencia != null)
                {
                    return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-007", "Chave já existente na tabela AUTORIZACAO_RECORRENCIA"));

                }

                var autorizacaoRecorrencia = ConverterCommandParaAutorizacaoRecorrencia(request);
                autorizacaoRecorrencia.DataUltimaAtualizacao = dataAtual;

                //Logger.Information("Inserting InsertAutorizacaoRecorrencia");
                var autorizacaoRecorrenciaCriada = await _autorizacaoRecorrenciaRepository.InsertAutorizacaoRecorrencia(autorizacaoRecorrencia);

                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status200OK, string.Empty, "OK", autorizacaoRecorrencia.IdAutorizacao));
            }
            else
            {
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-002", "Campos não preenchidos corretamente."));
            }

        }

        private AutorizacaoRecorrencia ConverterCommandParaAutorizacaoRecorrencia(InserirAutorizacaoRecorrenciaCommand request)
        {
            if (String.IsNullOrEmpty(request.DataProximoPagamento.ToString()))
                request.DataProximoPagamento = request.DataInicialAutorizacaoRecorrencia;

            var autorizacaoRecorrencia = new AutorizacaoRecorrencia
            {
                IdAutorizacao = GerarIdAutorizacao(),
                IdRecorrencia = request.IdRecorrencia,
                SituacaoRecorrencia = request.SituacaoRecorrencia,
                TipoRecorrencia = request.TipoRecorrencia,
                TipoFrequencia = request.TipoFrequencia,
                DataInicialAutorizacaoRecorrencia = request.DataInicialAutorizacaoRecorrencia,
                DataFinalAutorizacaoRecorrencia = request.DataFinalAutorizacaoRecorrencia,
                CodigoMoedaAutorizacaoRecorrencia = request.CodigoMoedaAutorizacaoRecorrencia,
                ValorRecorrencia = request.ValorRecorrencia,
                ValorMaximoAutorizado = request.ValorMaximoAutorizado,
                MotivoRejeicaoRecorrencia = request.MotivoRejeicaoRecorrencia,
                NomeUsuarioRecebedor = request.NomeUsuarioRecebedor,
                CpfCnpjUsuarioRecebedor = request.CpfCnpjUsuarioRecebedor,
                ParticipanteDoUsuarioRecebedor = request.ParticipanteDoUsuarioRecebedor,
                CodMunIBGE = _autorizacaoRecorrenciaRepository.ConsultarCodMunIBGE(),
                CpfCnpjUsuarioPagador = request.CpfCnpjUsuarioPagador,
                ContaUsuarioPagador = request.ContaUsuarioPagador,
                AgenciaUsuarioPagador = request.AgenciaUsuarioPagador,
                ParticipanteDoUsuarioPagador = request.ParticipanteDoUsuarioPagador,
                NomeDevedor = request.NomeDevedor,
                CpfCnpjDevedor = request.CpfCnpjDevedor,
                NumeroContrato = request.NumeroContrato,
                TipoSituacaoRecorrencia = request.TipoSituacaoRecorrencia,
                DescObjetoContrato = request.DescObjetoContrato,
                CodigoSituacaoCancelamentoRecorrencia = request.CodigoSituacaoCancelamentoRecorrencia,
                DataHoraCriacaoRecorr = request.DataHoraCriacaoRecorr,
                DataUltimaAtualizacao = request.DataUltimaAtualizacao,
                FlagPermiteNotificacao = request.FlagPermiteNotificacao,
                FlagValorMaximoAutorizado = request.FlagValorMaximoAutorizado,
                TpRetentativa = request.TpRetentativa,
                DataProximoPagamento = request.DataProximoPagamento,
                DataAutorizacao = request.DataAutorizacao,
                DataCancelamento = request.DataCancelamento,

            };

            return autorizacaoRecorrencia;

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

        private bool ValidaInformacoesRecebidas(InserirAutorizacaoRecorrenciaCommand autorizacaoRecorrenciaCommand)
        {
            if (autorizacaoRecorrenciaCommand.IdRecorrencia.ToString() == string.Empty)
                return false;

            if (String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.SituacaoRecorrencia) ||
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "PDRC" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "PRRC" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "RCSD" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "PDCF" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "LIDO" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "PDPG" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "CFPG" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "ERPG" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "PRCF" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "CFDB" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "ERFC" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "PDNG" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "CCLD" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "EXPR")
                return false;

            if (String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.TipoRecorrencia) ||
                autorizacaoRecorrenciaCommand.TipoRecorrencia != "RCUR")
                return false;

            if (String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.TipoFrequencia) ||
                autorizacaoRecorrenciaCommand.TipoFrequencia != "MIAN" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "MNTH" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "QURT" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "WEEK" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "YEAR")
                return false;

            if (autorizacaoRecorrenciaCommand.DataInicialAutorizacaoRecorrencia.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.NomeUsuarioRecebedor.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.CpfCnpjUsuarioRecebedor.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.ParticipanteDoUsuarioRecebedor.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.CpfCnpjUsuarioPagador.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.ContaUsuarioPagador.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.ParticipanteDoUsuarioPagador.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.NumeroContrato.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "LIDO")
            {
                if (String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.DataHoraCriacaoRecorr.ToString()))
                    return false;
            }

            if (!autorizacaoRecorrenciaCommand.TipoRecorrencia.ToString().Equals("RCUR"))
                return false;

            if (String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.TipoFrequencia) ||
                autorizacaoRecorrenciaCommand.TipoFrequencia != "MIAN" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "MNTH" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "QURT" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "WEEK" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "YEAR")
            {
                return false;
            }


            if (!String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.CodigoMoedaAutorizacaoRecorrencia?.ToString()) &&
                autorizacaoRecorrenciaCommand.CodigoMoedaAutorizacaoRecorrencia != "BRL")
                return false;


            if (String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia) ||
                                     autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "READ"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "RCSD"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "CRTN"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "AUT1"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "AUT2"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "AUT3"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "AUT4"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "CFDB"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "CCLD")
                return false;

            if (string.IsNullOrEmpty(autorizacaoRecorrenciaCommand.TpRetentativa))
            {
                return false;
            }
            else if (autorizacaoRecorrenciaCommand.TpRetentativa != "NAO_PERMITE" && autorizacaoRecorrenciaCommand.TpRetentativa != "PERMITE_3R_7D")
            {
                return false;
            }

            if (!String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.MotivoRejeicaoRecorrencia) &&
                autorizacaoRecorrenciaCommand.MotivoRejeicaoRecorrencia != "AP13" &&
                autorizacaoRecorrenciaCommand.MotivoRejeicaoRecorrencia != "AP14")
                return false;

            if (!String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia) &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "ACCL" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "CPCL" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "DCSD" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "ERSL" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "FRUD" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "NRES" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "PCFD" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "SLCR" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "SLDB")
                return false;

            if (DateTime.Now < autorizacaoRecorrenciaCommand.DataHoraCriacaoRecorr) // ASSUMINDO QUE DATAAUTORIZACAO É O MOMENTO DA CHAMADA À API
                return false;

            if (autorizacaoRecorrenciaCommand.DataCancelamento < autorizacaoRecorrenciaCommand.DataHoraCriacaoRecorr)
                return false;

            return true;
        }
    }
}