using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.PayPagamentoProcessado.Validation.IValidation
{
    public interface IValidacaoSolicitacao
    {
        string Validar(SolicitacaoRecorrenciaEntrada dados);
    }
}
