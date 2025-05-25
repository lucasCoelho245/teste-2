using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pay.Recorrencia.Gestao.Domain.Enums
{
    public enum TipoRetentativa
    {
        [EnumMember(Value = "NAO_PERMITE")]
        NAOPERMITE,
        [EnumMember(Value = "PERMITE_3R_7D")]
        PERMITE
    }
}
