﻿using Pay.Recorrencia.Gestao.Domain.Enums;

namespace Pay.Recorrencia.Gestao.Domain.Entities
{
    public class AutorizacaoRecorrencia
    {
        public string? IdAutorizacao { get; set; }

        public string? IdRecorrencia { get; set; }

        public string? SituacaoRecorrencia { get; set; }

        public string? TipoRecorrencia { get; set; }

        public string? TipoFrequencia { get; set; }

        public DateTime DataInicialAutorizacaoRecorrencia { get; set; }

        public DateTime? DataFinalAutorizacaoRecorrencia { get; set; }

        public string? CodigoMoedaAutorizacaoRecorrencia { get; set; }

        public decimal? ValorRecorrencia { get; set; }

        public decimal? ValorMaximoAutorizado { get; set; }

        public string? MotivoRejeicaoRecorrencia { get; set; }

        public string? NomeUsuarioRecebedor { get; set; }

        public string? CpfCnpjUsuarioRecebedor { get; set; }

        public string? ParticipanteDoUsuarioRecebedor { get; set; }

        public string? CodMunIBGE { get; set; }

        public string? CpfCnpjUsuarioPagador { get; set; }

        public int ContaUsuarioPagador { get; set; }

        public int? AgenciaUsuarioPagador { get; set; }

        public string? ParticipanteDoUsuarioPagador { get; set; }

        public string? NomeDevedor { get; set; }

        public string? CpfCnpjDevedor { get; set; }

        public string? NumeroContrato { get; set; }

        public string? TipoSituacaoRecorrencia { get; set; }

        public string? DescObjetoContrato { get; set; }

        public string? CodigoSituacaoCancelamentoRecorrencia { get; set; }

        public DateTime? DataHoraCriacaoRecorr { get; set; }

        public DateTime DataUltimaAtualizacao { get; set; }

        public bool? FlagPermiteNotificacao { get; set; }

        public bool? FlagValorMaximoAutorizado { get; set; }

        public string? TpRetentativa { get; set; }

        public DateTime? DataProximoPagamento { get; set; }
        public DateTime? DataAutorizacao { get; set; }
        public DateTime? DataCancelamento { get; set; }
    }
    public class AutorizacaoRecNonPagination
    {
        public AutorizacaoRecorrencia Data { get; set; }
    }

    public class AutorizacaoRecList
    {
        public string IdAutorizacao { get; set; }
        public string NomeUsuarioRecebedor { get; set; }
        public string SituacaoRecorrencia { get; set; }
    }
    public class ListaAutorizacaoRecPaginada
    {
        public IEnumerable<AutorizacaoRecList> Items { get; set; }
        public int TotalItems { get; set; }
    }
}
