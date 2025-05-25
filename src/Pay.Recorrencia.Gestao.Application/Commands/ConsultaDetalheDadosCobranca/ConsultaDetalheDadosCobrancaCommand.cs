using MediatR;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Application.Commands.ConsultaDetalheDadosCobranca
{
    public class ConsultaDetalheDadosCobrancaCommand : IRequest<DetalheDadosCobranca>
    {
        public string? AgenciaUsuarioPagador { get; set; }
        public string IdTipoContaPagador { get; set; }
        public string ContaUsuarioPagador { get; set; }
        public string IdRecorrencia { get; set; }
        public string IdOperacao { get; set; }
    }
}
