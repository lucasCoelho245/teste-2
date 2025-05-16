using DataAccess.Domain;
using Pay.Recorrencia.Gestao.Infrastructure.Data.Interfaces;

namespace Pay.Recorrencia.Gestao.Infrastructure.Data
{
    public class PixAutomaticoDataAccess(DBConfig dbConfig) : BaseDataAccess(dbConfig), IPixAutomaticoDataAccess
    {
    }
}