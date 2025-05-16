using System.ComponentModel.DataAnnotations;

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
    }
    public class GetListaSolicAutorizacaoRecDTOPaginada : PaginacaoDTO
    {
        public required string CpfCnpjUsuarioPagador { get; set; }
        public string? SituacaoSolicRecorrencia { get; set; }
        public string? NomeUsuarioRecebedor { get; set; }
        public string? AgenciaUsuarioPagador { get; set; }
        public required string ContaUsuarioPagador { get; set; }
    }
    public class GetSolicAutorizacaoRecDTO
    {
        public required string IdSolicRecorrencia { get; set; }
    }
    public class GetSolicAutorizacaoRecDTOPaginada : PaginacaoDTO
    {
        public required string IdSolicRecorrencia { get; set; }
    }
}