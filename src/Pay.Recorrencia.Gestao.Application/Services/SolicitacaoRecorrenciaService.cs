using System;
using System.Threading.Tasks;
using Pay.Recorrencia.Gestao.Application.Services.Interfaces;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Services
{
    public class SolicitacaoRecorrenciaService : ISolicitacaoRecorrenciaService
    {
        private readonly ISolicitacaoRecorrenciaRepository _repository;

        public SolicitacaoRecorrenciaService(ISolicitacaoRecorrenciaRepository repository)
        {
            _repository = repository;
        }

        public async Task IncluirAsync(SolicitacaoRecorrenciaEntrada entrada, bool status)
        {
            var agora = DateTime.UtcNow.ToString("o"); // ISO 8601

            var entidade = new SolicitacaoRecorrencia
            {
                IdSolicRecorrencia = entrada.IdSolicRecorrencia,
                IdRecorrencia = entrada.IdRecorrencia,
                TipoRecorrencia = "RCUR",
                TipoFrequencia = entrada.TipoFrequencia,
                DataInicialRecorrencia = entrada.DataInicialRecorrencia,
                DataFinalRecorrencia = entrada.DataFinalRecorrencia,
                SituacaoSolicRecorrencia = status ? "PDNG" : "RJCT",
                CodigoMoedaSolicRecorr = string.IsNullOrWhiteSpace(entrada.ValorFixoSolicRecorrencia) ? null : "BRL",
                ValorFixoSolicRecorrencia = entrada.ValorFixoSolicRecorrencia,
                IndicadorValorMin = string.IsNullOrWhiteSpace(entrada.ValorMinRecebedorSolicRecorr) ? "false" : "true",
                ValorMinRecebedorSolicRecorr = entrada.ValorMinRecebedorSolicRecorr,
                NomeUsuarioRecebedor = entrada.NomeUsuarioRecebedor,
                CpfCnpjUsuarioRecebedor = entrada.CpfCnpjUsuarioRecebedor,
                ParticipanteDoUsuarioRecebedor = entrada.ParticipanteDoUsuarioRecebedor,
                CpfCnpjUsuarioPagador = entrada.CpfCnpjUsuarioPagador,
                ContaUsuarioPagador = entrada.ContaUsuarioPagador,
                AgenciaUsuarioPagador = entrada.AgenciaUsuarioPagador,
                NomeDevedor = entrada.NomeDevedor,
                CpfCnpjDevedor = entrada.CpfCnpjDevedor,
                NumeroContrato = entrada.NumeroContrato,
                DescObjetoContrato = entrada.DescObjetoContrato,
                DataHoraCriacaoRecorr = entrada.DataHoraCriacaoRecorr,
                DataHoraCriacaoSolicRecorr = entrada.DataHoraCriacaoSolicRecorr,
                DataHoraExpiracaoSolicRecorr = entrada.DataHoraExpiracaoSolicRecorr,
                DataUltimaAtualizacao = agora
            };

            await _repository.Insert(entidade);
        }
    }
}
