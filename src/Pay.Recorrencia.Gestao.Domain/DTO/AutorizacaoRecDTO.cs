using System.ComponentModel.DataAnnotations;

namespace Pay.Recorrencia.Gestao.Domain.DTO
{
    public class GetListaAutorizacaoRecDTO
    {
        [Required]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "Tamanho de documento inválido.")]
        public required string CpfCnpjUsuarioPagador { get; set; }
        public string? SituacaoRecorrencia { get; set; }
        public string? NomeUsuarioRecebedor { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "O campo AgenciaUsuarioPagador deve conter apenas números.")]
        public string? AgenciaUsuarioPagador { get; set; }
        [MinLength(4)]
        [RegularExpression(@"^\d+$", ErrorMessage = "O campo ContaUsuarioPagador deve conter apenas números.")]
        public required string ContaUsuarioPagador { get; set; }
    }
    public class GetListaAutorizacaoRecDTOPaginada : PaginacaoDTO 
    {
        public required string CpfCnpjUsuarioPagador { get; set; }
        public string? SituacaoRecorrencia { get; set; }
        public string? NomeUsuarioRecebedor { get; set; }
        public string? AgenciaUsuarioPagador { get; set; }
        public required string ContaUsuarioPagador { get; set; }
    }
    public class GetAutorizacaoRecDTO
    {
        public required string IdRecorrencia { get; set; }
        public required string IdAutorizacao { get; set; }
    }
    public class GetAutorizacaoRecDTOPaginada : PaginacaoDTO 
    {
        public required string IdRecorrencia { get; set; }
        public required string IdAutorizacao { get; set; }
    }
}