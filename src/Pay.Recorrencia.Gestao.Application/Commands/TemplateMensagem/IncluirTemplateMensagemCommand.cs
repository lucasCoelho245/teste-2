using System.ComponentModel.DataAnnotations;
using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;

namespace Pay.Recorrencia.Gestao.Application.Commands.TemplateMensagem
{
    public class IncluirTemplateMensagemCommand : IRequest<MensagemPadraoResponse>
    {
        [Required]
        public string IdMensagem { get; set; }
        
        [Required]
        public string TxTemplate { get; set; }
    }
}
