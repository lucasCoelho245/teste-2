using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Domain.Repositories
{
    public interface ITemplateMensagemRepository
    {
        Task<TemplateMensagem> GetTemplateMensagem(string idMensagem);

        Task Insert(TemplateMensagem mensagem);
    }
}
