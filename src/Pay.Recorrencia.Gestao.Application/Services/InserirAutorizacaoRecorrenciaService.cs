using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Commands.IncluirAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Interfaces;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Enums;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Services
{
    public class InserirAutorizacaoRecorrenciaService : IInserirAutorizacaoRecorrenciaService
    {
        private IAutorizacaoRecorrenciaRepository _autorizacaoRecorrenciaRepository { get; }

        //public UpdateAtualizacoesHandler(ILogger logger, IAutorizacaoRecorrenciaRepository atualizacoesRepository)
        public InserirAutorizacaoRecorrenciaService(IAutorizacaoRecorrenciaRepository autorizacaoRecorrenciaRepository)
        {
            //Logger = logger;
            _autorizacaoRecorrenciaRepository = autorizacaoRecorrenciaRepository;
        }

        public async Task<MensagemPadraoResponse> Handle(InserirAutorizacaoRecorrenciaCommand request)
        {
            DateTime dataAtual = DateTime.Now;

            var consultaAutorizacaoRecorrencia = await _autorizacaoRecorrenciaRepository.ConsultaAutorizacao(request.IdRecorrencia);
            if (consultaAutorizacaoRecorrencia != null)
            {
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-007", "Chave já existente na tabela AUTORIZACAO_RECORRENCIA"));

            }
            var autorizacaoRecorrencia = ConverterCommandParaAutorizacaoRecorrencia(request);
            autorizacaoRecorrencia.DataUltimaAtualizacao = dataAtual;

            //Logger.Information("Inserting InsertAutorizacaoRecorrencia");
            var autorizacaoRecorrenciaCriada = await _autorizacaoRecorrenciaRepository.InsertAutorizacaoRecorrencia(autorizacaoRecorrencia);

            //if (autorizacaoRecorrenciaCriada != null)
            //{
            //    //Logger.Information("Getting ConsultarAtualizacaoAutorizacaoRecorrencia");
            //    //var consultaAtualizacao = await _autorizacaoRecorrenciaRepository.ConsultarAtualizacaoAutorizacaoRecorrencia(autorizacaoRecorrenciaCriada.IdRecorrencia);
            //    if (consultaAtualizacao == null)
            //    {
            //        //Logger.Information("Inserting InsertAtualizacoesAutorizacaoRecorrencia");
            //        //var consultaAut = await _autorizacaoRecorrenciaRepository.ConsultaAutorizacao(autorizacaoRecorrenciaCriada.IdRecorrencia);
            //        //consultaAut.TipoSituacaoRecorrencia = request.TipoSituacaoRecorrencia;
            //        //VERIFICAR SE É NECESSÁRIO MAIS ALGUMA POPULAÇÃO DE PROPRIEDADE.
            //        //var insercaoAtualizacao = await _autorizacaoRecorrenciaRepository.InsertAtualizacoesAutorizacaoRecorrencia(consultaAut);
            //    }
            //    else
            //    {
            //        return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-008", "Chave já existente na tabela ATUALIZACOES_AUTORIZACOES_RECORRENCIA"));

            //    }
            //}

            return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status200OK, string.Empty, "OK"));

        }

        private AutorizacaoRecorrencia ConverterCommandParaAutorizacaoRecorrencia(InserirAutorizacaoRecorrenciaCommand request)
        {
            if (String.IsNullOrEmpty(request.DataProximoPagamento.ToString()))
                request.DataProximoPagamento = request.DataInicialAutorizacaoRecorrencia;

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
                DataProximoPagamento = request.DataProximoPagamento
            };
        }
    }
}