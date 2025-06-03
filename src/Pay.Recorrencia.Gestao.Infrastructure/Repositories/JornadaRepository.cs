using DataAccess.Domain;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Infrastructure.Repositories;

public class JornadaRepository : IJornadaRepository
{
    private readonly IDataAccess _dataAccess;
    public JornadaRepository(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<ListaJornadaPaginada<JornadaList>> GetAllAsync(JornadaDTO request)
    {
        string sql = @"SELECT * FROM dbo.Jornadas";


        using var session = _dataAccess.CreateSession();
        try
        {
            session.Begin();

            var items = await session.QueryAsync<JornadaList>(sql, request);

            var pagedItems = items 
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);

            return new ListaJornadaPaginada<JornadaList> { Items = pagedItems, TotalItems = items.Count() };

        }
        catch
        {
            session.Rollback();
            throw;
        }


    }

    public async Task<JornadaNonPagination> GetByTpJornadaAndIdRecorrenciaAsync(JornadaAutorizacaoDTO request)
    {
        string sql = @"
                SELECT * FROM dbo.Jornadas
                WHERE TpJornada = @tpJornada";

        if (!string.IsNullOrEmpty(request.IdRecorrencia))
            sql += " AND  IdRecorrencia = @idRecorrencia";

        using var session = _dataAccess.CreateSession();
        try
        {
            session.Begin();

            var items = await session.QueryAsync<Jornada>(sql, request);

            return new JornadaNonPagination { Data = items.FirstOrDefault() };

        }
        catch
        {
            session.Rollback();
            throw;
        }
    }

    public async Task<JornadaNonPagination?> GetByTpJornadaAndIdE2EAsync(JornadaAgendamentoDTO request)
    {
        string sql = @"
                SELECT * FROM dbo.Jornadas
                WHERE TpJornada = @tpJornada";

        if (!string.IsNullOrEmpty(request.IdE2E))
            sql += " AND IdE2E = @IdE2E";

        using var session = _dataAccess.CreateSession();
        try
        {
            session.Begin();

            var items = await session.QueryAsync<Jornada>(sql, request);

            return new JornadaNonPagination { Data = items.FirstOrDefault() };

        }
        catch
        {
            session.Rollback();
            throw;
        }

    }

    public async Task<ListaJornadaPaginada<Jornada>> GetByAnyFilterAsync(JornadaAutorizacaoAgendamentoDTO request)
    {
        var sql = @"SELECT * FROM dbo.Jornadas WHERE TpJornada = @tpJornada";

        if (!string.IsNullOrEmpty(request.IdRecorrencia))
            sql += " AND IdRecorrencia = @IdRecorrencia";
        if (!string.IsNullOrEmpty(request.IdE2E))
            sql += " AND IdE2E = @IdE2E";
        if (!string.IsNullOrEmpty(request.IdConciliacaoRecebedor))
            sql += " AND IdConciliacaoRecebedor = @IdConciliacaoRecebedor";

        using var session = _dataAccess.CreateSession();
        try
        {
            session.Begin();

            var items = await session.QueryAsync<Jornada>(sql, request);

            var pagedItems = items
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);

            return new ListaJornadaPaginada<Jornada> { Items = pagedItems, TotalItems = items.Count() };

        }
        catch
        {
            session.Rollback();
            throw;
        }
    }
}