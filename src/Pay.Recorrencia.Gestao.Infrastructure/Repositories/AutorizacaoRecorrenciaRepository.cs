using DataAccess.Domain;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using Pay.Recorrencia.Gestao.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay.Recorrencia.Gestao.Infrastructure.Repositories
{
    public class AutorizacaoRecorrenciaRepository : IAutorizacaoRecorrenciaRepository
    {
        //private ILogger Logger { get; }
        private IDataAccess Db { get; }
        private readonly IPixAutomaticoDataAccess _dbContext;

        //public AutorizacaoRecorrenciaRepository(ILogger logger, IDataAccess db)
        public AutorizacaoRecorrenciaRepository(IDataAccess db, IPixAutomaticoDataAccess dbContext)
        {
            //Logger = logger;
            Db = db;
            _dbContext = dbContext;
        }

        #region IncluirAutorizacaoRecorrencia

        public async Task<AutorizacaoRecorrencia> ConsultaAutorizacao(Guid id)
        {
            const string query = @" SELECT 
                                    idAutorizacao AS IdAutorizacao,
                                    idRecorrencia AS IdRecorrencia,
                                    situacaoRecorrencia AS SituacaoRecorrencia,
                                    tipoRecorrencia AS TipoRecorrencia,
                                    tipoFrequencia AS TipoFrequencia,
                                    dataInicialAutorizacaoRecorrencia AS DataInicialAutorizacaoRecorrencia,
                                    dataFinalAutorizacaoRecorrencia AS DataFinalAutorizacaoRecorrencia,
                                    codigoMoedaAutorizacaoRecorrencia AS CodigoMoedaAutorizacaoRecorrencia,
                                    valorRecorrencia AS ValorRecorrencia,
                                    valorMaximoAutorizado AS ValorMaximoAutorizado,
                                    motivoRejeicaoRecorrencia AS MotivoRejeicaoRecorrencia,
                                    nomeUsuarioRecebedor AS NomeUsuarioRecebedor,
                                    cpfCnpjUsuarioRecebedor AS CpfCnpjUsuarioRecebedor,
                                    participanteDoUsuarioRecebedor AS ParticipanteDoUsuarioRecebedor,
                                    codMunIBGE AS CodMunIBGE,
                                    cpfCnpjUsuarioPagador AS CpfCnpjUsuarioPagador,
                                    ContaUsuarioPagador AS ContaUsuarioPagador,
                                    agenciaUsuariopagador AS AgenciaUsuariopagador,
                                    participanteDousuarioPagador AS ParticipanteDousuarioPagador,
                                    nomeDevedor AS NomeDevedor,
                                    cpfCnpjDevedor AS CpfCnpjDevedor,
                                    numeroContrato AS NumeroContrato,
                                    descObjetoContrato AS DescObjetoContrato,
                                    codigoSituacaoCancelamentoRecorrencia AS CodigoSituacaoCancelamentoRecorrencia,
                                    dataUltimaAtualizacao AS DataUltimaAtualizacao,
                                    dataHoraCriacaoRecorr AS DataHoraCriacaoRecorr,
                                    flagPermitenotificacao AS FlagPermitenotificacao,
                                    flagValorMaximoAutorizado AS FlagValorMaximoAutorizado,
                                    tpRetentativa AS TpRetentativa
                                                                FROM AUTORIZACAO_RECORRENCIA
                                                                WHERE idAutorizacao = @Id";

            using (var session = Db.CreateSession())
            {
                var result = await session.QueryAsync<AutorizacaoRecorrencia>(query, new { Id = id });
                return result.FirstOrDefault();
            }
        }

        public async Task<AtualizacaoAutorizacaoRecorrencia> ConsultarAtualizacaoAutorizacaoRecorrencia(Guid id)
        {
            const string query = @"SELECT idAutorizacao AS IdAutorizacao,
                                          idRecorrencia AS IdRecorrencia,
                                          tipoSituacaoRecorrencia AS TipoSituacaoRecorrencia,
                                          dataHoraSituacaoRecorrencia AS DataHoraSituacaoRecorrencia,
                                          dataUltimaAtualizacao AS DataUltimaAtualizacao
                                                FROM ATUALIZACOES_AUTORIZACOES_RECORRENCIA 
                                                WHERE idAutorizacao = @Id";

            using (var session = this.Db.CreateSession())
            {
                var result = Db.Query<AtualizacaoAutorizacaoRecorrencia>(query, new { Id = id });
                return result.FirstOrDefault();
            }

        }

        public async Task<AutorizacaoRecorrencia> InsertAutorizacaoRecorrencia(AutorizacaoRecorrencia autorizacaoRecorrencia)
        {

            using (var session = this.Db.CreateSession())
            {
                session.Begin();
                try
                {
                    //Logger.Information("Starting insert transaction");
                    const string query = @" INSERT INTO AUTORIZACAO_RECORRENCIA (idAutorizacao,
                                                                         idRecorrencia,
                                                                         situacaoRecorrencia,
                                                                         tipoRecorrencia,
                                                                         tipoFrequencia,
                                                                         dataInicialAutorizacaoRecorrencia,
                                                                         dataFinalAutorizacaoRecorrencia,
                                                                         codigoMoedaAutorizacaoRecorrencia,
                                                                         valorRecorrencia,
                                                                         valorMaximoAutorizado,
                                                                         motivoRejeicaoRecorrencia,
                                                                         nomeUsuarioRecebedor,
                                                                         cpfCnpjUsuarioRecebedor,
                                                                         participanteDoUsuarioRecebedor,
                                                                         codMunIBGE,
                                                                         cpfCnpjUsuarioPagador,
                                                                         contausuariopagador,
                                                                         agenciausuariopagador,
                                                                         participantedousuariopagador,
                                                                         nomeDevedor,
                                                                         cpfCnpjDevedor,
                                                                         numeroContrato,
                                                                         descObjetoContrato,
                                                                         codigoSituacaoCancelamentoRecorrencia,
                                                                         dataUltimaAtualizacao,
                                                                         dataHoraCriacaoRecorr,
                                                                         flagpermitenotificacao,
                                                                         flagValorMaximoAutorizado,
                                                                         tpRetentativa) 
                                         VALUES (@idAutorizacao,
                                                @idRecorrencia,
                                                @situacaoRecorrencia,
                                                @tipoRecorrencia,
                                                @tipoFrequencia,
                                                @dataInicialAutorizacaoRecorrencia,
                                                @dataFinalAutorizacaoRecorrencia,
                                                @codigoMoedaAutorizacaoRecorrencia,
                                                @valorRecorrencia,
                                                @valorMaximoAutorizado,
                                                @motivoRejeicaoRecorrencia,
                                                @nomeUsuarioRecebedor,
                                                @cpfCnpjUsuarioRecebedor,
                                                @participanteDoUsuarioRecebedor,
                                                @codMunIBGE,
                                                @cpfCnpjUsuarioPagador,
                                                @contaUsuarioPagador,
                                                @agenciaUsuarioPagador,
                                                @participanteDoUsuarioPagador,
                                                @nomeDevedor,
                                                @cpfCnpjDevedor,
                                                @numeroContrato,
                                                @descObjetoContrato,
                                                @codigoSituacaoCancelamentoRecorrencia,
                                                @dataUltimaAtualizacao,
                                                @dataHoraCriacaoRecorr,
                                                @flagPermiteNotificacao,
                                                @flagValorMaximoAutorizado,
                                                @tpRetentativa)";

                    session.Execute(query, new
                    {
                        autorizacaoRecorrencia.IdAutorizacao,
                        autorizacaoRecorrencia.IdRecorrencia,
                        autorizacaoRecorrencia.SituacaoRecorrencia,
                        autorizacaoRecorrencia.TipoRecorrencia,
                        autorizacaoRecorrencia.TipoFrequencia,
                        autorizacaoRecorrencia.DataInicialAutorizacaoRecorrencia,
                        autorizacaoRecorrencia.DataFinalAutorizacaoRecorrencia,
                        autorizacaoRecorrencia.CodigoMoedaAutorizacaoRecorrencia,
                        autorizacaoRecorrencia.ValorRecorrencia,
                        autorizacaoRecorrencia.ValorMaximoAutorizado,
                        autorizacaoRecorrencia.MotivoRejeicaoRecorrencia,
                        autorizacaoRecorrencia.NomeUsuarioRecebedor,
                        autorizacaoRecorrencia.CpfCnpjUsuarioRecebedor,
                        autorizacaoRecorrencia.ParticipanteDoUsuarioRecebedor,
                        autorizacaoRecorrencia.CodMunIBGE,
                        autorizacaoRecorrencia.CpfCnpjUsuarioPagador,
                        autorizacaoRecorrencia.ContaUsuarioPagador,
                        autorizacaoRecorrencia.AgenciaUsuarioPagador,
                        autorizacaoRecorrencia.ParticipanteDoUsuarioPagador,
                        autorizacaoRecorrencia.NomeDevedor,
                        autorizacaoRecorrencia.CpfCnpjDevedor,
                        autorizacaoRecorrencia.NumeroContrato,
                        autorizacaoRecorrencia.DescObjetoContrato,
                        autorizacaoRecorrencia.CodigoSituacaoCancelamentoRecorrencia,
                        autorizacaoRecorrencia.DataUltimaAtualizacao,
                        autorizacaoRecorrencia.DataHoraCriacaoRecorr,
                        autorizacaoRecorrencia.FlagPermiteNotificacao,
                        autorizacaoRecorrencia.FlagValorMaximoAutorizado,
                        autorizacaoRecorrencia.TpRetentativa
                    });

                    session.Commit();

                    const string SQLSelect = @" SELECT 
                                                idAutorizacao AS IdAutorizacao,
                                                idRecorrencia AS IdRecorrencia,
                                                situacaoRecorrencia AS SituacaoRecorrencia,
                                                tipoRecorrencia AS TipoRecorrencia,
                                                tipoFrequencia AS TipoFrequencia,
                                                dataInicialAutorizacaoRecorrencia AS DataInicialAutorizacaoRecorrencia,
                                                dataFinalAutorizacaoRecorrencia AS DataFinalAutorizacaoRecorrencia,
                                                codigoMoedaAutorizacaoRecorrencia AS CodigoMoedaAutorizacaoRecorrencia,
                                                valorRecorrencia AS ValorRecorrencia,
                                                valorMaximoAutorizado AS ValorMaximoAutorizado,
                                                motivoRejeicaoRecorrencia AS MotivoRejeicaoRecorrencia,
                                                nomeUsuarioRecebedor AS NomeUsuarioRecebedor,
                                                cpfCnpjUsuarioRecebedor AS CpfCnpjUsuarioRecebedor,
                                                participanteDoUsuarioRecebedor AS ParticipanteDoUsuarioRecebedor,
                                                codMunIBGE AS CodMunIBGE,
                                                cpfCnpjUsuarioPagador AS CpfCnpjUsuarioPagador,
                                                contaUsuarioPagador AS ContaUsuarioPagador,
                                                agenciaUsuariopagador AS AgenciaUsuariopagador,
                                                participanteDousuarioPagador AS ParticipanteDousuarioPagador,
                                                nomeDevedor AS NomeDevedor,
                                                cpfCnpjDevedor AS CpfCnpjDevedor,
                                                numeroContrato AS NumeroContrato,
                                                descObjetoContrato AS DescObjetoContrato,
                                                codigoSituacaoCancelamentoRecorrencia AS CodigoSituacaoCancelamentoRecorrencia,
                                                dataUltimaAtualizacao AS DataUltimaAtualizacao,
                                                dataHoraCriacaoRecorr AS dataHoraCriacaoRecorr,
                                                flagPermitenotificacao AS FlagPermitenotificacao,
                                                flagValorMaximoAutorizado AS FlagValorMaximoAutorizado,
                                                tpRetentativa AS TpRetentativa
                                                                FROM AUTORIZACAO_RECORRENCIA
                                                                WHERE idAutorizacao = @Id";

                    var idAutorizacao = autorizacaoRecorrencia.IdAutorizacao;
                    var data = Db.Query<AutorizacaoRecorrencia>(SQLSelect, new { Id = idAutorizacao }).ToList().FirstOrDefault();
                    return data;

                }
                catch (Exception)
                {
                    session.Rollback();
                    //Logger.Information("Finish insert transaction - Error: @e.Message", e.Message);
                    throw;
                }
            }
        }

        public async Task<AtualizacaoAutorizacaoRecorrencia> InsertAtualizacoesAutorizacaoRecorrencia(AutorizacaoRecorrencia autorizacaoRecorrencia)
        {
            using (var session = this.Db.CreateSession())
            {
                session.Begin();
                try
                {
                    //Logger.Information("Starting insert transaction");

                    //TODO: Verify
                    string tipoSituacaoRecorrencia = autorizacaoRecorrencia.SituacaoRecorrencia;
                    DateTime dataHoraSituacaoRecorrencia = autorizacaoRecorrencia.DataHoraCriacaoRecorr;
                    DateTime dataUltimaAtualizacao = autorizacaoRecorrencia.DataUltimaAtualizacao;

                    const string query = @" INSERT INTO ATUALIZACOES_AUTORIZACOES_RECORRENCIA (idAutorizacao,
                                                                        idRecorrencia,
                                                                        tipoSituacaoRecorrencia,
                                                                        dataHoraSituacaoRecorrencia,
                                                                        dataUltimaAtualizacao)
                                                        VALUES (@IdAutorizacao,
                                                                @IdRecorrencia,
                                                                @TipoSituacaoRecorrencia,
                                                                @DataHoraSituacaoRecorrencia,
                                                                @DataUltimaAtualizacao) ";

                    session.Execute(query, new
                    {
                        autorizacaoRecorrencia.IdAutorizacao,
                        autorizacaoRecorrencia.IdRecorrencia,
                        tipoSituacaoRecorrencia,
                        dataHoraSituacaoRecorrencia,
                        dataUltimaAtualizacao
                    });

                    session.Commit();

                    //var idAutorizacao = autorizacaoRecorrencia.IdAutorizacao;
                    //var sql = @"SELECT idAutorizacao AS IdAutorizacao,
                    //                   idRecorrencia AS IdRecorrencia,
                    //                   tipoSituacaoRecorrencia AS TipoSituacaoRecorrencia,
                    //                   dataHoraSituacaoRecorrencia AS dataHoraSituacaoRecorrencia,
                    //                   dataUltimaAtualizacao AS DataUltimaAtualizacao
                    //                FROM ATUALIZACOES_AUTORIZACOES_RECORRENCIA 
                    //            WHERE idAutorizacao = @IdAutorizacao";

                    //var atualizacao = Db.Query<AtualizacaoAutorizacaoRecorrencia>(sql, new { IdAutorizacao = idAutorizacao }).FirstOrDefault();
                    return null;
                }
                catch (Exception)
                {
                    session.Rollback();
                    //Logger.Information("Finish insert transaction - Error: @e.Message", e.Message);
                    throw;

                }
            }
        }

        public async Task<AtualizacaoAutorizacaoRecorrencia> InsertAtualizacoesAutorizacaoRecorrencia(AtualizacaoAutorizacaoRecorrencia atualizacaoAutorizacao)
        {
            using (var session = this.Db.CreateSession())
            {
                session.Begin();
                try
                {
                    //Logger.Information("Starting insert transaction");

                    ////TODO: Verify

                    const string query = @" INSERT INTO ATUALIZACOES_AUTORIZACOES_RECORRENCIA (idAutorizacao,
                                                                        idRecorrencia,
                                                                        tipoSituacaoRecorrencia,
                                                                        dataHoraSituacaoRecorrencia,
                                                                        dataUltimaAtualizacao)
                                                        VALUES (@IdAutorizacao,
                                                                @IdRecorrencia,
                                                                @strTipoSituacaoRecorrencia,
                                                                @DataHoraSituacaoRecorrencia,
                                                                @DataUltimaAtualizacao) ";

                    string strTipoSituacaoRecorrencia = atualizacaoAutorizacao.TipoSituacaoRecorrencia.ToString();

                    session.Execute(query, new
                    {
                        atualizacaoAutorizacao.IdAutorizacao,
                        atualizacaoAutorizacao.IdRecorrencia,
                        strTipoSituacaoRecorrencia,
                        atualizacaoAutorizacao.DataHoraSituacaoRecorrencia,
                        atualizacaoAutorizacao.DataUltimaAtualizacao
                    });

                    session.Commit();

                    return atualizacaoAutorizacao;
                }
                catch (Exception)
                {
                    session.Rollback();
                    //Logger.Information("Finish insert transaction - Error: @e.Message", e.Message);
                    throw;

                }
            }
        }

        #endregion

        #region AlterarAutorizacaoRecorrencia
        public async Task<IEnumerable<AtualizacaoAutorizacaoRecorrencia>> ObterAtualizacoesAsync()
        {
            //Logger.Information("Obtendo todas as atualizacoes de autorizacoes de recorrencia");

            const string query = @"
            SELECT
                idAutorizacao AS IdAutorizacao
                ,tipoSituacaoRecorrencia AS TipoSituacaoRecorrencia
                ,idRecorrencia AS IdRecorrencia
                ,dataHoraSituacaoRecorrencia AS DataHoraSituacaoRecorrencia 
                ,dataUltimaAtualizacao AS DataUltimaAtualizacao
            FROM ATUALIZACOES_AUTORIZACOES_RECORRENCIA";

            using (var session = Db.CreateSession())
            {
                var result = await session.QueryAsync<AtualizacaoAutorizacaoRecorrencia>(query);
                return result;
            }
        }

        public async Task<AtualizacaoAutorizacaoRecorrencia> ObterAtualizacoesPorIdAsync(Guid id)
        {
            const string query = @"
            SELECT
                idAutorizacao AS IdAutorizacao
                ,tipoSituacaoRecorrencia AS TipoSituacaoRecorrencia
                ,idRecorrencia AS IdRecorrencia
                ,dataHoraSituacaoRecorrencia AS DataHoraSituacaoRecorrencia 
                ,dataUltimaAtualizacao AS DataUltimaAtualizacao
            FROM ATUALIZACOES_AUTORIZACOES_RECORRENCIA
            WHERE idAutorizacao = @Id";

            using (var session = Db.CreateSession())
            {
                var result = await session.QueryAsync<AtualizacaoAutorizacaoRecorrencia>(query, new { Id = id });
                return result.FirstOrDefault();
            }
        }

        public void Update(AutorizacaoRecorrencia autorizacao)
        {
            //Logger.Information("Atualizando a autorizacao de recorrencia com ID: {IdAutorizacao} e TIPO: {TipoSituacaoRecorrencia}",
            //    atualizacoes.IdAutorizacao,
            //    atualizacoes.TipoSituacaoRecorrencia.ToString());

            const string query = @"
                UPDATE AUTORIZACAO_RECORRENCIA
                SET 
                    valorMaximoAutorizado = @ValorMaximoAutorizado,
                    flagValorMaximoAutorizado = @FlagValorMaximoAutorizado,
                    flagPermiteNotificacao = @FlagPermiteNotificacao,
                    situacaoRecorrencia = @SituacaoRecorrencia,
                    motivoRejeicaoRecorrencia = @MotivoRejeicaoRecorrencia,
                    codigoSituacaoCancelamentoRecorrencia = @CodigoSituacaoCancelamentoRecorrencia,
                    dataUltimaAtualizacao = @DataUltimaAtualizacao
                WHERE idAutorizacao = @IdAutorizacao";

            using (var session = Db.CreateSession())
            {
                session.Execute(query, new
                {
                    autorizacao.IdAutorizacao,
                    autorizacao.ValorMaximoAutorizado,
                    autorizacao.FlagValorMaximoAutorizado,
                    autorizacao.FlagPermiteNotificacao,
                    autorizacao.SituacaoRecorrencia,
                    autorizacao.MotivoRejeicaoRecorrencia,
                    autorizacao.CodigoSituacaoCancelamentoRecorrencia,
                    autorizacao.DataUltimaAtualizacao,
                });
            }

            //Logger.Information("Atualizacao concluida para o ID: {IdAutorizacao} e TIPO: {TipoSituacaoRecorrencia}",
            //    atualizacoes.IdAutorizacao,
            //    atualizacoes.TipoSituacaoRecorrencia.ToString());

        }
        #endregion

        public async Task<ListaAutorizacaoRecPaginada> GetAllAsync(GetListaAutorizacaoRecDTOPaginada data)
        {
            string sqlCount = $"SELECT COUNT(*) " +
                            "FROM [dbo].[AUTORIZACAO_RECORRENCIA] " +
                            "WHERE cpfCnpjUsuarioPagador = @CpfCnpjUsuarioPagador ";

            string sqlQuery = $"SELECT idAutorizacao IdAutorizacao," +
                            "nomeUsuarioRecebedor NomeUsuarioRecebedor, " +
                            "situacaoRecorrencia SituacaoRecorrencia " +
                            "FROM [dbo].[AUTORIZACAO_RECORRENCIA] " +
                            "WHERE cpfCnpjUsuarioPagador = @CpfCnpjUsuarioPagador " +
                            $"AND contaUsuarioPagador = {int.Parse(data.ContaUsuarioPagador)}";

            if (!String.IsNullOrEmpty(data.SituacaoRecorrencia))
            {
                sqlCount += "AND situacaoRecorrencia = @SituacaoRecorrencia";
                sqlQuery += "AND situacaoRecorrencia = @SituacaoRecorrencia";;
            }
            if (!String.IsNullOrEmpty(data.NomeUsuarioRecebedor))
            {
                sqlCount += $" AND nomeUsuarioRecebedor COLLATE Latin1_General_CI_AI LIKE '%{data.NomeUsuarioRecebedor}%'";
                sqlQuery += $" AND nomeUsuarioRecebedor COLLATE Latin1_General_CI_AI LIKE '%{data.NomeUsuarioRecebedor}%'";;
            }
            if(!String.IsNullOrEmpty(data.AgenciaUsuarioPagador?.ToString()))
            {
                sqlCount += $" AND agenciaUsuarioPagador = {int.Parse(data.AgenciaUsuarioPagador)}";
                sqlQuery += $" AND agenciaUsuarioPagador = {int.Parse(data.AgenciaUsuarioPagador)}";;
            }

            int offset = (data.Page - 1) * data.PageSize;

            sqlQuery += $" ORDER BY dataHoraCriacaoRecorr OFFSET {offset} ROWS FETCH NEXT @PageSize ROWS ONLY";

            int totalItems = await _dbContext.ExecuteScalarAsync(sqlCount, data);

            var items = await _dbContext.QueryAsync<AutorizacaoRecList>(sqlQuery, data);

            return new ListaAutorizacaoRecPaginada()
            {
                Items = items,
                TotalItems = totalItems
            };
        }
        public async Task<AutorizacaoRecPagination> GetAsync(GetAutorizacaoRecDTOPaginada data)
        {
            string sqlCount = $"SELECT COUNT(*) " +
                                        "FROM [dbo].[AUTORIZACAO_RECORRENCIA] " +
                                        "WHERE idAutorizacao = @IdAutorizacao AND " + 
                                        "idRecorrencia = @IdRecorrencia";

            string sqlQuery = $"SELECT * FROM [dbo].[AUTORIZACAO_RECORRENCIA] " +
                            $"WHERE idAutorizacao = @IdAutorizacao AND " + 
                            "idRecorrencia = @IdRecorrencia";

            int offset = (data.Page - 1) * data.PageSize;

            sqlQuery += $" ORDER BY dataHoraCriacaoRecorr OFFSET {offset} ROWS FETCH NEXT @PageSize ROWS ONLY";

            int totalItems = await _dbContext.ExecuteScalarAsync(sqlCount, data);

            var items = await _dbContext.QueryAsync<AutorizacaoRecorrenciaDetalhes>(sqlQuery, data);

            return new AutorizacaoRecPagination()
            {
                Items = items,
                TotalItems = totalItems
            };
        }


    }
}
