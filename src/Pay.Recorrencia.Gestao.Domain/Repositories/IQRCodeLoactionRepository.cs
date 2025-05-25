using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Domain.Repositories
{
    public interface IQRCodeLoactionRepository
    {
        Task InsertAsync(QRCodeLocation qRCodeLocation);
        Task<QRCodeLocation?> GetByEMVAtivoAsync(string txQRCodePadraoEMV);
        Task UpdateQrCodesStatus(string[] txQRCodePadraoEMVList, string status);
    }
}