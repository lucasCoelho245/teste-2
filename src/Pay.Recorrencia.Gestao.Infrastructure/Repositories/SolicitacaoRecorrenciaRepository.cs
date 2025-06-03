using DataAccess.Domain;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using Pay.Recorrencia.Gestao.Infrastructure.Data.Interfaces;

namespace Pay.Recorrencia.Gestao.Infrastructure.Repositories
{
    public class SolicitacaoRecorrenciaRepository : ISolicitacaoRecorrenciaRepository
    {
        private readonly IDataAccess _dataAccess;
        private readonly IPixAutomaticoDataAccess _dbContext;

        public SolicitacaoRecorrenciaRepository(IDataAccess dataAccess, IPixAutomaticoDataAccess dbContext)
        {
            _dataAccess = dataAccess;
            _dbContext = dbContext;
        }

        public async Task<SolicitacaoRecorrencia?> GetSolicitacaoRecorrencia(string id)
        {
            using var session = _dataAccess.CreateSession();
            try
            {
                string query = @"SELECT idSolicRecorrencia AS IdSolicRecorrencia,
                                        idRecorrencia AS IdRecorrencia,
                                        tipoRecorrencia AS TipoRecorrencia,
                                        tipoFrequencia AS TipoFrequencia,
                                        dataInicialRecorrencia AS DataInicialRecorrencia,
                                        dataFinalRecorrencia AS DataFinalRecorrencia,
                                        situacaoSolicRecorrencia AS SituacaoSolicRecorrencia,
                                        codigoMoedaSolicRecorr AS CodigoMoedaSolicRecorr,
                                        valorFixoSolicRecorrencia AS ValorFixoSolicRecorrencia,
                                        indicadorValorMin AS IndicadorValorMin,
                                        valorMinRecebedorSolicRecorr AS ValorMinRecebedorSolicRecorr,
                                        nomeUsuarioRecebedor AS NomeUsuarioRecebedor,
                                        cpfCnpjUsuarioRecebedor AS CpfCnpjUsuarioRecebedor,
                                        participanteDoUsuarioRecebedor AS ParticipanteDoUsuarioRecebedor,
                                        cpfCnpjUsuarioPagador AS CpfCnpjUsuarioPagador,
                                        contaUsuarioPagador AS ContaUsuarioPagador,
                                        agenciaUsuarioPagador AS AgenciaUsuarioPagador,
                                        nomeDevedor AS NomeDevedor,
                                        cpfCnpjDevedor AS CpfCnpjDevedor,
                                        numeroContrato AS NumeroContrato,
                                        descObjetoContrato AS DescObjetoContrato,
                                        dataHoraCriacaoRecorr AS DataHoraCriacaoRecorr,
                                        dataHoraCriacaoSolicRecorr AS DataHoraCriacaoSolicRecorr,
                                        dataHoraExpiracaoSolicRecorr AS DataHoraExpiracaoSolicRecorr,
                                        dataUltimaAtualizacao AS DataUltimaAtualizacao
                                   FROM [dbo].[SOLICITACAO_AUTORIZACAO_RECORRENCIA] 
                                  WHERE idSolicRecorrencia = @Id";

                var result = await session.QueryAsync<SolicitacaoRecorrencia>(query, new { Id = id });

                return result?.SingleOrDefault();
            }
            catch
            {
                session.Rollback();
                throw;
            }
        }

        public async Task Insert(SolicitacaoRecorrencia data)
        {
            using var session = _dataAccess.CreateSession();
            session.Begin();
            try
            {
                //Logger.Information("Starting insert transaction");
                string query = @" INSERT INTO [dbo].[SOLICITACAO_AUTORIZACAO_RECORRENCIA] 
                                ( idSolicRecorrencia
                                 ,idAutorizacao
                                 ,idRecorrencia
                                 ,tipoRecorrencia
                                 ,tipoFrequencia
                                 ,dataInicialRecorrencia
                                 ,dataFinalRecorrencia
                                 ,situacaoSolicRecorrencia
                                 ,codigoMoedaSolicRecorr
                                 ,valorFixoSolicRecorrencia
                                 ,indicadorValorMin
                                 ,valorMinRecebedorSolicRecorr
                                 ,nomeUsuarioRecebedor
                                 ,cpfCnpjUsuarioRecebedor
                                 ,participanteDoUsuarioRecebedor
                                 ,cpfCnpjUsuarioPagador
                                 ,contaUsuarioPagador
                                 ,agenciaUsuarioPagador
                                 ,nomeDevedor
                                 ,cpfCnpjDevedor
                                 ,numeroContrato
                                 ,descObjetoContrato
                                 ,dataHoraCriacaoRecorr
                                 ,dataHoraCriacaoSolicRecorr
                                 ,dataHoraExpiracaoSolicRecorr
                                 ,dataUltimaAtualizacao
                                 )
                            VALUES
                                 (@idSolicRecorrencia
                                 ,@idAutorizacao
                                 ,@idRecorrencia
                                 ,@tipoRecorrencia
                                 ,@tipoFrequencia
                                 ,@dataInicialRecorrencia
                                 ,@dataFinalRecorrencia
                                 ,@situacaoSolicRecorrencia
                                 ,@codigoMoedaSolicRecorr
                                 ,@valorFixoSolicRecorrencia
                                 ,@indicadorValorMin
                                 ,@valorMinRecebedorSolicRecorr
                                 ,@nomeUsuarioRecebedor
                                 ,@cpfCnpjUsuarioRecebedor
                                 ,@participanteDoUsuarioRecebedor
                                 ,@cpfCnpjUsuarioPagador
                                 ,@contaUsuarioPagador
                                 ,@agenciaUsuarioPagador
                                 ,@nomeDevedor
                                 ,@cpfCnpjDevedor
                                 ,@numeroContrato
                                 ,@descObjetoContrato
                                 ,@dataHoraCriacaoRecorr
                                 ,@dataHoraCriacaoSolicRecorr
                                 ,@dataHoraExpiracaoSolicRecorr
                                 ,@dataUltimaAtualizacao)";
                session.Execute(query, data);
                session.Commit();

                await Task.CompletedTask;
            }
            catch (Exception)
            {
                session.Rollback();
                //Logger.Information("Finish insert transaction - Error: @e.Message", e.Message);
                throw;
            }
        }
        public async Task Update(SolicitacaoAutorizacaoRecorrenciaUpdateDTO data)
        {
            using var session = _dataAccess.CreateSession();
            session.Begin();
            try
            {
                //Logger.Information("Starting update transaction");
                string query = @"UPDATE [dbo].[SOLICITACAO_AUTORIZACAO_RECORRENCIA]
                                    SET [situacaoSolicRecorrencia] = @situacaoSolicRecorrencia
                                        ,[dataUltimaAtualizacao] = @dataUltimaAtualizacao";

                if (!String.IsNullOrEmpty(data.IdAutorizacao))
                {
                    query += @",[idAutorizacao] = @idAutorizacao ";
                }

                query += @"WHERE [idSolicRecorrencia] = @idSolicRecorrencia";
                
                session.Execute(query, data);
                session.Commit();

                await Task.CompletedTask;
            }
            catch (Exception)
            {
                session.Rollback();
                //Logger.Information("Finish update transaction - Error: @e.Message", e.Message);
                throw;
            }
        }
        
        public async Task<string> CancelarSolicitacaoRecorrencia(SolicitacaoAutorizacaoRecorrenciaUpdateDTO data)
        {
            using var session = _dataAccess.CreateSession();
            session.Begin();
            try
            {
                //Logger.Information("Starting update transaction");
                string query = @"UPDATE [dbo].[SOLICITACAO_AUTORIZACAO_RECORRENCIA]
                                    SET [situacaoSolicRecorrencia] = @situacaoSolicRecorrencia
                                        ,[dataUltimaAtualizacao] = @dataUltimaAtualizacao";

                if (!String.IsNullOrEmpty(data.IdAutorizacao))
                {
                    query += @",[idAutorizacao] = @idAutorizacao ";
                }

                query += @"WHERE [idSolicRecorrencia] = @idSolicRecorrencia";
                
                session.Execute(query, data);
                session.Commit();

                return await Task.FromResult("OK");
            }
            catch (Exception)
            {
                session.Rollback();
                //Logger.Information("Finish update transaction - Error: @e.Message", e.Message);
                throw;
            }
        }
        public async Task<ListaSolicAutorizacaoRecPaginada> GetAllAsync(GetListaSolicAutorizacaoRecDTOPaginada data)
        {
            string sqlCount = $"SELECT COUNT(*) " +
                            "FROM [dbo].[SOLICITACAO_AUTORIZACAO_RECORRENCIA] " +
                            "WHERE cpfCnpjUsuarioPagador = @CpfCnpjUsuarioPagador ";

            string sqlQuery = $"SELECT idSolicRecorrencia IdSolicRecorrencia," +
                            "nomeUsuarioRecebedor NomeUsuarioRecebedor, " +
                            "situacaoSolicRecorrencia SituacaoSolicRecorrencia, " +
                            "cpfCnpjUsuarioPagador CpfCnpjUsuarioPagador, " +
                            "nomeDevedor NomeDevedor, " +
                            "dataHoraExpiracaoSolicRecorr DataHoraExpiracaoSolicRecorr, " +
                            "cpfCnpjDevedor CpfCnpjDevedor " +
                            "FROM [dbo].[SOLICITACAO_AUTORIZACAO_RECORRENCIA] " +
                            "WHERE cpfCnpjUsuarioPagador = @CpfCnpjUsuarioPagador " +
                            $"AND contaUsuarioPagador = {int.Parse(data.ContaUsuarioPagador)}";

            if (!String.IsNullOrEmpty(data.SituacaoSolicRecorrencia))
            {
                sqlCount += "AND situacaoSolicRecorrencia = @SituacaoSolicRecorrencia";
                sqlQuery += "AND situacaoSolicRecorrencia = @SituacaoSolicRecorrencia"; ;
            }
            if (!String.IsNullOrEmpty(data.NomeUsuarioRecebedor))
            {
                sqlCount += $" AND nomeUsuarioRecebedor COLLATE Latin1_General_CI_AI LIKE '%{data.NomeUsuarioRecebedor}%'";
                sqlQuery += $" AND nomeUsuarioRecebedor COLLATE Latin1_General_CI_AI LIKE '%{data.NomeUsuarioRecebedor}%'"; ;
            }
            if (!String.IsNullOrEmpty(data.AgenciaUsuarioPagador?.ToString()))
            {
                sqlCount += $" AND agenciaUsuarioPagador = {int.Parse(data.AgenciaUsuarioPagador)}";
                sqlQuery += $" AND agenciaUsuarioPagador = {int.Parse(data.AgenciaUsuarioPagador)}"; ;
            }
            if(data.DtExpiracaoInicio != DateTime.MinValue && data.DtExpiracaoFim != DateTime.MinValue)
            {
                sqlCount += $" AND dataHoraExpiracaoSolicRecorr BETWEEN @DtExpiracaoInicio AND @DtExpiracaoFim";
                sqlQuery += $" AND dataHoraExpiracaoSolicRecorr BETWEEN @DtExpiracaoInicio AND @DtExpiracaoFim";
            }

            int offset = (data.Page - 1) * data.PageSize;

            sqlQuery += $" ORDER BY dataHoraCriacaoSolicRecorr OFFSET {offset} ROWS FETCH NEXT @PageSize ROWS ONLY";

            int totalItems = await _dbContext.ExecuteScalarAsync(sqlCount, data);

            var items = await _dbContext.QueryAsync<SolicAutorizacaoRecList>(sqlQuery, data);

            return new ListaSolicAutorizacaoRecPaginada()
            {
                Items = items,
                TotalItems = totalItems
            };
        }
        public async Task<SolicAutorizacaoRecNonPagination> GetAsync(GetSolicAutorizacaoRecDTOPaginada data)
        {
            string sqlQuery = $"SELECT * FROM [dbo].[SOLICITACAO_AUTORIZACAO_RECORRENCIA] ";
            
            if(data.IdSolicRecorrencia != null)
            {
                sqlQuery += "WHERE idSolicRecorrencia = @IdSolicRecorrencia";
            }
            else if(data.IdRecorrencia != null)
            {
                sqlQuery += "WHERE idRecorrencia = @IdRecorrencia";
            }

            var items = await _dbContext.QueryAsync<SolicitacaoAutorizacaoRecorrenciaDetalhes>(sqlQuery, data);

            return new SolicAutorizacaoRecNonPagination()
            {
                Data = items.FirstOrDefault()
            };
        }

        public async Task<SolicitacaoRecorrencia> GetById(string id)
        {
            using var session = _dataAccess.CreateSession();
            try
            {
                //id = "e962da16-87d1-489d-9159-d827f02240b3";
                string query = "SELECT * FROM [dbo].[SOLICITACAO_AUTORIZACAO_RECORRENCIA] WHERE idSolicRecorrencia = @Id";
                var count = await _dbContext.QueryAsync<SolicitacaoRecorrencia>(query, new { Id = id });
                return count.FirstOrDefault();
            }
            catch
            {
                session.Rollback();
                throw;
            }
        }

        public string ConsultarCodMunIBGE()
        {
            return "3550308";
        }

        public Task<SolicitacaoRecorrencia> InsertSolicitacaoRecorrencia(SolicitacaoRecorrencia solicitacaoRecorrencia)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsByIdAsync(string id)
        {
            using var session = _dataAccess.CreateSession();
            try
            {
                string query = "SELECT COUNT(1) FROM [dbo].[SOLICITACAO_AUTORIZACAO_RECORRENCIA] WHERE idSolicRecorrencia = @Id";
                var count = session.Query<int>(query, new { Id = id }).SingleOrDefault();
                return count > 0;
            }
            catch
            {
                session.Rollback();
                throw;
            }
        }

        public async Task InsertAsync(SolicitacaoRecorrencia solicitacaoRecorrencia)
        {
            using var session = _dataAccess.CreateSession();
            session.Begin();
            try
            {
                //Logger.Information("Starting insert transaction");
                string query = ObtemQueryInsert();
                object param = ObterParametros(solicitacaoRecorrencia);

                session.Execute(query, param);
                session.Commit();

                return;

            }
            catch (Exception)
            {
                session.Rollback();
                //Logger.Information("Finish insert transaction - Error: @e.Message", e.Message);
                throw;
            }
        }

        private string ObtemQueryInsert()
        {
            return @" INSERT INTO [dbo].[SOLICITACAO_AUTORIZACAO_RECORRENCIA] ( idSolicRecorrencia
                                                                               ,idAutorizacao
                                                                               ,idRecorrencia
                                                                               ,tipoRecorrencia
                                                                               ,tipoFrequencia
                                                                               ,dataInicialRecorrencia
                                                                               ,dataFinalRecorrencia
                                                                               ,situacaoSolicRecorrencia
                                                                               ,codigoMoedaSolicRecorr
                                                                               ,valorFixoSolicRecorrencia
                                                                               ,indicadorValorMin
                                                                               ,valorMinRecebedorSolicRecorr
                                                                               ,nomeUsuarioRecebedor
                                                                               ,cpfCnpjUsuarioRecebedor
                                                                               ,participanteDoUsuarioRecebedor
                                                                               ,cpfCnpjUsuarioPagador
                                                                               ,contaUsuarioPagador
                                                                               ,agenciaUsuarioPagador
                                                                               ,nomeDevedor
                                                                               ,cpfCnpjDevedor
                                                                               ,numeroContrato
                                                                               ,descObjetoContrato
                                                                               ,dataHoraCriacaoRecorr
                                                                               ,dataHoraCriacaoSolicRecorr
                                                                               ,dataHoraExpiracaoSolicRecorr
                                                                               ,dataUltimaAtualizacao
                                                                                )
                                                                         VALUES
                                                                               (@idSolicRecorrencia
                                                                               ,@idAutorizacao
                                                                               ,@idRecorrencia
                                                                               ,@tipoRecorrencia
                                                                               ,@tipoFrequencia
                                                                               ,@dataInicialRecorrencia
                                                                               ,@dataFinalRecorrencia
                                                                               ,@situacaoSolicRecorrencia
                                                                               ,@codigoMoedaSolicRecorr
                                                                               ,@valorFixoSolicRecorrencia
                                                                               ,@indicadorValorMin
                                                                               ,@valorMinRecebedorSolicRecorr
                                                                               ,@nomeUsuarioRecebedor
                                                                               ,@cpfCnpjUsuarioRecebedor
                                                                               ,@participanteDoUsuarioRecebedor
                                                                               ,@cpfCnpjUsuarioPagador
                                                                               ,@contaUsuarioPagador
                                                                               ,@agenciaUsuarioPagador
                                                                               ,@nomeDevedor
                                                                               ,@cpfCnpjDevedor
                                                                               ,@numeroContrato
                                                                               ,@descObjetoContrato
                                                                               ,@dataHoraCriacaoRecorr
                                                                               ,@dataHoraCriacaoSolicRecorr
                                                                               ,@dataHoraExpiracaoSolicRecorr
                                                                               ,@dataUltimaAtualizacao)";
        }

        private object ObterParametros(SolicitacaoRecorrencia solicitacaoRecorrencia)
        {
            return new
            {
                solicitacaoRecorrencia.IdSolicRecorrencia,
                solicitacaoRecorrencia.IdAutorizacao,
                solicitacaoRecorrencia.IdRecorrencia,
                solicitacaoRecorrencia.TipoRecorrencia,
                solicitacaoRecorrencia.TipoFrequencia,
                solicitacaoRecorrencia.DataInicialRecorrencia,
                solicitacaoRecorrencia.DataFinalRecorrencia,
                solicitacaoRecorrencia.SituacaoSolicRecorrencia,
                solicitacaoRecorrencia.CodigoMoedaSolicRecorr,

                solicitacaoRecorrencia.ValorFixoSolicRecorrencia,
                solicitacaoRecorrencia.IndicadorValorMin,

                solicitacaoRecorrencia.ValorMinRecebedorSolicRecorr,
                solicitacaoRecorrencia.NomeUsuarioRecebedor,
                solicitacaoRecorrencia.CpfCnpjUsuarioRecebedor,
                solicitacaoRecorrencia.ParticipanteDoUsuarioRecebedor,
                solicitacaoRecorrencia.CpfCnpjUsuarioPagador,
                solicitacaoRecorrencia.ContaUsuarioPagador,
                solicitacaoRecorrencia.AgenciaUsuarioPagador,
                solicitacaoRecorrencia.NomeDevedor,
                solicitacaoRecorrencia.CpfCnpjDevedor,
                solicitacaoRecorrencia.NumeroContrato,
                solicitacaoRecorrencia.DescObjetoContrato,
                solicitacaoRecorrencia.DataHoraCriacaoRecorr,
                solicitacaoRecorrencia.DataHoraCriacaoSolicRecorr,
                solicitacaoRecorrencia.DataHoraExpiracaoSolicRecorr,
                solicitacaoRecorrencia.DataUltimaAtualizacao
            };
        }
    }
}
