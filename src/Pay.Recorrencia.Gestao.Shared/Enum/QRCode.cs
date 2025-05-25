namespace Pay.Recorrencia.Gestao.Shared.Enum
{
    public enum TiposQRCode
    {
        CPR, // QR Codes composto apenas com parâmetros de recorrência
        CDPR, // QR Code composto com dados dinâmicos (pagamento imediato ou com vencimento) e parâmetros de recorrência   
        CEPR //QR Code composto com dados estáticos e parâmetros de recorrência:
    }
}