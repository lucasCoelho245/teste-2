using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Infrastructure.Repositories
{
    public class MockAutorizacaoRecorrenciaRepository : IMockAutorizacaoRecorrenciaRepository
    {
        public Task<AutorizacaoRecorrencia> ConsultaAutorizacao(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<AtualizacaoAutorizacaoRecorrencia> ConsultarAtualizacaoAutorizacaoRecorrencia(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ListaAutorizacaoRecPaginada> GetAllAsync(GetListaAutorizacaoRecDTOPaginada request)
        {
            var lista = new List<dynamic>
            {
                new
                {
                    CpfCnpjUsuarioPagador = "98765432100",
                    IdRecorrencia = "REC01",
                    IdAutorizacao = "AUT1",
                    NomeUsuarioRecebedor = "João da Silva",
                    SituacaoRecorrencia = "CFDB",
                    ContaUsuarioPagador = "1234"
                },
                new
                {
                    CpfCnpjUsuarioPagador = "98765432100",
                    IdRecorrencia = "REC02",
                    IdAutorizacao = "AUT1",
                    NomeUsuarioRecebedor = "João da Silva",
                    SituacaoRecorrencia = "CCLD",
                    ContaUsuarioPagador = "1234"
                },
                new
                {
                    CpfCnpjUsuarioPagador = "98765432101",
                    IdRecorrencia = "REC03",
                    IdAutorizacao = "AUT1",
                    NomeUsuarioRecebedor = "Carlos Souza",
                    SituacaoRecorrencia = "CFDB",
                    ContaUsuarioPagador = "12345"
                },
                new
                {
                    CpfCnpjUsuarioPagador = "98765432101",
                    IdRecorrencia = "REC04",
                    IdAutorizacao = "AUT1",
                    NomeUsuarioRecebedor = "Eduardo Silva",
                    SituacaoRecorrencia = "CCLD",
                    ContaUsuarioPagador = "123456"
                }
            };

            IEnumerable<dynamic> dataFilter = lista.Where(item => item.CpfCnpjUsuarioPagador == request.CpfCnpjUsuarioPagador);

            if (!string.IsNullOrEmpty(request.NomeUsuarioRecebedor))
            {
                dataFilter = dataFilter.Where(item => item.NomeUsuarioRecebedor == request.NomeUsuarioRecebedor);
            }

            if (!string.IsNullOrEmpty(request.SituacaoRecorrencia))
            {
                dataFilter = dataFilter.Where(item => item.SituacaoRecorrencia == request.SituacaoRecorrencia);
            }
            if (!string.IsNullOrEmpty(request.ContaUsuarioPagador))
            {
                dataFilter = dataFilter.Where(item => item.ContaUsuarioPagador == request.ContaUsuarioPagador);
            }

            IList<AutorizacaoRecList>? dataDTO = [];

            foreach (var item in dataFilter)
            {
                dataDTO.Add(new AutorizacaoRecList
                {
                    IdAutorizacao = item.IdAutorizacao,
                    NomeUsuarioRecebedor = item.NomeUsuarioRecebedor,
                    SituacaoRecorrencia = item.SituacaoRecorrencia
                });
            }

            return await Task.FromResult(
                new ListaAutorizacaoRecPaginada()
                {
                    Items = dataDTO,
                    TotalItems = dataDTO.Count()
                });
        }

        public Task<AutorizacaoRecNonPagination> GetAsync(GetAutorizacaoRecDTOPaginada request)
        {
            var lista = new List<AutorizacaoRecorrencia>
            {
                new AutorizacaoRecorrencia
                {
                    IdAutorizacao = "b5e5b6c9-4c1f-4e7e-bb9a-3d8e5f9c5f1a",
                    IdRecorrencia = "d2f7e3a0-8b6f-4c2a-a8f3-1e9b7a8d2e4f",
                    SituacaoRecorrencia = "CFDB",
                    TipoRecorrencia = "Mensal",
                    TipoFrequencia = "01M",
                    DataInicialAutorizacaoRecorrencia = DateTime.Parse("2025-05-01"),
                    DataFinalAutorizacaoRecorrencia = DateTime.Parse("2025-12-01"),
                    CodigoMoedaAutorizacaoRecorrencia = "BRL",
                    ValorRecorrencia = 150.00m,
                    ValorMaximoAutorizado = 200.00m,
                    MotivoRejeicaoRecorrencia = "",
                    NomeUsuarioRecebedor = "João da Silva",
                    CpfCnpjUsuarioRecebedor = "12345678901",
                    ParticipanteDoUsuarioRecebedor = "34100001",
                    CodMunIBGE = "3550308",
                    CpfCnpjUsuarioPagador = "98765432100",
                    ContaUsuarioPagador = 12345678,
                    AgenciaUsuarioPagador = 1234,
                    ParticipanteDoUsuarioPagador = "001",
                    NomeDevedor = "Empresa XYZ",
                    CpfCnpjDevedor = "00998877665544",
                    NumeroContrato = "CONTR123456789",
                    TipoSituacaoRecorrencia = "Normal",
                    DescObjetoContrato = "Assinatura mensal",
                    CodigoSituacaoCancelamentoRecorrencia = "",
                    DataHoraCriacaoRecorr = DateTime.Parse("2025-04-29 10:00:00"),
                    DataUltimaAtualizacao = DateTime.Parse("2025-04-29 10:05:00"),
                    FlagPermiteNotificacao = true,
                    FlagValorMaximoAutorizado = true,
                    TpRetentativa = "Automatica"
                },
                new AutorizacaoRecorrencia
                {
                    IdAutorizacao = "f1e2d3c4-b5a6-7f8e-9d0c-1b2a3c4d5e6f",
                    IdRecorrencia = "9a8b7c6d-5e4f-3a2b-1c0d-e9f8a7b6c5d4",
                    SituacaoRecorrencia = "CFDB",
                    TipoRecorrencia = "Anual",
                    TipoFrequencia = "01Y",
                    DataInicialAutorizacaoRecorrencia = DateTime.Parse("2025-06-01"),
                    DataFinalAutorizacaoRecorrencia = DateTime.Parse("2026-06-01"),
                    CodigoMoedaAutorizacaoRecorrencia = "BRL",
                    ValorRecorrencia = 1200.00m,
                    ValorMaximoAutorizado = 1500.00m,
                    MotivoRejeicaoRecorrencia = "",
                    NomeUsuarioRecebedor = "Carlos Souza",
                    CpfCnpjUsuarioRecebedor = "98712345600",
                    ParticipanteDoUsuarioRecebedor = "23700001",
                    CodMunIBGE = "3304557",
                    CpfCnpjUsuarioPagador = "98765432100",
                    ContaUsuarioPagador = 11223344,
                    AgenciaUsuarioPagador = 5678,
                    ParticipanteDoUsuarioPagador = "002",
                    NomeDevedor = "Empresa ABC",
                    CpfCnpjDevedor = "00112233445566",
                    NumeroContrato = "CONTR987654321",
                    TipoSituacaoRecorrencia = "Normal",
                    DescObjetoContrato = "Assinatura anual",
                    CodigoSituacaoCancelamentoRecorrencia = "",
                    DataHoraCriacaoRecorr = DateTime.Parse("2025-04-29 11:00:00"),
                    DataUltimaAtualizacao = DateTime.Parse("2025-04-29 11:05:00"),
                    FlagPermiteNotificacao = true,
                    FlagValorMaximoAutorizado = false,
                    TpRetentativa = "Manual"
                }, new AutorizacaoRecorrencia
                {
                    IdAutorizacao = "4f3e2d1c-0b9a-8f7e-6d5c-4b3a2f1e0d9c",
                    IdRecorrencia = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d",
                    SituacaoRecorrencia = "CCLD",
                    TipoRecorrencia = "Semanal",
                    TipoFrequencia = "01W",
                    DataInicialAutorizacaoRecorrencia = DateTime.Parse("2025-07-01"),
                    DataFinalAutorizacaoRecorrencia = DateTime.Parse("2025-07-31"),
                    CodigoMoedaAutorizacaoRecorrencia = "BRL",
                    ValorRecorrencia = 50.00m,
                    ValorMaximoAutorizado = 100.00m,
                    MotivoRejeicaoRecorrencia = "Solicitação do cliente",
                    NomeUsuarioRecebedor = "Eduardo Silva",
                    CpfCnpjUsuarioRecebedor = "98712345600",
                    ParticipanteDoUsuarioRecebedor = "23700001",
                    CodMunIBGE = "2927408",
                    CpfCnpjUsuarioPagador = "98765432100",
                    ContaUsuarioPagador = 55667788,
                    AgenciaUsuarioPagador = 9101,
                    ParticipanteDoUsuarioPagador = "003",
                    NomeDevedor = "Empresa DEF",
                    CpfCnpjDevedor = "00998877664000",
                    NumeroContrato = "CONTR456789123",
                    TipoSituacaoRecorrencia = "Cancelada",
                    DescObjetoContrato = "Assinatura semanal",
                    CodigoSituacaoCancelamentoRecorrencia = "001",
                    DataHoraCriacaoRecorr = DateTime.Parse("2025-04-29 12:00:00"),
                    DataUltimaAtualizacao = DateTime.Parse("2025-04-29 12:05:00"),
                    FlagPermiteNotificacao = false,
                    FlagValorMaximoAutorizado = true,
                    TpRetentativa = "Automatica"
                }
            };

            IEnumerable<dynamic>? dataFilter = lista.Where(item => item.IdAutorizacao == request.IdAutorizacao.ToString() && item.IdRecorrencia == request.IdRecorrencia.ToString());

            return Task.FromResult(
                new AutorizacaoRecNonPagination()
                {
                    Data = dataFilter.FirstOrDefault(),
                });
        }


        public Task<AtualizacaoAutorizacaoRecorrencia> InsertAtualizacoesAutorizacaoRecorrencia(AutorizacaoRecorrencia atualizacaoAutorizacaoRecorrencia)
        {
            throw new NotImplementedException();
        }

        public Task<AtualizacaoAutorizacaoRecorrencia> InsertAtualizacoesAutorizacaoRecorrencia(AtualizacaoAutorizacaoRecorrencia atualizacaoAutorizacao)
        {
            throw new NotImplementedException();
        }

        public Task<AutorizacaoRecorrencia> InsertAutorizacaoRecorrencia(AutorizacaoRecorrencia autorizacaoRecorrencia)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AtualizacaoAutorizacaoRecorrencia>> ObterAtualizacoesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AtualizacaoAutorizacaoRecorrencia> ObterAtualizacoesPorIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(AutorizacaoRecorrencia autorizacao)
        {
            throw new NotImplementedException();
        }
    }
}