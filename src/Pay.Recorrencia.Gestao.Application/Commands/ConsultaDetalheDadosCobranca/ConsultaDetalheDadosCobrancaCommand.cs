using MediatR;
using Pay.Recorrencia.Gestao.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Pay.Recorrencia.Gestao.Application.Commands.ConsultaDetalheDadosCobranca
{
    public class ConsultaDetalheDadosCobrancaCommand : IRequest<DetalheDadosCobranca>
    {
        public string? AgenciaUsuarioPagador { get; set; }

        [Required]
        [RegularExpression("^(CACC|SLRY|SVGS|TRAN|CAHO|CCTE|DBMO|DBMI|DORD)$")]
        public string IdTipoContaPagador { get; set; }

        [Required]
        public string ContaUsuarioPagador { get; set; }

        [Required]
        public string IdRecorrencia { get; set; }

        [Required]
        public string IdOperacao { get; set; }
    }
}
