using MediatR;
using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Application.Commands.ConsultaAgendamentoCobranca
{
    public class ConsultaAgendamentoCobrancaCommand : IRequest<List<PixAgendamentoDTO>>
    {
        public string? AgenciaUsuarioPagador { get; set; }
        public string IdTipoContaPagador { get; set; }
        public string ContaUsuarioPagador { get; set; }
        public string? NomeUsuarioRecebedor { get; set; }
    }
}
