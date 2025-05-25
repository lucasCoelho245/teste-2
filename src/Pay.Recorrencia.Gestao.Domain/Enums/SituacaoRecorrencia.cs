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
        [EnumMember(Value = "PDRC")]
        PDRC,
        [EnumMember(Value = "PRRC")]
        PRRC,
        [EnumMember(Value = "RCSD")]
        RCSD,
        [EnumMember(Value = "PDCF")]
        PDCF,
        [EnumMember(Value = "LIDO")]
        LIDO,
        [EnumMember(Value = "PDPG")]
        PDPG,
        [EnumMember(Value = "CFPG")]
        CFPG,
        [EnumMember(Value = "ERPG")]
        ERPG,
        [EnumMember(Value = "PRCF")]
        PRCF,
        [EnumMember(Value = "ERCF")]
        ERCF

    }
}
