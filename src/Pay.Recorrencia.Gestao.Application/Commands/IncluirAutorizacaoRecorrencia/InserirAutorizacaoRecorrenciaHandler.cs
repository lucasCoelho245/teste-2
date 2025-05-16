using MediatR;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Commands.IncluirAutorizacaoRecorrencia
{
    public class InserirAutorizacaoRecorrenciaHandler : IRequestHandler<InserirAutorizacaoRecorrenciaCommand, AutorizacaoRecorrencia>
    {
        //private ILogger Logger { get; }
        private IAutorizacaoRecorrenciaRepository _autorizacaoRecorrenciaRepository { get; }

        //public InserirAutorizacaoRecorrenciaHandler(IAutorizacaoRecorrenciaRepository autorizacaoRecorrenciaRepository, ILogger logger)
        public InserirAutorizacaoRecorrenciaHandler(IAutorizacaoRecorrenciaRepository autorizacaoRecorrenciaRepository)
        {
            _autorizacaoRecorrenciaRepository = autorizacaoRecorrenciaRepository;
            //Logger = logger;
        }

        public async Task<AutorizacaoRecorrencia> Handle(InserirAutorizacaoRecorrenciaCommand request, CancellationToken cancellationToken)
        {
            DateTime dataAtual = DateTime.Now;

            var consultaAutorizacaoRecorrencia = await _autorizacaoRecorrenciaRepository.ConsultaAutorizacao(request.IdAutorizacao);
            if (consultaAutorizacaoRecorrencia != null)
            {
                throw new Exception("Chave já existente na tabela AUTORIZACAO_RECORRENCIA");
            }

            var autorizacaoRecorrencia = ConverterCommandParaAutorizacaoRecorrencia(request);
            autorizacaoRecorrencia.DataUltimaAtualizacao = dataAtual;

            //Logger.Information("Inserting InsertAutorizacaoRecorrencia");
            var autorizacaoRecorrenciaCriada = await _autorizacaoRecorrenciaRepository.InsertAutorizacaoRecorrencia(autorizacaoRecorrencia);
            if (autorizacaoRecorrenciaCriada != null)
            {
                //Logger.Information("Getting ConsultarAtualizacaoAutorizacaoRecorrencia");
                var consultaAtualizacao = await _autorizacaoRecorrenciaRepository.ConsultarAtualizacaoAutorizacaoRecorrencia(autorizacaoRecorrenciaCriada.IdAutorizacao);
                if (consultaAtualizacao == null)
                {
                    //Logger.Information("Inserting InsertAtualizacoesAutorizacaoRecorrencia");
                    var consultaAut = await _autorizacaoRecorrenciaRepository.ConsultaAutorizacao(autorizacaoRecorrenciaCriada.IdAutorizacao);
                    var insercaoAtualizacao = await _autorizacaoRecorrenciaRepository.InsertAtualizacoesAutorizacaoRecorrencia(consultaAut);
                }
                else
                {
                    throw new Exception("Chave já existente na tabela ATUALIZACOES_AUTORIZACOES_RECORRENCIA");
                }
            }
            return autorizacaoRecorrenciaCriada;
        }

        private AutorizacaoRecorrencia ConverterCommandParaAutorizacaoRecorrencia(InserirAutorizacaoRecorrenciaCommand request)
        {
            return new AutorizacaoRecorrencia
            {
                IdAutorizacao = request.IdAutorizacao,
                IdRecorrencia = request.IdRecorrencia,
                SituacaoRecorrencia = request.SituacaoRecorrencia,
                TipoRecorrencia = request.TipoRecorrencia,
                TipoFrequencia = request.TipoFrequencia,
                DataInicialAutorizacaoRecorrencia = request.DataInicialAutorizacaoRecorrencia,
                DataFinalAutorizacaoRecorrencia = request.DataFinalAutorizacaoRecorrencia,
                CodigoMoedaAutorizacaoRecorrencia = request.CodigoMoedaAutorizacaoRecorrencia,
                ValorRecorrencia = request.ValorRecorrencia,
                ValorMaximoAutorizado = request.ValorMaximoAutorizado,
                MotivoRejeicaoRecorrencia = request.MotivoRejeicaoRecorrencia,
                NomeUsuarioRecebedor = request.NomeUsuarioRecebedor,
                CpfCnpjUsuarioRecebedor = request.CpfCnpjUsuarioRecebedor,
                ParticipanteDoUsuarioRecebedor = request.ParticipanteDoUsuarioRecebedor,
                CodMunIBGE = request.CodMunIBGE,
                CpfCnpjUsuarioPagador = request.CpfCnpjUsuarioPagador,
                ContaUsuarioPagador = request.ContaUsuarioPagador,
                AgenciaUsuarioPagador = request.AgenciaUsuarioPagador,
                ParticipanteDoUsuarioPagador = request.ParticipanteDoUsuarioPagador,
                NomeDevedor = request.NomeDevedor,
                CpfCnpjDevedor = request.CpfCnpjDevedor,
                NumeroContrato = request.NumeroContrato,
                TipoSituacaoRecorrencia = request.TipoSituacaoRecorrencia,
                DescObjetoContrato = request.DescObjetoContrato,
                CodigoSituacaoCancelamentoRecorrencia = request.CodigoSituacaoCancelamentoRecorrencia,
                DataHoraCriacaoRecorr = request.DataHoraCriacaoRecorr,
                DataUltimaAtualizacao = request.DataUltimaAtualizacao,
                FlagPermiteNotificacao = request.FlagPermiteNotificacao,
                FlagValorMaximoAutorizado = request.FlagValorMaximoAutorizado,
                TpRetentativa = request.TpRetentativa,
            };
        }
    }
}
