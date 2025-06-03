using System.Text.RegularExpressions;
using Pay.Recorrencia.Gestao.Shared.Enum;

namespace Pay.Recorrencia.Gestao.Shared.Helpers;

public class QRCodeTools
{
    private TiposQRCode _tipo;
    //QR Codes composto apenas com parâmetros de recorrência
    private readonly string CPR = @"00020126180014(.{1,14})5204(.{4})5303(.{3})5802(.{2})5913(.{1,13})6008(.{1,8})62070503\*\*\*80740014(.{1,14})2552(.{1,52})00026304F2DA";

    //QR Code composto com dados dinâmicos (pagamento imediato ou com vencimento) e parâmetros de recorrência
    private readonly string CDPR = @"00020101021226700014(.{1,14})2548(.{1,48})5204(.{4})5303(.{3})5802(.{2})5913(.{1,13})6008(.{1,8})62070503\*\*\*80740014(.{1,14})2552(.{1,52})00026304FB42";

    //QR Code composto com dados estáticos e parâmetros de recorrência:
    private readonly string CEPR = @"00020126580014(.{1,14})0136(.{1,48})5204(.{4})5303(.{3})5406(.{1,6})5802(.{2})5913(.{1,13})6008(.{1,8})62070503\*\*\*80740014(.{1,14})2552(.{1,52})000263042875";
    
    public bool Valida(string emv)
    {
        var regras = new Dictionary<TiposQRCode, Func<Match>>
        {
            {
                TiposQRCode.CPR,
                () => {
                    Regex regex = new(CPR);
                    return regex.Match(emv);
                }
            },
             {
                TiposQRCode.CDPR,
                () => {
                    Regex regex = new(CDPR);
                    return regex.Match(emv);
                }
            },
            {
                TiposQRCode.CEPR,
                () => {
                    Regex regex = new(CEPR);
                    return regex.Match(emv);
                }
            }
        };
        var regraSelecionada = regras.FirstOrDefault(regra => regra.Value().Success);
        _tipo = regraSelecionada.Key;

        return regraSelecionada.Value != null;
    }
    public TiposQRCode GetTipo()
    {
        return _tipo;
    }
}
