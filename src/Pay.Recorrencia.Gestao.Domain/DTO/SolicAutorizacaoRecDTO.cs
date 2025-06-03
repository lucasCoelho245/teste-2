using System.ComponentModel.DataAnnotations;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Validators;

namespace Pay.Recorrencia.Gestao.Domain.DTO
{
    public class GetListaSolicAutorizacaoRecDTO
    {
        [Required]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "Tamanho de documento inválido.")]
        public required string CpfCnpjUsuarioPagador { get; set; }
        public string? SituacaoSolicRecorrencia { get; set; }
        public string? NomeUsuarioRecebedor { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "O campo AgenciaUsuarioPagador deve conter apenas números.")]
        public string? AgenciaUsuarioPagador { get; set; }
        [MinLength(4)]
        [RegularExpression(@"^\d+$", ErrorMessage = "O campo ContaUsuarioPagador deve conter apenas números.")]
        public required string ContaUsuarioPagador { get; set; }
        [DtExpiracao]
        public DateTime DtExpiracaoInicio { get; set; }
        [DtExpiracao]
        public DateTime DtExpiracaoFim { get; set; }
    }
    public class GetListaSolicAutorizacaoRecDTOPaginada : PaginacaoDTO
    {
        public required string CpfCnpjUsuarioPagador { get; set; }
        public string? SituacaoSolicRecorrencia { get; set; }
        public string? NomeUsuarioRecebedor { get; set; }
        public string? AgenciaUsuarioPagador { get; set; }
        public required string ContaUsuarioPagador { get; set; }
        public DateTime DtExpiracaoInicio { get; set; }
        public DateTime DtExpiracaoFim { get; set; }
    }
    public class GetSolicAutorizacaoRecDTO
    {
        [IDsDesatlhesSolicRecorrencia]
        public string? IdSolicRecorrencia { get; set; }
        [IDsDesatlhesSolicRecorrencia]
        public string? IdRecorrencia { get; set; }
    }
    public class GetSolicAutorizacaoRecDTOPaginada : PaginacaoDTO
    {
        public string IdSolicRecorrencia { get; set; }
        public string IdRecorrencia { get; set; }
    }
    public class SolicitacaoAutorizacaoRecorrenciaDetalhesDTO : SolicitacaoAutorizacaoRecorrenciaDetalhes
    {
        public new bool IndicadorValorMin { get; set; }
    }
}