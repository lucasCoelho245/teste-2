using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Options;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using Pay.Recorrencia.Gestao.Domain.Services;
using Pay.Recorrencia.Gestao.Domain.Settings;
using Pay.Recorrencia.Gestao.Shared.Enum;
using Pay.Recorrencia.Gestao.Shared.Helpers;

namespace Pay.Recorrencia.Gestao.Application.Query.QRCode.Traducao
{
    public class TraducaoQRCodeRequestHandler : IRequestHandler<TraducaoQRCodeRequest, TraducaoQRCodeResponse>
    {
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly KafkaTopics _kafkaTopics;
        private readonly IQRCodeLoactionRepository _qrCodeLoactionRepository;
        public TraducaoQRCodeRequestHandler(IKafkaProducerService kafkaProducerService, IOptions<KafkaTopics> kafkaTopics, IQRCodeLoactionRepository qrCodeLoactionRepository)
        {
            _kafkaProducerService = kafkaProducerService;
            _kafkaTopics = kafkaTopics.Value;
            _qrCodeLoactionRepository = qrCodeLoactionRepository;
        }

        public async Task<TraducaoQRCodeResponse> Handle(TraducaoQRCodeRequest request, CancellationToken cancellationToken)
        {
            var qrcodeTools = new QRCodeTools();

            if (qrcodeTools.Valida(request.TxQRCodePadraoEMV))
            {
                var now = DateTime.Now;
                var tipo = qrcodeTools.GetTipo();

                var regras = new Dictionary<TiposQRCode, Func<object>>
                {
                    {
                        TiposQRCode.CPR,
                        () => 2
                    },
                    {
                        TiposQRCode.CDPR,
                        () => {
                            int jornada = 0;

                            if(request.PaymentData.DtExpiracao != DateTime.MinValue) jornada = 3;
                            else if(request.PaymentData.DtVencimento != DateTime.MinValue) jornada = 4;

                            return jornada;
                        }
                    },
                    {
                        TiposQRCode.CEPR,
                        () => "4e"
                    }
                };

                // metodo para recuperar dados do cliente (necessita acesso)
                // metodo para salvar location do qrcode (necessida resolucao de problema no codigo em develop)

                var tipoFrequencia = new Dictionary<string, string>
                {
                    { "SEMANAL", "WEEK" },
                    { "MENSAL", "MNTH"},
                    { "TRIMESTRAL", "QURT" },
                    { "SEMESTRAL", "MIAN" },
                    { "ANUAL", "YEAR" },
                };

                var payloadEventoInclusaoAutorizacaoRecorrencia = new EventoInclusaoAutorizacaoRecorrencia
                {
                    IdRecorrencia = request.PaymentData.IdRecorrencia,
                    TipoRecorrencia = "RCUR",
                    TipoFrequencia = tipoFrequencia.Where(item => item.Key == request.PaymentData.TpPeriodicidade).FirstOrDefault().Value,
                    DataInicialRecorrencia = request.PaymentData.DtPrimeiroPagamento,
                    DataFinalRecorrencia = request.PaymentData.DtFinalPagamento,
                    CodigoMoedaSolicRecorr = String.IsNullOrEmpty(request.PaymentData.VlMinimoRecorrencia.ToString()) ? "BRL" : "",
                    ValorFixoSolicRecorrencia = request.PaymentData.VlRecorrencia,
                    IndicadorValorMin = String.IsNullOrEmpty(request.PaymentData.VlMinimoRecorrencia.ToString()),
                    ValorMinRecebedorSolicRecorr = request.PaymentData.VlMinimoRecorrencia,
                    NomeUsuarioRecebedor = request.PaymentData.NmPessoaRecebedor,
                    CpfCnpjUsuarioRecebedor = request.PaymentData.NrCpfCnpjPessoaRecebedor,
                    ParticipanteDoUsuarioRecebedor = request.PaymentData.NrSpbRecebedor,
                    CpfCnpjUsuarioPagador = request.PaymentData.NrCpfCnpjPessoaPagador,
                    ContaUsuarioPagador = "0000", // retornar da consulta de dados do cliente
                    AgenciaUsuarioPagador = "0", // retornar da consulta
                    ParticipanteDoUsuarioPagador = request.Ispb,
                    NomeDevedor = request.PaymentData.NmPessoaPagador,
                    CpfCnpjDevedor = request.PaymentData.NrCpfCnpjPessoaPagador,
                    NumeroContrato = request.PaymentData.IdInternoOrigem,
                    DescObjetoContrato = request.PaymentData.ObjetoVinculo,
                    TpRetentativa = request.PaymentData.TpRetentativa,
                    DataHoraCriacaoRecorr = now,
                    DataUltimaAtualizacao = now
                };

                var jornada = regras.Where(item => item.Key == tipo).First().Value();

                var payloadEventoAtualizarControleJornada = new EventoAtualizarControleJornada
                {
                    TpJornada = $"Jornada {jornada}",
                    IdRecorrencia = request.PaymentData.IdRecorrencia,
                    IdFimAFim = request.PaymentData.IdFimAFim,
                    SituacaoJornada = "LIDO",
                    DataUltimaAtualizacao = now
                };

                string msgEventoInclusaoAutorizacaoRecorrencia = JsonSerializer.Serialize(payloadEventoInclusaoAutorizacaoRecorrencia);
                string msgEventoAtualizarControleJornada = JsonSerializer.Serialize(payloadEventoAtualizarControleJornada);
                try
                {
                    var qrCodeSalvo = await _qrCodeLoactionRepository.GetByEMVAtivoAsync(request.TxQRCodePadraoEMV);
                    var insertData = new QRCodeLocation
                    {
                        TxQRCodePadraoEMV = request.TxQRCodePadraoEMV,
                        TpJornada = jornada.ToString(),
                        IdFimAFim = request.PaymentData.IdFimAFim,
                        StatusQRCode = "Ativo"
                    };
                    if (qrCodeSalvo == null)
                    {
                        await _qrCodeLoactionRepository.InsertAsync(insertData);
                    }
                    else
                    {
                        await Task.WhenAll(
                            _qrCodeLoactionRepository.UpdateQrCodesStatus([request.TxQRCodePadraoEMV], "Inativo"),
                            _qrCodeLoactionRepository.InsertAsync(insertData)
                        );
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Erro ao salvar a location no banco de dados");
                }
                try
                {
                    var allTasks = Task.WhenAll(
                        _kafkaProducerService.SendMessageWithParameterAsync(_kafkaTopics.AllTopics.InclusaoAutorizacaoRecorrencia, msgEventoInclusaoAutorizacaoRecorrencia),
                        _kafkaProducerService.SendMessageWithParameterAsync(_kafkaTopics.AllTopics.AtualizarControleJornada, msgEventoAtualizarControleJornada)
                    );

                    var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                    var completed = await Task.WhenAny(allTasks, timeoutTask);

                    if (completed == timeoutTask) throw new Exception("Falha ao enviar os eventos para a fila");

                }
                catch (Exception)
                {
                    throw new Exception("Falha ao enviar os eventos para a fila");
                }

                return new TraducaoQRCodeResponse
                {
                    StatusCode = 200,
                    Status = "success",
                    Data = { },
                    Message = "Operação realizada com sucesso"
                };
            }

            throw new Exception("QRCode inválido");
        }
    }
}