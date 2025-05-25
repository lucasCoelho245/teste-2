namespace Pay.Recorrencia.Gestao.Domain.DTO
{
        public class InfoAdicional
    {
        public string Valor { get; set; }
        public string Nome { get; set; }
    }
    public class ContaDict
    {
        public string IdFimAFim { get; set; }   
        public string? IdRequisicao { get; set; }
        public string? DataAberturaConta { get; set; }
        public string? TpPessoa { get; set; }
        public string? Conta { get; set; }
        public string? CpfCnpjPessoa { get; set; }
        public List<string> MotivoFraudeCPFCNPJ { get; set; }
        public string? DataSolicitacaoReivindicacao { get; set; }
        public string? TextoChave { get; set; }
        public string? DataCriacaoChave { get; set; }
        public string? OwnerStatistics { get; set; }
        public string? TpConta { get; set; }
        public int IdContadict { get; set; }
        public string? NomeFantasiaPessoa { get; set; }
        public string? Agencia { get; set; }
        public string? SpbParticipante { get; set; }
        public bool ContaAtiva { get; set; }
        public string? IdCID { get; set; }
        public string? KeyStatistics { get; set; }
        public List<string> MotivoFraudeChave { get; set; }
        public bool Fraude { get; set; }
        public string? DataPosseChave { get; set; }
        public string? NomePessoa { get; set; }
        public string? TpChave { get; set; }
        public bool Bloqueado { get; set; }
    }
    public class PayloadQRCode
    {
        public string? NrSpbRecebedor { get; set; }
        public string? NrContaRecebedor { get; set; }
        public string? CidadeRecebedor { get; set; }
        public string? AccessToken { get; set; }
        public DateTime DtExpiracao { get; set; }
        public string? NrCpfCnpjPessoaRecebedor { get; set; }
        public decimal VlDesconto { get; set; }
        public decimal VlJuros { get; set; }
        public string? Pss { get; set; }
        public bool? IcPermiteAlteracaoValorSaque { get; set; }
        public string? UfRecebedor { get; set; }
        public DateTime DtFinalPagamento { get; set; }
        public string? TxEmailPagador { get; set; }
        public string? TpPessoaPagador { get; set; }
        public string? IdRevisaoPayload { get; set; }
        public decimal VlAbatimento { get; set; }
        public int PointOfInitiationMethod { get; set; }
        public string? IdRecorrencia { get; set; }
        public string? LogradouroRecebedor { get; set; }
        public string? IdFimAFim { get; set; }
        public decimal VlPagto { get; set; }
        public decimal VlOriginal { get; set; }
        public string? NmPessoaPagador { get; set; }
        public string? TpPessoaRecebedor { get; set; }
        public string? InfoKey { get; set; }
        public string? TpContaRecebedor { get; set; }
        public string? IdPayload { get; set; }
        public string? TpRetentativa { get; set; }
        public string? NrCpfCnpjPessoaDevedor { get; set; }
        public string? TxEnderecoPagador { get; set; }
        public decimal? VlSaque { get; set; }
        public string? CepRecebedor { get; set; }
        public string? ModalidadeSaque { get; set; }
        public string? NrTelefonePagador { get; set; }
        public string? StatusRecorrencia { get; set; }
        public decimal VlRecorrencia { get; set; }
        public string? ModalidadeTroco { get; set; }
        public string? TxSolicitacaoPagador { get; set; }
        public DateTime DtPrimeiroPagamento { get; set; }
        public decimal? VlTroco { get; set; }
        public string? IdInternoOrigem { get; set; }
        public bool? IcPermiteAlteracaoValorTroco { get; set; }
        public string? NmPessoaDevedor { get; set; }
        public string? NrCpfCnpjPessoaPagador { get; set; }
        public string? TxInformacoesAdicionais { get; set; }
        public bool Ok { get; set; }
        public string? TxChave { get; set; }
        public decimal VlMinimoRecorrencia { get; set; }
        public decimal VlMulta { get; set; }
        public decimal VlDocumento { get; set; }
        public bool IcPermiteAlteracaoValor { get; set; }
        public List<InfoAdicional> InfoAdicionais { get; set; }
        public object? Erros { get; set; }
        public ContaDict Contadict { get; set; }
        public string? NrAgenciaRecebedor { get; set; }
        public DateTime DhCriacaoRecorrencia { get; set; }
        public string? TpPeriodicidade { get; set; }
        public string? TpQrCode { get; set; }
        public string? NmPessoaRecebedor { get; set; }
        public DateTime DtVencimento { get; set; }
        public string? PrestadorServicoTroco { get; set; }
        public string? PrestadorServicoSaque { get; set; }
        public string? ObjetoVinculo { get; set; }
        public string? IdTransacao { get; set; }
        public string? DsStatus { get; set; }
    }
    public class TraducaoQRCodeDTO
    {
        public PayloadQRCode PaymentData { get; set; }
        public string TxQRCodePadraoEMV { get; set; }
        public string Ispb { get; set; }
    }
}