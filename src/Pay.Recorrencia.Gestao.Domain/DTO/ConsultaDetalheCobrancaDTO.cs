using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay.Recorrencia.Gestao.Domain.DTO
{
    #region ListarPixAgendados

    public class ListarPixAgendadosRequestDTO
    {
        public string NrSpb { get; set; }
        public string? Agencia { get; set; }
        public string IdTipoConta { get; set; }
        public string Conta { get; set; }
        public string IdRecorrencia { get; set; }
        public int Situacao { get; set; } = 20;
    }

    public class ListarPixAgendadosResponseDTO
    {
        public string IdOperacao { get; set; }
        public string IdRecorrencia { get; set; }
        public decimal VlOperacao { get; set; }
        public decimal VlDocumento { get; set; }
        public DateTime DtVencimento { get; set; }
        public string IdFimAFim { get; set; }
        public string IdInicPagto { get; set; }
        public string IdConciliacaoRecebedor { get; set; }
        public string IdPayload { get; set; }
        public string IdRevisaoPayload { get; set; }
        public DateTime DtPagto { get; set; }
        public DateTime DtHrOperacao { get; set; }
        public DateTime DhLiquidacao { get; set; }
        public string NmPessoaRecebedor { get; set; }
        public string IdTipoPessoaRecebedor { get; set; }
        public string NrCpfCnpjPessoaRecebedor { get; set; }
        public string NrSpbRecebedor { get; set; }
        public string NrAgenciaRecebedor { get; set; }
        public string IdTipoContaRecebedor { get; set; }
        public string NrContaRecebedor { get; set; }
        public string NmPessoaPagador { get; set; }
        public string IdTipoPessoaPagador { get; set; }
        public string NrCpfCnpjPessoaPagador { get; set; }
        public string NrSpbPagador { get; set; }
        public string NrAgenciaPagador { get; set; }
        public string IdTipoContaPagador { get; set; }
        public string NrContaPagador { get; set; }
        public string IdTipoSituacaoAtual { get; set; }
        public string DescricaoSituacaoAtual { get; set; }
        public string IdSpi { get; set; }
        public string IdInstFinanc { get; set; }
        public string IdFilialInst { get; set; }
        public string NrDoctosGeradoContaJson { get; set; }
        public string NrSpbControlador { get; set; }
        public string NrDocumentoOrigem { get; set; }
        public string TxChave { get; set; }
        public string TxDescricao { get; set; }
        public string FlDevolucao { get; set; }
        public bool IcPermiteAlterarValor { get; set; }
        public IEnumerable<ListarPixAgendadosRetornoResponseDTO> LstRetorno { get; set; }
        public string TpFinalidade { get; set; }
        public IEnumerable<ListarPixAgendadosTipoValorResponseDTO> LstTipoValor { get; set; }
        public int ParcelaAtual { get; set; }
        public int TotalParcelas { get; set; }
        public string MotivoDevolucao { get; set; }
    }

    public class ListarPixAgendadosRetornoResponseDTO
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
    }

    public class ListarPixAgendadosTipoValorResponseDTO
    {
        public string VlTipo { get; set; }
        public string TpPagto { get; set; }
    }

    #endregion


    #region ConsultaAutorizacaoRecorrencia

    public class ConsultaAutorizacaoRecorrenciaRequestDTO
    {
        public string IdRecorrencia { get; set; }
    }

    public class ConsultaAutorizacaoRecorrenciaResponseDTO
    {
        public string IdRecorrencia { get; set; }
        public string SituacaoRecorrencia { get; set; }
        public string TipoRecorrencia { get; set; }
        public string TipoFrequencia { get; set; }
        public DateTime DataInicialAutorizacaoRecorrencia { get; set; }
        public DateTime DataFinalAutorizacaoRecorrencia { get; set; }
        public string CodigoMoedaAutorizacaoRecorrencia { get; set; }
        public decimal ValorRecorrencia { get; set; }
        public string MotivoRejeicaoRecorrencia { get; set; }
        public string NomeUsuarioRecebedor { get; set; }
        public string CpfCnpjUsuarioRecebedor { get; set; }
        public string ParticipanteDoUsuarioRecebedor { get; set; }
        public string CodMunIBGE { get; set; }
        public string CprfCnpjUsuarioPagador { get; set; }
        public string ContaUsuarioPagador { get; set; }
        public string AgenciaUsuarioPagador { get; set; }
        public string ParticipanteDoUsuarioPagador { get; set; }
        public string NomeDevedor { get; set; }
        public string CpfCnpjDevedor { get; set; }
        public string NumeroContrato { get; set; }
        public string DescObjetoContrato { get; set; }
        public string CodigoSituacaoCancelamentoRecorrencia { get; set; }
        public DateTime DataHoraCriacaoRecorr { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
        public string FlagPermiteNotificacao { get; set; }
        public string FlagValorMaximoAutorizado { get; set; }
        public string TpTentativa { get; set; }
    }

    #endregion
}
