using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;

namespace Pay.Recorrencia.Gestao.Application.Commands.ConfirmacaoAutorizacaoRecorr
{
    public class ValidarConfirmacaoCommand : IRequest<MensagemPadraoResponse>
    {
        private readonly ReceberConfirmacaoAutorizacaoRecorrCommand _RequestConfirmacao;

        public ValidarConfirmacaoCommand(ReceberConfirmacaoAutorizacaoRecorrCommand requestConfirmacao)
        {
            _RequestConfirmacao = requestConfirmacao;
        }

        public ReceberConfirmacaoAutorizacaoRecorrCommand GetRequestConfirmacao()
        {
            return _RequestConfirmacao;
        }
    }
}
