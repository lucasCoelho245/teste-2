using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Detalhes
{
    public class DetalhesControleJornadaAgendamentoResquest : IRequest<DetalhesControleJornadaResponse>
    {
        [Required]
        [RegularExpression("AGND|NTAG|RIFL", ErrorMessage = "O valor de Tpjornada deve ser um dos seguintes: AGND, NTAG, RIFL.")]
        public required string TpJornada { get; set; }
        public string? IdE2E { get; set; }
    }
}
