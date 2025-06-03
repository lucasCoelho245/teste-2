using System.ComponentModel.DataAnnotations;
using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;

namespace Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrenciaCanal
{
    public class AlterarAutorizacaoRecorrenciaCanalCommand : IRequest<ApiSimpleResponse>
    {
        [Required(ErrorMessage = "IdAutorizacao é obrigatório")]
        public string IdAutorizacao { get; set; }

        [Required(ErrorMessage = "IdRecorrencia é obrigatório")]
        public string IdRecorrencia { get; set; }

        public bool? FlagValorMaximoAutorizado { get; set; }
        public decimal? ValorMaximoAutorizado { get; set; }
        public bool? FlagPermiteNotificacao { get; set; }
    }
}
