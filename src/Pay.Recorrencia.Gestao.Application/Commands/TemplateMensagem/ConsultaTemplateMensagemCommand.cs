using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;

namespace Pay.Recorrencia.Gestao.Application.Commands.TemplateMensagem
{
    public class ConsultaTemplateMensagemCommand: IRequest<TemplateMensagemResponse>
    {
        public string? IdMensagem { get; set; }
    }
}
