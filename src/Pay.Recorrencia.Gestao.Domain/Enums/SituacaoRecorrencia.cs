using System.Runtime.Serialization;

namespace Pay.Recorrencia.Gestao.Domain.Enums
{
    public enum SituacaoRecorrencia
    {
        [EnumMember(Value = "PDNG")] // Pendente
        PDNG,
        [EnumMember(Value = "INPR")] // Em Processamento
        INPR,
        [EnumMember(Value = "CFDB")] // Confirmado?
        CFDB,
        [EnumMember(Value = "CCLD")] // Cancelado?
        CCLD,
    }
}
