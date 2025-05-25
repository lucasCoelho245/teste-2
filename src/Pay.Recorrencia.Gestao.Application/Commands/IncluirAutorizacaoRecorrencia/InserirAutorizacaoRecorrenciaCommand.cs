using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Enums;

namespace Pay.Recorrencia.Gestao.Application.Commands.IncluirAutorizacaoRecorrencia
{
    public class InserirAutorizacaoRecorrenciaCommand : IRequest<MensagemPadraoResponse>
    {
        public string IdAutorizacao { get; set; }

        public string IdRecorrencia { get; set; }

        public string SituacaoRecorrencia { get; set; }

        public string TipoRecorrencia { get; set; }

        public string TipoFrequencia { get; set; }

        public DateTime DataInicialAutorizacaoRecorrencia { get; set; }

        public DateTime? DataFinalAutorizacaoRecorrencia { get; set; }

        public string? CodigoMoedaAutorizacaoRecorrencia { get; set; }

        public decimal? ValorRecorrencia { get; set; }

        public decimal? ValorMaximoAutorizado { get; set; }

        public string MotivoRejeicaoRecorrencia { get; set; }

        public string NomeUsuarioRecebedor { get; set; }

        public string CpfCnpjUsuarioRecebedor { get; set; }

        public string ParticipanteDoUsuarioRecebedor { get; set; }

        public string CodMunIBGE { get; set; }

        public string CpfCnpjUsuarioPagador { get; set; }

        public int ContaUsuarioPagador { get; set; }

        public int? AgenciaUsuarioPagador { get; set; }

        public string ParticipanteDoUsuarioPagador { get; set; }

        public string? NomeDevedor { get; set; }

        public string? CpfCnpjDevedor { get; set; }

        public string NumeroContrato { get; set; }

        public string TipoSituacaoRecorrencia { get; set; }

        public string? DescObjetoContrato { get; set; }

        public string CodigoSituacaoCancelamentoRecorrencia { get; set; }

        public DateTime DataHoraCriacaoRecorr { get; set; }

        public DateTime DataUltimaAtualizacao { get; set; }

        public bool FlagPermiteNotificacao { get; set; }

        public bool FlagValorMaximoAutorizado { get; set; }

        public string TpRetentativa { get; set; }

        public DateTime? DataProximoPagamento { get; set; }

        public InserirAutorizacaoRecorrenciaCommand()
        {
            
        }

        public InserirAutorizacaoRecorrenciaCommand(SolicitacaoAutorizacaoRecorrencia solicitacaoAutorizacaoRecorrencia, string idAutorizacao)
        {
            IdAutorizacao = idAutorizacao;
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
            DataUltimaAtualizacao = solicitacaoAutorizacaoRecorrencia.DataUltimaAtualizacao;

            if (!solicitacaoAutorizacaoRecorrencia.ValorMinRecebedorSolicRecorr.HasValue)
            {
                ValorRecorrencia = solicitacaoAutorizacaoRecorrencia.ValorFixoSolicRecorrencia;
            }
            else
            {
                ValorRecorrencia = solicitacaoAutorizacaoRecorrencia.ValorMinRecebedorSolicRecorr;
            }

            SituacaoRecorrencia = "LIDO";
        }
    }
}
