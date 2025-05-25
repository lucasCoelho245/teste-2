using System.Threading.Tasks;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Application.Services.Interfaces
{
    public interface ISolicitacaoRecorrenciaService
    {
        Task IncluirAsync(SolicitacaoRecorrenciaEntrada entrada, bool status);
    }
}
