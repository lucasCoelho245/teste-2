using System.ComponentModel.DataAnnotations;
using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;

namespace Pay.Recorrencia.Gestao.Application.Commands.ConfirmacaoAutorizacaoRecorr
{
    public class ReceberConfirmacaoAutorizacaoRecorrCommand : IRequest<MensagemPadraoResponse>
    {
        [Required]
        public string Status { get; set; }
        [Required]
        public string IdRecorrencia { get; set; }
        [Required]
        public string IdInformacaoStatus { get; set; }
        [Required]
        public string TipoFrequencia { get; set; }
        [Required]
        public DateTime DataInicialRecorrencia { get; set; }
        public DateTime? DataFinalRecorrencia { get; set; }
        public decimal? ValorFixoSolicRecorrencia { get; set; }
        public string? CodMunIBGE { get; set; }
        [Required]
        public string NomeUsuarioRecebedor { get; set; }
        [Required]
        public string CpfCnpjUsuarioRecebedor { get; set; }
        [Required]
        public string ParticipanteDoUsuarioRecebedor { get; set; }
        [Required]
        public string CpfCnpjUsuarioPagador { get; set; }
        [Required]
        public string ContaUsuarioPagador { get; set; }
        public string? AgenciaUsuarioPagador { get; set; }
        [Required]
        public string ParticipanteDoUsuarioPagador { get; set; }
        public string? NomeUsuarioDevedor { get; set; }
        public string? CpfCnpjUsuarioDevedor { get; set; }
        public string? SituacaoRecorrencia { get; set; }
        [Required]
        public string NumeroContrato { get; set; }
        public string? DescObjetoContrato { get; set; }
        [Required]
        public string TpJornada { get; set; }
        [Required]
        public DateTime DataHoraCriacaoRecorr { get; set; }
        [Required]
        public DateTime DataUltimaAtualizacao { get; set; }
    }
}
