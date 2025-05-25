using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Inclusao.Autorizacao.Recorrencia.Consumer.Consumer.DTO
{
    public class RecorrenciaIncluidaMessage
    {
        public string IdRecorrencia { get; set; }

        public string SituacaoRecorrencia { get; set; }

        public string TipoRecorrencia { get; set; }

        public string TipoFrequencia { get; set; }

        public DateTime DataInicialAutorizacaoRecorrencia { get; set; }

        public DateTime? DataFinalAutorizacaoRecorrencia { get; set; }

        public string? CodigoMoedaAutorizacaoRecorrencia { get; set; }

        public decimal? ValorRecorrencia { get; set; }

        public string NomeUsuarioRecebedor { get; set; }

        public string CpfCnpjUsuarioRecebedor { get; set; }

        public string ParticipanteDoUsuarioRecebedor { get; set; }

        public string CpfCnpjUsuarioPagador { get; set; }

        public int ContaUsuarioPagador { get; set; }

        public int? AgenciaUsuarioPagador { get; set; }

        public string ParticipanteDoUsuarioPagador { get; set; }

        public string? NomeDevedor { get; set; }

        public string? CpfCnpjDevedor { get; set; }

        public string NumeroContrato { get; set; }

        public string TipoSituacaoRecorrencia { get; set; }

        public string? DescObjetoContrato { get; set; }

        public DateTime DataHoraCriacaoRecorr { get; set; }

        public string TpRetentativa { get; set; }

        public RecorrenciaIncluidaMessage(SolicitacaoAutorizacaoRecorrencia solicitacaoAutorizacaoRecorrencia)
        {
            IdRecorrencia = solicitacaoAutorizacaoRecorrencia.IdRecorrencia;
            TipoRecorrencia = solicitacaoAutorizacaoRecorrencia.TipoRecorrencia;
            TipoFrequencia = solicitacaoAutorizacaoRecorrencia.TipoFrequencia;
            DataInicialAutorizacaoRecorrencia = solicitacaoAutorizacaoRecorrencia.DataInicialRecorrencia;
            DataFinalAutorizacaoRecorrencia = solicitacaoAutorizacaoRecorrencia.DataFinalRecorrencia; 
            CodigoMoedaAutorizacaoRecorrencia = solicitacaoAutorizacaoRecorrencia.CodigoMoedaSolicRecorr; 
            NomeUsuarioRecebedor = solicitacaoAutorizacaoRecorrencia.NomeUsuarioRecebedor;
            CpfCnpjUsuarioRecebedor = solicitacaoAutorizacaoRecorrencia.CpfCnpjUsuarioRecebedor;
            ParticipanteDoUsuarioRecebedor = solicitacaoAutorizacaoRecorrencia.ParticipanteDoUsuarioRecebedor;
            CpfCnpjUsuarioPagador = solicitacaoAutorizacaoRecorrencia.CpfCnpjUsuarioPagador;
            ContaUsuarioPagador = solicitacaoAutorizacaoRecorrencia.ContaUsuarioPagador;
            AgenciaUsuarioPagador = solicitacaoAutorizacaoRecorrencia.AgenciaUsuarioPagador;
            ParticipanteDoUsuarioPagador = solicitacaoAutorizacaoRecorrencia.ParticipanteDoUsuarioPagador;
            NomeDevedor = solicitacaoAutorizacaoRecorrencia.NomeDevedor;
            CpfCnpjDevedor = solicitacaoAutorizacaoRecorrencia.CpfCnpjDevedor;
            NumeroContrato = solicitacaoAutorizacaoRecorrencia.NumeroContrato;
            DescObjetoContrato = solicitacaoAutorizacaoRecorrencia.DescObjetoContrato;
            TpRetentativa = solicitacaoAutorizacaoRecorrencia.TpRetentativa;
            DataHoraCriacaoRecorr = solicitacaoAutorizacaoRecorrencia.DataHoraCriacaoRecorr;

            if (string.IsNullOrEmpty(solicitacaoAutorizacaoRecorrencia.ValorMinRecebedorSolicRecorr.ToString()))
            {
                ValorRecorrencia = solicitacaoAutorizacaoRecorrencia.ValorFixoSolicRecorrencia;
            }
            else
            {
                ValorRecorrencia = solicitacaoAutorizacaoRecorrencia.ValorMinRecebedorSolicRecorr;
            }

            SituacaoRecorrencia = "LIDO";
            TipoSituacaoRecorrencia = "READ";
        }
    }
}
