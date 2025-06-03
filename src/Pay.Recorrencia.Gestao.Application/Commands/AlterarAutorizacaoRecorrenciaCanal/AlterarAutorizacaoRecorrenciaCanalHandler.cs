using MediatR;
using Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Query.AutorizacaoRec.Detalhes;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrenciaCanal
{
    public class AlterarAutorizacaoRecorrenciaCanalHandler : IRequestHandler<AlterarAutorizacaoRecorrenciaCanalCommand, ApiSimpleResponse>
    {
        private readonly IMediator _mediator;

        public AlterarAutorizacaoRecorrenciaCanalHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ApiSimpleResponse> Handle(AlterarAutorizacaoRecorrenciaCanalCommand request, CancellationToken cancellationToken)
        {
            DetalhesAutorizacaoRecRequest requestDtoPaginado = new DetalhesAutorizacaoRecRequest()
            {
                IdAutorizacao = request.IdAutorizacao,
                IdRecorrencia = request.IdRecorrencia
            };

            DetalhesAutorizacaoRecResponse detalhesResponse = await _mediator.Send(requestDtoPaginado, cancellationToken);
            var autorizacao = detalhesResponse?.Data;

            if (autorizacao == null)
            {
                return new ApiSimpleResponse("NOK", "ERRO-PIXAUTO-011: Autorização de recorrência não encontrada.");
            }

            AlterarAutorizacaoCommand alterarAutorizacaoCommand = new AlterarAutorizacaoCommand()
            {
                IdAutorizacao = autorizacao.IdAutorizacao,
                IdRecorrencia = autorizacao.IdRecorrencia,
                ValorMaximoAutorizado = request.ValorMaximoAutorizado ?? autorizacao.ValorMaximoAutorizado,
                FlagValorMaximoAutorizado = request.FlagValorMaximoAutorizado ?? autorizacao.FlagValorMaximoAutorizado,
                FlagPermiteNotificacao = request.FlagPermiteNotificacao ?? autorizacao.FlagPermiteNotificacao,
                MotivoRejeicaoRecorrencia = autorizacao.MotivoRejeicaoRecorrencia,
                CodigoSituacaoCancelamentoRecorrencia = autorizacao.CodigoSituacaoCancelamentoRecorrencia,
                SituacaoRecorrencia = autorizacao.SituacaoRecorrencia,
                TipoSituacaoRecorrencia = autorizacao.TipoSituacaoRecorrencia,
                DataHoraSituacaoRecorrencia = autorizacao.DataHoraCriacaoRecorr
            };

            MensagemPadraoResponse resultadoAlteracaoAutorizacao = await _mediator.Send(alterarAutorizacaoCommand, cancellationToken);

            if (resultadoAlteracaoAutorizacao.StatusCode == 200)
                return new ApiSimpleResponse("Ok", "Autorização de recorrência alterada com sucesso.");
            else
                return new ApiSimpleResponse(resultadoAlteracaoAutorizacao.Error.Code, resultadoAlteracaoAutorizacao.Error.Message);
        }
    }
}