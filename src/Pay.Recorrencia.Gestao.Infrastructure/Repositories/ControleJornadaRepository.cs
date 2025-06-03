using DataAccess.Domain;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Infrastructure.Repositories
{
    public class ControleJornadaRepository : IControleJornadaRepository
    {
        private readonly IDataAccess _dataAccess;

        public ControleJornadaRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IList<ControleJornada>> GetControle(string tpJornada, string idRecorrencia, string? idFimAFim)
        {
            using var session = _dataAccess.CreateSession();
            try
            {
                string query = @"SELECT 
                                     TpJornada as TpJornada
                                    ,IdRecorrencia as IdRecorrencia
                                    ,IdE2E as IdE2E
                                    ,IdConciliacaoRecebedor as IdConciliacaoRecebedor
                                    ,SituacaoJornada as SituacaoJornada
                                    ,DtAgendamento as DtAgendamento
                                    ,VlAgendamento as VlAgendamento
                                    ,DtPagamento as DtPagamento
                                    ,DataHoraCriacao as DataHoraCriacao
                                    ,DataUltimaAtualizacao as DataUltimaAtualizacao
                                FROM JORNADAS
                                WHERE TpJornada = @TpJornada ";

                if (!string.IsNullOrEmpty(idRecorrencia))
                    query += " AND IdRecorrencia = @IdRecorrencia ";

                if (!string.IsNullOrEmpty(idFimAFim))
                    query += " AND IdE2E = @IdE2E ";

                var result = session.QueryAsync<ControleJornada>(query, new { TpJornada = tpJornada, IdRecorrencia = idRecorrencia, IdE2E = idFimAFim });

                return result.Result.ToList();
            }
            catch
            {
                session.Rollback();
                throw;
            }
        }

        public async Task IncluirControle(ControleJornada controleJornada)
        {
            using var session = _dataAccess.CreateSession();
            session.Begin();
            try
            {
                string query = ObtemQueryIncluir(controleJornada);
                object param = ObterParametros(controleJornada);

                session.Execute(query, param);
                session.Commit();

                return;

            }
            catch (Exception)
            {
                session.Rollback();
                throw;
            }
        }

        public async Task AtualizarControle(ControleJornada controleJornada)
        {
            using var session = _dataAccess.CreateSession();
            session.Begin();
            try
            {
                string query = ObtemQueryAtualizar(controleJornada);
                object param = ObterParametros(controleJornada);

                session.Execute(query, param);
                session.Commit();

                return;

            }
            catch (Exception)
            {
                session.Rollback();
                throw;
            }
        }

        private string ObtemQueryAtualizar(ControleJornada controleJornada)
        {
            string updateQuery = @" UPDATE JORNADAS
                                    SET  TpJornada = @TpJornada ";

            if (!string.IsNullOrEmpty(controleJornada.IdRecorrencia))
                updateQuery += " ,IdRecorrencia = @IdRecorrencia ";

            if (!string.IsNullOrEmpty(controleJornada.IdE2E))
                updateQuery += " ,IdE2E = @IdE2E ";

            if (!string.IsNullOrEmpty(controleJornada.IdConciliacaoRecebedor))
                updateQuery += " ,IdConciliacaoRecebedor = @IdConciliacaoRecebedor ";

            if (!string.IsNullOrEmpty(controleJornada.SituacaoJornada))
                updateQuery += " ,SituacaoJornada = @SituacaoJornada ";

            if (controleJornada.DtAgendamento.HasValue)
                updateQuery += " ,DtAgendamento = @DtAgendamento ";

            if (!controleJornada.VlAgendamento.Equals(null))
                updateQuery += " ,VlAgendamento = @VlAgendamento ";

            if (controleJornada.DtPagamento.HasValue)
                updateQuery += " ,DtPagamento = @DtPagamento ";

            if (controleJornada.DataHoraCriacao.HasValue)
                updateQuery += " ,DataHoraCriacao = @DataHoraCriacao ";

            if (controleJornada.DataUltimaAtualizacao.HasValue)
                updateQuery += " ,DataUltimaAtualizacao = @DataUltimaAtualizacao ";

            updateQuery += " WHERE TpJornada = @TpJornada ";

            if (!string.IsNullOrEmpty(controleJornada.IdRecorrencia))
                updateQuery += " AND IdRecorrencia = @IdRecorrencia ";

            if (!string.IsNullOrEmpty(controleJornada.IdE2E))
                updateQuery += " AND IdE2E = @IdE2E ";

            return updateQuery;
        }

        private string ObtemQueryIncluir(ControleJornada controleJornada)
        {
            return @"INSERT INTO JORNADAS
                           (TpJornada
                           ,IdRecorrencia
                           ,IdE2E
                           ,IdConciliacaoRecebedor
                           ,SituacaoJornada
                           ,DtAgendamento
                           ,VlAgendamento
                           ,DtPagamento
                           ,DataHoraCriacao
                           ,DataUltimaAtualizacao)
                     VALUES
                           (@TpJornada
                           ,@IdRecorrencia
                           ,@IdE2E
                           ,@IdConciliacaoRecebedor
                           ,@SituacaoJornada
                           ,@DtAgendamento
                           ,@VlAgendamento
                           ,@DtPagamento
                           ,@DataHoraCriacao
                           ,@DataUltimaAtualizacao) ";
        }

        private object ObterParametros(ControleJornada jornada)
        {
            return new
            {
                jornada.TpJornada,
                jornada.IdRecorrencia,
                jornada.IdE2E,
                jornada.IdConciliacaoRecebedor,
                jornada.SituacaoJornada,
                jornada.DtAgendamento,
                jornada.VlAgendamento,
                jornada.DtPagamento,
                jornada.DataHoraCriacao,
                jornada.DataUltimaAtualizacao

            };
        }
        public async Task<IList<ControleJornada>> BuscarPorIdRecorrenciaAsync(string idRecorrencia)
        {
            using var session = _dataAccess.CreateSession();
            try
            {
                string query = @"SELECT 
                             TpJornada as TpJornada,
                             IdRecorrencia as IdRecorrencia,
                             IdE2E as IdE2E,
                             IdConciliacaoRecebedor as IdConciliacaoRecebedor,
                             SituacaoJornada as SituacaoJornada,
                             DtAgendamento as DtAgendamento,
                             VlAgendamento as VlAgendamento,
                             DtPagamento as DtPagamento,
                             DataHoraCriacao as DataHoraCriacao,
                             DataUltimaAtualizacao as DataUltimaAtualizacao
                         FROM JORNADAS
                         WHERE IdRecorrencia = @IdRecorrencia";

                var result = await session.QueryAsync<ControleJornada>(query, new { IdRecorrencia = idRecorrencia });
                return result.ToList();
            }
            catch
            {
                session.Rollback();
                throw;
            }
        }

    }
}
