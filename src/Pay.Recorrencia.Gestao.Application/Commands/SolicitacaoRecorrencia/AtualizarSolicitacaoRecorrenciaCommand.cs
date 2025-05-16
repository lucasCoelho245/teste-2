using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoRecorrencia
{
    public class AtualizarSolicitacaoRecorrenciaCommand : IRequest<MensagemPadraoResponse>
    {
        [JsonIgnore]
        public string? IdSolicRecorrencia { get; set; }

        public string? IdAutorizacao { get; set; }

        public string? SituacaoSolicRecorrencia { get; set; }

        [Required]
        public DateTime DataUltimaAtualizacao { get; set; }
    }
}
