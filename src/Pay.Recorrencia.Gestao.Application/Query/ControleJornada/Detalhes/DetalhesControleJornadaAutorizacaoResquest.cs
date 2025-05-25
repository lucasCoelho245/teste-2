using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Detalhes
{
    public class DetalhesControleJornadaAutorizacaoResquest : IRequest<DetalhesControleJornadaResponse>
    {
        [Required]
        [RegularExpression("Jornada 1|Jornada 2|Jornada 3|Jornada 4", ErrorMessage = "O valor de Tpjornada deve ser um dos seguintes: Jornada 1, Jornada 2, Jornada 3, Jornada 4.")]
        public required string TpJornada { get; set; }
        public string? IdRecorrencia { get; set; }
    }
}
