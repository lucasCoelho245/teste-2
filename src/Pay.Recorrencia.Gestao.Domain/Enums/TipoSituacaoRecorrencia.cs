using System.Runtime.Serialization;

namespace Pay.Recorrencia.Gestao.Domain.Enums
{
    public enum TipoSituacaoRecorrencia
    {
        [EnumMember(Value = "READ")] // ?
        READ,
        [EnumMember(Value = "CRTN")] // ?
        CRTN,
        [EnumMember(Value = "AUT1")] // ?
        AUT1,
        [EnumMember(Value = "AUT2")] // ?
        AUT2,
        [EnumMember(Value = "AUT3")] // ?
        AUT3,
        [EnumMember(Value = "AUT4")] // ?
        AUT4,
        [EnumMember(Value = "CFDB")] // ?
        CFDB,
        [EnumMember(Value = "CCLD")] // ?
        CCLD,
    }
}
