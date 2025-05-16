using MediatR;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Enums;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia
{
    public sealed class AlterarAtualizacoesHandler
        : IRequestHandler<AlterarAutorizacaoCommand, AutorizacaoRecorrencia>
    {
        //private ILogger Logger { get; }
        private IAutorizacaoRecorrenciaRepository _autorizacaoRecorrenciaRepository { get; }

        //public UpdateAtualizacoesHandler(ILogger logger, IAutorizacaoRecorrenciaRepository atualizacoesRepository)
        public AlterarAtualizacoesHandler(IAutorizacaoRecorrenciaRepository autorizacaoRecorrenciaRepository)
        {
            //Logger = logger;
            _autorizacaoRecorrenciaRepository = autorizacaoRecorrenciaRepository;
        }

        public async Task<AutorizacaoRecorrencia> Handle(AlterarAutorizacaoCommand request, CancellationToken cancellationToken)
        {
            //Logger.Information("Getting ObterAtualizacoesPorIdQuery");

            if (!ValidarCamposComDominioCorreto(request))
            {
                throw new ArgumentException("ERRO-PIXAUTO-002");
            }

            var autorizacaoEncontrada = await _autorizacaoRecorrenciaRepository.ConsultaAutorizacao(request.IdAutorizacao);

            if (autorizacaoEncontrada is null)
            {
                throw new Exception("ERRO-PIXAUTO-009");
            }

            DateTime dataHoraAtual = DateTime.Now;

            autorizacaoEncontrada.ValorMaximoAutorizado = request.ValorMaximoAutorizado;
            autorizacaoEncontrada.FlagValorMaximoAutorizado = request.FlagValorMaximoAutorizado;
            autorizacaoEncontrada.FlagPermiteNotificacao = request.FlagPermiteNotificacao;
            autorizacaoEncontrada.SituacaoRecorrencia = request.SituacaoRecorrencia.ToString();
            autorizacaoEncontrada.MotivoRejeicaoRecorrencia = request.MotivoRejeicaoOcorrencia;
            autorizacaoEncontrada.CodigoSituacaoCancelamentoRecorrencia = request.CodigoSituacaoCancelamentoRecorrencia;
            autorizacaoEncontrada.DataUltimaAtualizacao = dataHoraAtual;

            _autorizacaoRecorrenciaRepository.Update(autorizacaoEncontrada);

            var atualizacao = _autorizacaoRecorrenciaRepository.ConsultarAtualizacaoAutorizacaoRecorrencia(request.IdAutorizacao);

            if (atualizacao == null)
            {
                throw new Exception("ERRO-PIXAUTO-010");
            }

            AtualizacaoAutorizacaoRecorrencia atualizacaoAutorizacao = new()
            {
                IdAutorizacao = request.IdAutorizacao,
                IdRecorrencia = autorizacaoEncontrada.IdRecorrencia,
                TipoSituacaoRecorrencia = Enum.Parse<TipoSituacaoRecorrencia>(request.TipoSituacaoRecorrencia),
                DataHoraSituacaoRecorrencia = request.DataHoraSituacaoRecorrencia,
                DataUltimaAtualizacao = dataHoraAtual
            };

            await _autorizacaoRecorrenciaRepository.InsertAtualizacoesAutorizacaoRecorrencia(atualizacaoAutorizacao);


            return autorizacaoEncontrada;
        }

        private bool ValidarCamposComDominioCorreto(AlterarAutorizacaoCommand request)
        {
            string[] dominioSituacaoRecorrencia = { "PDNG", "INPR", "CFDB", "CCLD" };
            string[] dominioTipoSituacaoRecorrencia = { "CRTN", "AUT1", "AUT2", "AUT3", "AUT4", "CFDB", "CCLD" };

            if (!dominioSituacaoRecorrencia.Contains(request.SituacaoRecorrencia.ToUpper()))
            {
                return false;
            }

            if (!dominioTipoSituacaoRecorrencia.Contains(request.TipoSituacaoRecorrencia.ToUpper()))
            {
                return false;
            }

            return true;
        }
    }
}
