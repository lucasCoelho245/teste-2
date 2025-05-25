using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Api.Mock
{
    public class TypedApiMetaDataPaginatedResponseMock<T> : ApiMetaDataPaginatedResponseMock
    {
        public T Data { get; set; }

    }
}
