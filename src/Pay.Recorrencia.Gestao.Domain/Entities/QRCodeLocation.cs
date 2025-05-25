namespace Pay.Recorrencia.Gestao.Domain.Entities
{
    public class QRCodeLocation
    {
        public string TxQRCodePadraoEMV { get; set; }
        public string TpJornada { get; set; }
        public string IdFimAFim { get; set; }
        public string StatusQRCode { get; set; } = "Ativo";
    }
}