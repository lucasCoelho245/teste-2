using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Pay.Recorrencia.Gestao.Application.Query.SolicAutorizacaoRec.Detalhes;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using Pay.Recorrencia.Gestao.Domain.Services;

namespace Pay.Recorrencia.Gestao.Application.Commands.CancelarAutorizacaoRecorrencia;

public class CancelarAutorizacaoRecorrenciaHandler : IRequestHandler<CancelarAutorizacaoRecorrenciaCommand, MensagemPadraoResponse>
{
    private ISolicitacaoRecorrenciaRepository _solicitacaoRecorrenciaRepository;
    private IAutorizacaoRecorrenciaRepository _autorizacaoRecorrenciaRepository;
    private IControleJornadaRepository _controleJornadaRepository;
    private IMapper _mapper { get; }

    public CancelarAutorizacaoRecorrenciaHandler(
        ISolicitacaoRecorrenciaRepository solicitacaoRecorrenciaRepository,
        IAutorizacaoRecorrenciaRepository autorizacaoRecorrenciaRepository,
        IControleJornadaRepository controleJornadaRepository,
        IMapper mapper
    )
    {
        _solicitacaoRecorrenciaRepository = solicitacaoRecorrenciaRepository;
        _autorizacaoRecorrenciaRepository = autorizacaoRecorrenciaRepository;
        _controleJornadaRepository = controleJornadaRepository;
        _mapper = mapper;
    }

    public async Task<MensagemPadraoResponse> Handle(CancelarAutorizacaoRecorrenciaCommand request, CancellationToken cancellationToken)
    {
        DateTime DataAtual = DateTime.Now;
        RecorrenciaUnificadaDto dto = new();
        bool isSolicitacaoRecorrencia =
            !(request.IdMotivo == "ERSL" || request.IdMotivo == "NRES" || request.IdMotivo == "PCFD");

        if (!isSolicitacaoRecorrencia)
        {
            AutorizacaoRecorrencia autorizacaoRecorrencia =
                _autorizacaoRecorrenciaRepository.ConsultaAutorizacao(null, request.IdRecorrencia).Result;

            if (autorizacaoRecorrencia == null)
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest,
                    "ERRO-PIXAUTO-003", "Autorização de recorrência não encontrada"));

            dto = mapToRecorrenciaUnificadaDto(autorizacaoRecorrencia, null);
        }
        else
        {
            var objConsultaSolicitacaoAutorizacao = new DetalhesSolicAutorizacaoRecRequest()
            {
                IdRecorrencia = request.IdRecorrencia,
            };

            SolicAutorizacaoRecNonPagination solicitacaoRecorrenciaBanco =
                _solicitacaoRecorrenciaRepository.GetAsync(objConsultaSolicitacaoAutorizacao).Result;

            if (solicitacaoRecorrenciaBanco == null)
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest,
                    "ERRO-PIXAUTO-003", "Solicitação de recorrência não encontrada"));

            dto = mapToRecorrenciaUnificadaDto(null, solicitacaoRecorrenciaBanco);
        }

        string motivoRejeicao = ValidarCancelamentoRecorrencia(
            dto,
            request.NrCpfCnpjSolicitanteCancelamento,
            request.IdMotivo != "ERSL" && request.IdMotivo != "NRES" && request.IdMotivo != "PCFD",
            request.IdMotivo == "ERSL" || request.IdMotivo == "NRES" || request.IdMotivo == "PCFD"
        );

        if (!string.IsNullOrEmpty(motivoRejeicao))
        {
            var respostaRejeicao = new MensagemRecorrenciaResponse
            {
                IdRecorrencia = request.IdRecorrencia,
                IdInformacaoCancelamento = request.IdInformacaoCancelamento,
                Erros = new List<ErroRecorrencia>
                {
                    new ErroRecorrencia
                    {
                        Code = -51,
                        Msg = $"{motivoRejeicao} - Cancelamento rejeitado"
                    }
                },
                OK = false
            };

            //enviar mensagem kafka

            return await Task.FromResult(new MensagemPadraoResponse(
                StatusCodes.Status400BadRequest,
                $"ERRO-PIXAUTO-{motivoRejeicao}",
                $"Cancelamento rejeitado. Motivo: {motivoRejeicao}"
            ));
        }

        if (isSolicitacaoRecorrencia)
        {
            //cenário 5
        }
        else
        {
            //cenário 6 Cancelamento interno da recorrência
            var resultadoCancelamento = await ExecutarCancelamentoInternoSolicitacao(
                dto.IdSolicRecorrencia,
                request.IdMotivo
            );

            //cenário 8
            if (resultadoCancelamento.StatusCode == StatusCodes.Status200OK)
            {
                // TODO: Implementar chamadas paralelas para cenários 08 e 10
                // Pode ser feito usando Task.WhenAll ou background jobs

                // Início do cenário 09.1
                var listaAgendamentos = await _controleJornadaRepository.BuscarPorIdRecorrenciaAsync(request.IdRecorrencia);
                if (listaAgendamentos != null && listaAgendamentos.Any())
                {
                    var limiteData = DataAtual.Hour < 22 ? DataAtual.Date.AddDays(1) : DataAtual.Date.AddDays(2);

                    var agendamentosFiltrados = listaAgendamentos
                        .Where(x =>
                            new[] { "AGND", "NTAG", "RIFL" }.Contains(x.TpJornada) &&
                            x.SituacaoJornada == "Agendamento Programado" &&
                            x.DtAgendamento >= limiteData
                        ).ToList();

                    if (agendamentosFiltrados.Any())
                    {
                        // TODO: publicar eventos do cenário 09.2
                    }
                }
                // Fim do cenário 09.1
            }

            return resultadoCancelamento;
        }
        // else
        // {
        //     // TODO: Implementar cenário 06
        // }

        return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status200OK, string.Empty, "OK"));
    }

    private RecorrenciaUnificadaDto mapToRecorrenciaUnificadaDto(AutorizacaoRecorrencia? autorizacao,
        SolicAutorizacaoRecNonPagination? solicitacao)
    {
        RecorrenciaUnificadaDto dto = new();
        if (autorizacao != null)
            dto = _mapper.Map<RecorrenciaUnificadaDto>(autorizacao);
        else
            dto = _mapper.Map<RecorrenciaUnificadaDto>(solicitacao);

        return dto;
    }

    private string ValidarCancelamentoRecorrencia(RecorrenciaUnificadaDto dto, string cpfCnpjSolicitante, bool consultaCenario02Realizada, bool consultaCenario03Realizada)
    {
        if (consultaCenario02Realizada)
        {
            if (dto.SituacaoSolicRecorrencia == "EXPR" || dto.SituacaoSolicRecorrencia == "CCLD")
                return "AP07";

            if (dto == null)
                return "AP09";
        }

        bool cpfCnpjValido = false;

        if (cpfCnpjSolicitante == dto.CpfCnpjUsuarioPagador ||
            cpfCnpjSolicitante == dto.CpfCnpjUsuarioRecebedor)
        {
            cpfCnpjValido = true;
        }

        if (cpfCnpjSolicitante == "61033106000186" ||
            cpfCnpjSolicitante == "60779196000196")
        {
            cpfCnpjValido = true;
        }

        if (dto.ParticipanteDoUsuarioRecebedor != null &&
            cpfCnpjSolicitante.Length >= 8 &&
            cpfCnpjSolicitante.Substring(0, 8) == dto.ParticipanteDoUsuarioRecebedor)
        {
            cpfCnpjValido = true;
        }

        if (!cpfCnpjValido)
            return "AP10";

        if (consultaCenario03Realizada)
        {
            if (dto == null)
                return "MD01";

            string[] situacoesSolicInvalidas = { "INRJ", "RJCT", "INRC", "RCSD" };
            if (situacoesSolicInvalidas.Contains(dto.SituacaoSolicRecorrencia))
                return "MD01";

            if (dto.SituacaoRecorrencia != null &&
                dto.SituacaoRecorrencia != "CFDB" &&
                dto.SituacaoRecorrencia != "EXPR")
                return "MD01";
        }

        if (dto.SituacaoSolicRecorrencia == "EXPR" || dto.SituacaoRecorrencia == "EXPR")
            return "MD20";

        return string.Empty;
    }

    private async Task<MensagemPadraoResponse> ExecutarCancelamentoInternoSolicitacao(string idSolicRecorrencia, string tpMotivo, int maxRetries = 3)
    {
        for (int tentativa = 1; tentativa <= maxRetries; tentativa++)
        {
            try
            {
                var requestCancelamento = new SolicitacaoAutorizacaoRecorrenciaUpdateDTO()
                {
                    IdSolicRecorrencia = idSolicRecorrencia,
                    SituacaoSolicRecorrencia = "CCLD",
                    CodigoSituacaoCancelamentoRecorrencia = tpMotivo
                };

                await _solicitacaoRecorrenciaRepository.Update(requestCancelamento);

                if (tentativa == maxRetries)
                {
                    return new MensagemPadraoResponse(
                        StatusCodes.Status400BadRequest,
                        "ERRO-PIXAUTO-CANCEL",
                        $"Falha após {maxRetries} tentativas de cancelamento: "
                    );
                }

                await Task.Delay(1000 * tentativa);
            }
            catch (Exception ex)
            {
                if (tentativa == maxRetries)
                {
                    return new MensagemPadraoResponse(
                        StatusCodes.Status500InternalServerError,
                        "ERRO-PIXAUTO-INTERNAL",
                        $"Erro interno no cancelamento: {ex.Message}"
                    );
                }
                await Task.Delay(1000 * tentativa);
            }
        }

        return new MensagemPadraoResponse(
            StatusCodes.Status500InternalServerError,
            "ERRO-PIXAUTO-UNEXPECTED",
            "Erro inesperado no processo de cancelamento"
        );
    }
}
