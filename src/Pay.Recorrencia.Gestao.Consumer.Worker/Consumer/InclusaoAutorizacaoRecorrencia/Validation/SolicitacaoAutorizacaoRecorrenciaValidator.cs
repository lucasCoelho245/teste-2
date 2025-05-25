using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.InclusaoAutorizacaoRecorrencia.Validation
{
    public static class SolicitacaoAutorizacaoRecorrenciaValidator
    {
        public static bool Validar(SolicitacaoAutorizacaoRecorrencia solicitacaoAutorizacaoRecorrencia)
        {
            if (string.IsNullOrEmpty(solicitacaoAutorizacaoRecorrencia.IdRecorrencia))
                return false;

            if (!solicitacaoAutorizacaoRecorrencia.TipoRecorrencia.ToString().Equals("RCUR"))
                return false;

            if (!solicitacaoAutorizacaoRecorrencia.TipoFrequencia.ToString().Equals("MIAN")
                && !solicitacaoAutorizacaoRecorrencia.TipoFrequencia.ToString().Equals("MNTH")
                && !solicitacaoAutorizacaoRecorrencia.TipoFrequencia.ToString().Equals("QURT")
                && !solicitacaoAutorizacaoRecorrencia.TipoFrequencia.ToString().Equals("WEEK")
                && !solicitacaoAutorizacaoRecorrencia.TipoFrequencia.ToString().Equals("YEAR"))
            {
                return false;
            }

            if (string.IsNullOrEmpty(solicitacaoAutorizacaoRecorrencia.DataInicialRecorrencia.ToString()))
                return false;

            if (!string.IsNullOrEmpty(solicitacaoAutorizacaoRecorrencia.CodigoMoedaSolicRecorr) &&
                !solicitacaoAutorizacaoRecorrencia.CodigoMoedaSolicRecorr.ToString().Equals("BRL"))
            {
                return false;
            }

            if (string.IsNullOrEmpty(solicitacaoAutorizacaoRecorrencia.NomeUsuarioRecebedor))
                return false;

            if (string.IsNullOrEmpty(solicitacaoAutorizacaoRecorrencia.CpfCnpjUsuarioRecebedor))
                return false;

            if (string.IsNullOrEmpty(solicitacaoAutorizacaoRecorrencia.ParticipanteDoUsuarioRecebedor))
                return false;

            if (string.IsNullOrEmpty(solicitacaoAutorizacaoRecorrencia.CpfCnpjUsuarioPagador))
                return false;

            if (string.IsNullOrEmpty(solicitacaoAutorizacaoRecorrencia.ContaUsuarioPagador.ToString()))
                return false;

            if (string.IsNullOrEmpty(solicitacaoAutorizacaoRecorrencia.ParticipanteDoUsuarioPagador))
                return false;

            if (string.IsNullOrEmpty(solicitacaoAutorizacaoRecorrencia.NumeroContrato))
                return false;

            if (!solicitacaoAutorizacaoRecorrencia.TpRetentativa.Equals("NAO_PERMITE") 
                && !solicitacaoAutorizacaoRecorrencia.TpRetentativa.Equals("PERMITE_3R_7D"))
            {
                return false;
            }

            return true;
        }
    }
}
