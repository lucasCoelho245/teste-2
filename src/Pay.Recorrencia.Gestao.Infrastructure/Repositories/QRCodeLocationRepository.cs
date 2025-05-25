using DataAccess.Domain;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Infrastructure.Repositories
{
    public class QRCodeLocationRepository(IDataAccess dataAccess) : IQRCodeLoactionRepository
    {
        public async Task InsertAsync(QRCodeLocation qRCodeLocation)
        {
            using var session = dataAccess.CreateSession();
            session.Begin();

            try
            {
                var query = @"
                    INSERT INTO QRCODE_LOCATION (
                        txQRCodePadraoEMV, 
                        tpJornada, 
                        idFimAFim,
                        statusQRCode)
                    VALUES (
                        @txQRCodePadraoEMV, 
                        @tpJornada, 
                        @idFimAFim,
                        @statusQRCode);
                ";

                session.Execute(query, qRCodeLocation);
                session.Commit();

                await Task.CompletedTask;
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
        }

        public async Task<QRCodeLocation?> GetByEMVAtivoAsync(string txQRCodePadraoEMV)
        {
            using var session = dataAccess.CreateSession();

            try
            {
                var query = @"
                    SELECT 
                        txQRCodePadraoEMV as TxQRCodePadraoEMV, 
                        tpJornada as TpJornada, 
                        idFimAFim as IdFimAFim, 
                        statusQRCode as StatusQRCode
                    FROM QRCODE_LOCATION
                    WHERE txQRCodePadraoEMV = @txQRCodePadraoEMV
                    AND statusQRCode = 'Ativo';
                ";

                var data = await session.QueryAsync<QRCodeLocation>(query, new { txQRCodePadraoEMV });
                return data.SingleOrDefault();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task UpdateQrCodesStatus(string[] txQRCodePadraoEMVList, string status)
        {
            using var session = dataAccess.CreateSession();
            session.Begin();

            try
            {
                var query = @"
                    update QRCODE_LOCATION
                        set statusQRCode = @status
                    where txQRCodePadraoEMV in @txQRCodePadraoEMVList
                ";

                session.Execute(query, new { txQRCodePadraoEMVList, status });
                session.Commit();

                await Task.CompletedTask;
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
        }
    }
}