using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Infrastructure.Repositories
{
    public class ControleJornadaRepository : IControleJornadaRepository
    {
        public async Task<IList<ControleJornadaEntrada>> GetControle(string tpJornada, string idRecorrencia, string? idFimAFim)
        {
            // Implemente aqui a lógica para buscar dados (mock ou banco)
            return new List<ControleJornadaEntrada>();
        }

        public async Task IncluirControle(ControleJornada controleJornada)
        {
            // Implemente aqui a lógica para inserir
            await Task.CompletedTask;
        }

        public async Task AtualizarControle(ControleJornada controleJornada)
        {
            // Implemente aqui a lógica para atualizar
            await Task.CompletedTask;
        }
    }
}
