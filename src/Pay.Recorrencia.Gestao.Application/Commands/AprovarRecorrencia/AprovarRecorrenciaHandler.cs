using Confluent.Kafka;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Commands.IncluirAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Interfaces;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Helpers;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using Pay.Recorrencia.Gestao.Domain.Services;
using System.Text.Json;

namespace Pay.Recorrencia.Gestao.Application.Commands.AprovarRecorrencia
{
    public class AprovarRecorrenciaHandler : IRequestHandler<AprovarRecorrenciaCommand, MensagemPadraoResponse>
    {
        //private ILogger Logger { get; }
        private static int sequenceCounter = 0;
        private IAutorizacaoRecorrenciaRepository _autorizacaoRecorrenciaRepository { get; }
        private readonly IKafkaProducerService _kafkaProducerService;
        private IInserirAutorizacaoRecorrenciaService _inserirAutorizacaoRecorrenciaService;
        private IAtualizarAutorizacaoService _atualizarAutorizacaoService;
        private IConfiguration _config { get; }

        //public InserirAutorizacaoRecorrenciaHandler(IAutorizacaoRecorrenciaRepository autorizacaoRecorrenciaRepository, ILogger logger)
        public AprovarRecorrenciaHandler(
            IAutorizacaoRecorrenciaRepository autorizacaoRecorrenciaRepository, 
            IKafkaProducerService kafkaProducerService,
            IInserirAutorizacaoRecorrenciaService alterarAtualizacoesService,
            IInserirAutorizacaoRecorrenciaService inserirAutorizacaoRecorrenciaService,
            IAtualizarAutorizacaoService atualizarAutorizacaoService,
            IConfiguration configuration)
        {
            _autorizacaoRecorrenciaRepository = autorizacaoRecorrenciaRepository;
            _kafkaProducerService = kafkaProducerService;
            _inserirAutorizacaoRecorrenciaService = inserirAutorizacaoRecorrenciaService;
            _atualizarAutorizacaoService = atualizarAutorizacaoService;
            _config = configuration;
            //Logger = logger;
        }

        public async Task<MensagemPadraoResponse> Handle(AprovarRecorrenciaCommand request, CancellationToken cancellationToken)
        {
            DateTime dataAtual = DateTime.Now;

            var dataUltimaAtualizacao = dataAtual;

            //Logger.Information("Inserting InsertAutorizacaoRecorrencia");

            string codMunIBGE = _autorizacaoRecorrenciaRepository.ConsultarCodMunIBGE();
            var inserirAutorizacaoRecorrenciaCommand = ConverteParaInserirAutorizacaoRecorrencia(request, codMunIBGE);
            var responseInsertAutorizacao = await _inserirAutorizacaoRecorrenciaService.Handle(inserirAutorizacaoRecorrenciaCommand);

            if (responseInsertAutorizacao.StatusCode != StatusCodes.Status200OK)
                return responseInsertAutorizacao;

            var atualizarAutorizacaoCommand = ConverteParaAtualizarAutorizacaoRecorrencia(request, inserirAutorizacaoRecorrenciaCommand, dataAtual, responseInsertAutorizacao.IdAutorizacaoResponse);
            var responseUpdateAutorizacao = await _atualizarAutorizacaoService.Handle(atualizarAutorizacaoCommand);
            if (responseUpdateAutorizacao.StatusCode != StatusCodes.Status200OK)
                return responseUpdateAutorizacao;

            string ispb = "12345678";//Obter o ISPB do Agente Responsável pelo Envio;
            string idInformacaoStatus = IdInformacaoStatusGenerator.Gerar(ispb);

            ConfirmacaoAutorizacaoRecorrencia confirmacaoAutorizacaoRecorrencia = ConverteParaPostarConfirmacaoRecorrencia(request, codMunIBGE, idInformacaoStatus);

            var topic = _config.GetSection("Kafka:Producer:TopicList:AprovarAutorizacaoRecorrencia").Value;
            await _kafkaProducerService.SendObjectAsync(topic, confirmacaoAutorizacaoRecorrencia);
            return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status200OK, string.Empty, "OK"));
        }

        private ConfirmacaoAutorizacaoRecorrencia ConverteParaPostarConfirmacaoRecorrencia(AprovarRecorrenciaCommand request, string codMunIBGE, string idInformacaoStatus)
        {
            var dataFinalRecorrencia = "";

            if (request.DataFinalRecorrencia != null)
                dataFinalRecorrencia = request.DataFinalRecorrencia.ToString();


            return new ConfirmacaoAutorizacaoRecorrencia
            {
                Status = true,
                IdRecorrencia = request.IdRecorrencia,
                IdInformacaoStatus = idInformacaoStatus,
                DataInicialRecorrencia = request.DataInicialRecorrencia.ToString(),
                DataFinalRecorrencia = dataFinalRecorrencia,
                ValorFixoSolicRecorrencia = Convert.ToDecimal(request.ValorFixoSolicRecorrencia),
                NomeUsuarioRecebedor = request.NomeUsuarioRecebedor,
                CpfCnpjUsuarioRecebedor = request.CpfCnpjUsuarioRecebedor,
                ParticipanteDoUsuarioRecebedor = request.ParticipanteDoUsuarioRecebedor,
                CpfCnpjUsuarioPagador = request.CpfCnpjUsuarioPagador,
                ContaUsuarioPagador = request.ContaUsuarioPagador.ToString(),
                AgenciaUsuarioPagador = request.AgenciaUsuarioPagador.ToString(),
                ParticipanteDoUsuarioPagador = request.ParticipanteDoUsuarioRecebedor,
                CodMunIBGE = codMunIBGE,
                NomeDevedor = request.NomeDevedor,
                CpfCnpjDevedor = request.CpfCnpjDevedor,
                TpJornada = request.TpJornada,
                SituacaoRecorrencia = "CFDB",
                NumeroContrato = request.NumeroContrato,
                DescObjetoContrato = request.DescObjetoContrato,
                DataHoraCriacaoRecorr = request.DataHoraCriacaoRecorr.ToString(),
                DataUltimaAtualizacao = DateTime.Now.ToString(),
            };
        }

        private AlterarAutorizacaoCommand ConverteParaAtualizarAutorizacaoRecorrencia(AprovarRecorrenciaCommand request, InserirAutorizacaoRecorrenciaCommand inserirAutorizacaoRecorrencia, DateTime dataAtual, string idAutorizacao)
        {
            return new AlterarAutorizacaoCommand
            {
                IdAutorizacao = idAutorizacao,
                IdRecorrencia = inserirAutorizacaoRecorrencia.IdRecorrencia,
                TipoSituacaoRecorrencia = "AUT" + Convert.ToInt32(request.TpJornada),
                SituacaoRecorrencia = "INPR",
                DataHoraSituacaoRecorrencia = dataAtual,// timestamp da jornada 1, verificar!
                //TipoRecorrencia = "RCUR",
                TipoFrequencia = request.TipoFrequencia,
                DataInicialAutorizacaoRecorrencia = dataAtual,
                DataFinalAutorizacaoRecorrencia = request.DataFinalRecorrencia,
                //CodigoMoedaAutorizacaoRecorrencia = "BRL",
                ValorMaximoAutorizado = inserirAutorizacaoRecorrencia.ValorMaximoAutorizado,
                FlagPermiteNotificacao = inserirAutorizacaoRecorrencia.FlagPermiteNotificacao,
                CodigoSituacaoCancelamentoRecorrencia = inserirAutorizacaoRecorrencia.CodigoSituacaoCancelamentoRecorrencia
            };
        }

        private static InserirAutorizacaoRecorrenciaCommand ConverteParaInserirAutorizacaoRecorrencia(AprovarRecorrenciaCommand request, string codMunIBGE)
        {
            var flagValorMaximoAutorizado = false;
            if (String.IsNullOrEmpty(request.ValorMaximoAutorizado.ToString()))
                flagValorMaximoAutorizado = true;

            var alterarAutorizacaoCommand = new InserirAutorizacaoRecorrenciaCommand
            {
                IdRecorrencia = request.IdRecorrencia,
                TipoRecorrencia = request.TipoRecorrencia,
                TipoFrequencia = request.TipoFrequencia,
                DataInicialAutorizacaoRecorrencia = request.DataInicialRecorrencia,
                DataFinalAutorizacaoRecorrencia = request.DataFinalRecorrencia,
                SituacaoRecorrencia = "PDNG",
                CodigoMoedaAutorizacaoRecorrencia = request.CodigoMoedaSolicRecorr,
                ValorRecorrencia = request.ValorFixoSolicRecorrencia,
                ValorMaximoAutorizado = request.ValorMaximoAutorizado,
                NomeUsuarioRecebedor = request.NomeUsuarioRecebedor,
                CpfCnpjUsuarioRecebedor = request.CpfCnpjUsuarioRecebedor,
                ParticipanteDoUsuarioRecebedor = request.ParticipanteDoUsuarioRecebedor,
                CodMunIBGE = codMunIBGE,
                CpfCnpjUsuarioPagador = request.CpfCnpjUsuarioPagador,
                ContaUsuarioPagador = request.ContaUsuarioPagador,
                AgenciaUsuarioPagador = request.AgenciaUsuarioPagador,
                ParticipanteDoUsuarioPagador = request.ParticipanteDoUsuarioPagador,
                NomeDevedor = request.NomeDevedor,
                CpfCnpjDevedor = request.CpfCnpjDevedor,
                NumeroContrato = request.NumeroContrato,
                DescObjetoContrato = request.DescObjetoContrato,
                TipoSituacaoRecorrencia = "CRTN",
                DataHoraCriacaoRecorr = request.DataHoraCriacaoRecorr,
                FlagPermiteNotificacao = true,
                FlagValorMaximoAutorizado = flagValorMaximoAutorizado,
                TpRetentativa = request.TpRetentativa,
            };
            return alterarAutorizacaoCommand;
        }

        private static async Task<IProducer<Null, string>> EnviodeEventoParaKafkaLocal(AprovarRecorrenciaCommand aprovacaoRecorrencia)
        {
            var json = JsonSerializer.Serialize(aprovacaoRecorrencia);
            var topic = "autorizacao-recorrencia-confirmada";

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = json });

                Console.WriteLine($"Evento enviado! Offset: {result.Offset}, Partition: {result.Partition}");
            }
            catch (ProduceException<Null, string> ex)
            {
                Console.WriteLine($"Falha ao enviar: {ex.Error.Reason}");
            }

            return producer;
        }
    }
}
