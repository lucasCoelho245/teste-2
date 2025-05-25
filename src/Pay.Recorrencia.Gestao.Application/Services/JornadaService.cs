using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Services
{
    public class JornadaService : IJornadaService
    {
        private readonly IJornadaRepository _repo;
        public JornadaService(IJornadaRepository repo) => _repo = repo;

        //public async Task<ListaJornadaPaginada> GetAllAsync(GetListaJornadaDTOPaginada request)
        //{
        //    var lista = new List<dynamic>
        //    {
        //        new
        //        {
        //            Id = 1,
        //            TpJornada = "TipoA",
        //            IdRecorrencia = "Rec001",
        //            IdE2E = "E2E001",
        //            IdConciliacaoRecebedor = "Conc001",
        //            SituacaoJornada = "Pendente",
        //            DtAgendamento = DateTime.Parse("2025-05-21"),
        //            VlAgendamento = 150.75m,
        //            DtPagamento = (DateTime?)null,
        //            DataHoraCriacao = DateTime.Now,
        //            DataUltimaAtualizacao = DateTime.Now
        //        },
        //        new
        //        {
        //            Id = 2,
        //            TpJornada = "TipoB",
        //            IdRecorrencia = "Rec002",
        //            IdE2E = "E2E002",
        //            IdConciliacaoRecebedor = "Conc002",
        //            SituacaoJornada = "Concluída",
        //            DtAgendamento = DateTime.Parse("2025-05-22"),
        //            VlAgendamento = 200.00m,
        //            DtPagamento = DateTime.Parse("2025-05-23"),
        //            DataHoraCriacao = DateTime.Now,
        //            DataUltimaAtualizacao = DateTime.Now
        //        },
        //        new
        //        {
        //            Id = 3,
        //            TpJornada = "TipoC",
        //            IdRecorrencia = "Rec003",
        //            IdE2E = "E2E003",
        //            IdConciliacaoRecebedor = "Conc003",
        //            SituacaoJornada = "Erro",
        //            DtAgendamento = (DateTime?)null,
        //            VlAgendamento = (decimal?)null,
        //            DtPagamento = (DateTime?)null,
        //            DataHoraCriacao = DateTime.Now,
        //            DataUltimaAtualizacao = DateTime.Now
        //        }
        //    };

        //    IList<JornadaList>? dataDTO = [];

        //    foreach (var item in lista)
        //    {
        //        dataDTO.Add(new JornadaList
        //        {
        //            TpJornada = item.TpJornada,
        //            IdRecorrencia = item.IdRecorrencia,
        //            IdE2E = item.IdE2E,
        //            IdConciliacaoRecebedor = item.IdConciliacaoRecebedor,
        //        });
        //    }

        //    return await Task.FromResult(
        //        new ListaJornadaPaginada()
        //        {
        //            Items = dataDTO,
        //            TotalItems = dataDTO.Count()
        //        });
        //}

        //public async Task<JornadaDto> GetByJornadaERecorrenciaAsync(string tpJornada, string idRecorrencia)
        //{
        //    var j = await _repo.GetByTpJornadaAndIdRecorrenciaAsync(tpJornada, idRecorrencia);
        //    return j == null ? null : ToDto(j);
        //}

        //public async Task<JornadaDto> GetByJornadaE2EAsync(string tpJornada, string idE2E)
        //{
        //    var j = await _repo.GetByTpJornadaAndIdE2EAsync(tpJornada, idE2E);
        //    return j == null ? null : ToDto(j);
        //}

        //public async Task<IEnumerable<JornadaDto>> GetByAnyFilterAsync(
        //    string tpJornada,
        //    string idRecorrencia,
        //    string idE2E,
        //    string idConciliacaoRecebedor)
        //{
        //    var list = await _repo.GetByAnyFilterAsync(
        //        tpJornada, idRecorrencia, idE2E, idConciliacaoRecebedor);
        //    return list.Select(ToDto);
        //}

        public Task<JornadaNonPagination> GetByTpJornadaAndIdRecorrenciaAsync(JornadaDTO request)
        {
            const string sql = @"
                SELECT * FROM dbo.Jornadas
                WHERE TpJornada = @tpJornada
                  AND IdRecorrencia = @idRecorrencia";
            return Task.FromResult<JornadaNonPagination>(null);
        }

        public Task<JornadaNonPagination> GetByTpJornadaAndIdE2EAsync(JornadaDTO request)
        {
            //const string sql = @"
            //        SELECT * FROM dbo.Jornadas
            //        WHERE TpJornada = @tpJornada
            //          AND IdE2E = @idE2E";
            //return _db.QueryFirstOrDefaultAsync<JornadaNonPagination>(sql, request);

            var lista = new List<Jornada>
        {
            new Jornada
            {
                TpJornada = "TipoA",
                IdRecorrencia = "Rec001",
                IdE2E = "E2E001",
                IdConciliacaoRecebedor = "Conc001",
                SituacaoJornada = "Pendente",
                DtAgendamento = DateTime.Parse("2025-05-21"),
                VlAgendamento = 150.75m,
                DtPagamento = (DateTime?)null,
                DataHoraCriacao = DateTime.Now,
                DataUltimaAtualizacao = DateTime.Now
            },
            new Jornada
            {
                TpJornada = "TipoB",
                IdRecorrencia = "Rec002",
                IdE2E = "E2E002",
                IdConciliacaoRecebedor = "Conc002",
                SituacaoJornada = "Concluída",
                DtAgendamento = DateTime.Parse("2025-05-22"),
                VlAgendamento = 200.00m,
                DtPagamento = DateTime.Parse("2025-05-23"),
                DataHoraCriacao = DateTime.Now,
                DataUltimaAtualizacao = DateTime.Now
            },
            new Jornada
            {
                TpJornada = "TipoC",
                IdRecorrencia = "Rec003",
                IdE2E = "E2E003",
                IdConciliacaoRecebedor = "Conc003",
                SituacaoJornada = "Erro",
                DtAgendamento = (DateTime?)null,
                VlAgendamento = (decimal?)null,
                DtPagamento = (DateTime?)null,
                DataHoraCriacao = DateTime.Now,
                DataUltimaAtualizacao = DateTime.Now
            }
        };


            IEnumerable<dynamic>? dataFilter = lista.Where(item => item.TpJornada == request.TpJornada);

            return Task.FromResult(
                new JornadaNonPagination()
                {
                    Data = dataFilter.FirstOrDefault(),
                });

        }

        public Task<JornadaNonPagination> GetByAnyFilterAsync(JornadaDTO request)
        {
            var sql = @"SELECT * FROM dbo.Jornadas WHERE 1=1";
            if (!string.IsNullOrEmpty(request.TpJornada))
                sql += " AND TpJornada = @TpJornada";
            if (!string.IsNullOrEmpty(request.IdRecorrencia))
                sql += " AND IdRecorrencia = @IdRecorrencia";
            if (!string.IsNullOrEmpty(request.IdE2E))
                sql += " AND IdE2E = @IdE2E";
            if (!string.IsNullOrEmpty(request.IdConciliacaoRecebedor))
                sql += " AND IdConciliacaoRecebedor = @IdConciliacaoRecebedor";

            return Task.FromResult<JornadaNonPagination>(null);
        }
    }
}
