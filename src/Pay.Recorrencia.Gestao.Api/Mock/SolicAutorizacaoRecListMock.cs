using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Api.Mock
{
    public class SolicAutorizacaoRecListMock
    {
        public IEnumerable<SolicAutorizacaoRecList> Items { get; set; }

        public PaginationMock pagination { get; set; }
    }
}
