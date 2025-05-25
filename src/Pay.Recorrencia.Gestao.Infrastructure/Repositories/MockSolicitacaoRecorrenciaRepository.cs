using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Infrastructure.Repositories
{
    public class MockSolicitacaoRecorrenciaRepository : IMockSolicitacaoRecorrenciaRepository
    {
        public async Task<ListaSolicAutorizacaoRecPaginada> GetAllAsync(GetListaSolicAutorizacaoRecDTOPaginada request)
        {
            var lista = new List<dynamic>
            {
                new
                {
                    CpfCnpjUsuarioPagador = "98765432100",
                    IdSolicRecorrencia = "REC01",
                    NomeUsuarioRecebedor = "João da Silva",
                    SituacaoSolicRecorrencia = "PDNG",
                    ContaUsuarioPagador = "1234"
                },
                new
                {
                    CpfCnpjUsuarioPagador = "98765432101",
                    IdSolicRecorrencia = "REC02",
                    NomeUsuarioRecebedor = "Carlos Souza",
                    SituacaoSolicRecorrencia = "PDNG",
                    ContaUsuarioPagador = "12345"
                },
                new
                {
                    CpfCnpjUsuarioPagador = "98765432102",
                    IdSolicRecorrencia = "REC03",
                    NomeUsuarioRecebedor = "Eduardo Silva",
                    SituacaoSolicRecorrencia = "PDNG",
                    ContaUsuarioPagador = "123456"
                }
            };

            IEnumerable<dynamic> dataFilter = lista.Where(item => item.CpfCnpjUsuarioPagador == request.CpfCnpjUsuarioPagador);

            if (!string.IsNullOrEmpty(request.NomeUsuarioRecebedor))
            {
                dataFilter = dataFilter.Where(item => item.NomeUsuarioRecebedor == request.NomeUsuarioRecebedor);
            }

            if (!string.IsNullOrEmpty(request.SituacaoSolicRecorrencia))
            {
                dataFilter = dataFilter.Where(item => item.SituacaoSolicRecorrencia == request.SituacaoSolicRecorrencia);
            }
            if (!string.IsNullOrEmpty(request.ContaUsuarioPagador))
            {
                dataFilter = dataFilter.Where(item => item.ContaUsuarioPagador == request.ContaUsuarioPagador);
            }

            IList<SolicAutorizacaoRecList>? dataDTO = [];

            foreach (var item in dataFilter)
            {
                dataDTO.Add(new SolicAutorizacaoRecList
                {
                    IdSolicRecorrencia = item.IdSolicRecorrencia,
                    NomeUsuarioRecebedor = item.NomeUsuarioRecebedor,
                    SituacaoSolicRecorrencia = item.SituacaoSolicRecorrencia
                });
            }

            return await Task.FromResult(
                new ListaSolicAutorizacaoRecPaginada()
                {
                    Items = dataDTO,
                    TotalItems = dataDTO.Count()
                });
        }

        public Task<SolicAutorizacaoRecNonPagination> GetAsync(GetSolicAutorizacaoRecDTOPaginada request)
        {
            var lista = new List<SolicitacaoRecorrencia>
            {
            new SolicitacaoRecorrencia
                {
                    IdSolicRecorrencia = "REC01",
                    IdAutorizacao = "AUT01",
                    IdRecorrencia = "RCN01",
                    TipoRecorrencia = "MENS",
                    TipoFrequencia = "01D",
                    DataInicialRecorrencia = "2025-05-01",
                    DataFinalRecorrencia = "2025-12-01",
                    SituacaoSolicRecorrencia = "CCLD",
                    CodigoMoedaSolicRecorr = "986",
                    ValorFixoSolicRecorrencia = "150.00",
                    IndicadorValorMin = "1",
                    ValorMinRecebedorSolicRecorr = "100.00",
                    NomeUsuarioRecebedor = "João da Silva",
                    CpfCnpjUsuarioRecebedor = "12345678901",
                    ParticipanteDoUsuarioRecebedor = "34100001",
                    CpfCnpjUsuarioPagador = "98765432100",
                    ContaUsuarioPagador = "12345678901234567890",
                    AgenciaUsuarioPagador = "1234",
                    NomeDevedor = "Empresa XYZ",
                    CpfCnpjDevedor = "00998877665544",
                    NumeroContrato = "CONTR123456789",
                    DescObjetoContrato = "Assinatura mensal",
                    DataHoraCriacaoRecorr = "2025-04-29 10:00:00.000",
                    DataHoraCriacaoSolicRecorr = "2025-04-29 10:01:00.000",
                    DataHoraExpiracaoSolicRecorr = "2025-05-01 00:00:00.000",
                    DataUltimaAtualizacao = "2025-04-29 10:05:00.000"
                },
                new SolicitacaoRecorrencia
                {
                    IdSolicRecorrencia = "REC02",
                    IdAutorizacao = "AUT02",
                    IdRecorrencia = "RCN02",
                    TipoRecorrencia = "FIXO",
                    TipoFrequencia = "01M",
                    DataInicialRecorrencia = "2025-06-01",
                    DataFinalRecorrencia = "2025-06-01",
                    SituacaoSolicRecorrencia = "PDNG",
                    CodigoMoedaSolicRecorr = "986",
                    ValorFixoSolicRecorrencia = "150.00",
                    IndicadorValorMin = "1",
                    ValorMinRecebedorSolicRecorr = "100.00",
                    NomeUsuarioRecebedor = "João da Silva",
                    CpfCnpjUsuarioRecebedor = "12345678901",
                    ParticipanteDoUsuarioRecebedor = "34100001",
                    CpfCnpjUsuarioPagador = "98765432100",
                    ContaUsuarioPagador = "12345678901234567890",
                    AgenciaUsuarioPagador = "1234",
                    NomeDevedor = "jUK KUIG'",
                    CpfCnpjDevedor = "00998877664000",
                    NumeroContrato = "CONTR987654321",
                    DescObjetoContrato = "Assinatura mensal",
                    DataHoraCriacaoRecorr = "2025-04-29 11:00:00.000",
                    DataHoraCriacaoSolicRecorr = "2025-04-29 11:01:00.000",
                    DataHoraExpiracaoSolicRecorr = "2025-06-01 00:00:00.000",
                    DataUltimaAtualizacao = "2025-04-29 11:05:00.000"
                },
                new SolicitacaoRecorrencia
                {
                    IdSolicRecorrencia = "REC03",
                    IdAutorizacao = "AUT03",
                    IdRecorrencia = "RCN03",
                    TipoRecorrencia = "VARI",
                    TipoFrequencia = "07D",
                    DataInicialRecorrencia = "2025-07-01",
                    DataFinalRecorrencia = "2025-07-10",
                    SituacaoSolicRecorrencia = "PDNG",
                    CodigoMoedaSolicRecorr = "986",
                    ValorFixoSolicRecorrencia = "150.00",
                    IndicadorValorMin = "1",
                    ValorMinRecebedorSolicRecorr = "100.00",
                    NomeUsuarioRecebedor = "João da Silva",
                    CpfCnpjUsuarioRecebedor = "12345678901",
                    ParticipanteDoUsuarioRecebedor = "34100001",
                    CpfCnpjUsuarioPagador = "98765432100",
                    ContaUsuarioPagador = "12345678901234567890",
                    AgenciaUsuarioPagador = "1234",
                    NomeDevedor = "jUK KUIG",
                    CpfCnpjDevedor = "00998877664000",
                    NumeroContrato = "CONTR456789123",
                    DescObjetoContrato = "Assinatura mensal",
                    DataHoraCriacaoRecorr = "2025-04-29 12:00:00.000",
                    DataHoraCriacaoSolicRecorr = "2025-04-29 12:01:00.000",
                    DataHoraExpiracaoSolicRecorr = "2025-07-01 00:00:00.000",
                    DataUltimaAtualizacao = "2025-04-29 12:05:00.000"
                }
            };

            IEnumerable<dynamic>? dataFilter = lista.Where(item => item.IdSolicRecorrencia == request.IdSolicRecorrencia);

            return Task.FromResult(
                new SolicAutorizacaoRecNonPagination()
                {
                    Data = dataFilter.FirstOrDefault(),
                });
        }

        public Task<SolicitacaoRecorrencia?> GetSolicitacaoRecorrencia(string id)
        {
            throw new NotImplementedException();
        }

        public Task Insert(SolicitacaoRecorrencia solicitacaoRecorrencia)
        {
            throw new NotImplementedException();
        }

        public Task Update(SolicitacaoAutorizacaoRecorrenciaUpdateDTO solicitacaoRecorrencia)
        {
            throw new NotImplementedException();
        }
    }
}