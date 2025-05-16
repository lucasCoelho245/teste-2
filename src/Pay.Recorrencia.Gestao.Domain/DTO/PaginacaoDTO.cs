using System.ComponentModel.DataAnnotations;

namespace Pay.Recorrencia.Gestao.Domain.DTO
{
    public class PaginacaoDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "O valor mínimo para a página é 1")]
        public int Page { get; set; } = 1;
        [Range(1, 50, ErrorMessage = "O tamanho da página deve ser entre 1 e 50")]
        public int PageSize { get; set; } = 5;
    }
}