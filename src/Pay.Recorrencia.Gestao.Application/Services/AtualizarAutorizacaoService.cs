using Microsoft.AspNetCore.Http;
using Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Interfaces;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Services
{
    public class AtualizarAutorizacaoService : IAtualizarAutorizacaoService
    {
        private IAutorizacaoRecorrenciaRepository _autorizacaoRecorrenciaRepository { get; }
        //public UpdateAtualizacoesHandler(ILogger logger, IAutorizacaoRecorrenciaRepository atualizacoesRepository)

        public AtualizarAutorizacaoService(IAutorizacaoRecorrenciaRepository autorizacaoRecorrenciaRepository)
        {
            //Logger = logger;
            _autorizacaoRecorrenciaRepository = autorizacaoRecorrenciaRepository;
        }


        public async Task<MensagemPadraoResponse> Handle(AlterarAutorizacaoCommand request)
        {
            if (!ValidaRequest(request))
            {
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-002", "Campos não preenchidos corretamente"));
                //throw new ArgumentException("ERRO-PIXAUTO-003");
            }

            AutorizacaoRecorrencia autorizacaoEncontrada = await _autorizacaoRecorrenciaRepository.ConsultaAutorizacao(request.IdAutorizacao, request.IdRecorrencia);
            
            if (autorizacaoEncontrada is null)
            {
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-009", "Chave não encontrada na tabela AUTORIZACAO_RECORRENCIA"));
                //throw new Exception("ERRO-PIXAUTO-009");
            }

            // var atualizacao = _autorizacaoRecorrenciaRepository.ConsultarAtualizacaoAutorizacaoRecorrencia(request.IdAutorizacao, request.IdRecorrencia).Result;

            //if (atualizacao == null)
            //{
            //    return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-010", "Chave não encontrada na tabela ATUALIZACOES_AUTORIZACOES_RECORRENCIA"));
            //    //throw new Exception("ERRO-PIXAUTO-010");
            //}

            DateTime dataHoraAtual = DateTime.Now;
            
            AtualizaCamposAutorizacaoRecorrencia(autorizacaoEncontrada, request, dataHoraAtual);

            InsereAtualizacaoAutorizacaoRecorrencia(autorizacaoEncontrada, request, dataHoraAtual);

            return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status200OK, string.Empty, "OK"));
        }

        private bool ValidaRequest(AlterarAutorizacaoCommand request)
        {

            if (!ValidaCamposObrigatorios(request))
            {
                return false;
            }
            if (!ValidarCamposComDominioCorreto(request))
            {
                return false;
            }

            return true;
        }

        private bool ValidaCamposObrigatorios(AlterarAutorizacaoCommand request)
        {

            DateTime? dataInicialAutorizacaoRecorrencia = request.DataInicialAutorizacaoRecorrencia;

            if (dataInicialAutorizacaoRecorrencia != null && !dataInicialAutorizacaoRecorrencia.HasValue)
            {
                return false;
            }

            if (request.IdAutorizacao == null && request.IdRecorrencia == null)
            {
                return false;
            }

            return true;
        }

        private static bool ValidarCamposComDominioCorreto(AlterarAutorizacaoCommand request)
        {
            // string[] dominioSituacaoRecorrencia = { "PDRC", "PRRC", "RCSD", "PDCF", "LIDO", "PDPG", "CFPG", "ERPG", "PRCF", "CFDB", "ERCF", "CCLD" };
            string[] dominioSituacaoRecorrencia = { "INAC", "INRJ", "PDNG", "RJCT", "EXPR", "INRC", "RCSD", "INAP", "APRV", "SPND", "CCLD" };
            string[] dominioTipoRecorrencia = { "RCUR" };
            string[] dominioTipoFrequencia = { "MIAN", "MNTH", "QURT", "WEEK", "YEAR" };
            string[] dominioCodigoMoedaAutorizacaoRecorrencia = { "BRL" };
            string[] dominioMotivoRejeicaoRecorrencia = { "AC01", "AC04", "AC06", "AG12", "AM05", "AP01", "AP02", "AP03", "AP04", "AP05",
                "AP06", "AP07", "AP08", "AP09", "AP10", "AP11", "AP12", "AP13", "AP14", "AP15",
                "CH16", "DS27", "MD01", "MD20", "RC09", "RC10"};
            string[] dominioCodigoSituacaoCancelamentoRecorrencia = { "ACCL", "CPCL", "DCSD", "ERSL", "FRUD", "NRES", "PCFD", "SLCR", "SLDB" };
            string[] dominioTipoSituacaoRecorrencia = { "READ", "CRTN", "AUT1", "AUT2", "AUT3", "AUT4", "CFDB", "CCLD" };
            string[] dominioTipoRetentativa = { "NAO_PERMITE", "PERMITE_3R_7D" };

            if (!String.IsNullOrEmpty(request.SituacaoRecorrencia) && !dominioSituacaoRecorrencia.Contains(request.SituacaoRecorrencia.ToUpper()))
            {
                return false;
            }

            if (!String.IsNullOrEmpty(request.TipoRecorrencia) && !dominioTipoRecorrencia.Contains(request.TipoRecorrencia.ToUpper()))
            {
                return false;
            }

            if (!String.IsNullOrEmpty(request.TipoFrequencia) && !dominioTipoFrequencia.Contains(request.TipoFrequencia.ToUpper()))
            {
                return false;
            }

            if (!String.IsNullOrEmpty(request.CodigoMoedaAutorizacaoRecorrencia) && !dominioCodigoMoedaAutorizacaoRecorrencia.Contains(request.CodigoMoedaAutorizacaoRecorrencia.ToUpper()))
            {
                return false;
            }

            if (!String.IsNullOrEmpty(request.MotivoRejeicaoRecorrencia) && !dominioMotivoRejeicaoRecorrencia.Contains(request.MotivoRejeicaoRecorrencia.ToUpper()))
            {
                return false;
            }

            if (!String.IsNullOrEmpty(request.CodigoSituacaoCancelamentoRecorrencia) && !dominioCodigoSituacaoCancelamentoRecorrencia.Contains(request.CodigoSituacaoCancelamentoRecorrencia.ToUpper()))
            {
                return false;
            }

            if (!String.IsNullOrEmpty(request.TipoSituacaoRecorrencia) && !dominioTipoSituacaoRecorrencia.Contains(request.TipoSituacaoRecorrencia.ToUpper()))
            {
                return false;
            }

            if (!String.IsNullOrEmpty(request.TpRetentativa) && !dominioTipoRetentativa.Contains(request.TpRetentativa.ToUpper()))
            {
                return false;
            }

            return true;
        }

        private void AtualizaCamposAutorizacaoRecorrencia(AutorizacaoRecorrencia autorizacaoEncontrada, AlterarAutorizacaoCommand request, DateTime dataHoraAtual)
        {
            autorizacaoEncontrada.ValorMaximoAutorizado = request.ValorMaximoAutorizado ?? autorizacaoEncontrada.ValorMaximoAutorizado;
            autorizacaoEncontrada.FlagValorMaximoAutorizado = request.FlagValorMaximoAutorizado ?? autorizacaoEncontrada.FlagValorMaximoAutorizado;
            autorizacaoEncontrada.FlagPermiteNotificacao = request.FlagPermiteNotificacao ?? autorizacaoEncontrada.FlagPermiteNotificacao;
            autorizacaoEncontrada.CodMunIBGE = request.CodMuniIBGE ?? autorizacaoEncontrada.CodMunIBGE;
            autorizacaoEncontrada.DataHoraCriacaoRecorr = request.DataHoraCriacaoRecorr ?? autorizacaoEncontrada.DataHoraCriacaoRecorr;
            autorizacaoEncontrada.SituacaoRecorrencia = request.SituacaoRecorrencia ?? autorizacaoEncontrada.SituacaoRecorrencia;
            autorizacaoEncontrada.MotivoRejeicaoRecorrencia = request.MotivoRejeicaoRecorrencia ?? autorizacaoEncontrada.MotivoRejeicaoRecorrencia;
            autorizacaoEncontrada.CodigoSituacaoCancelamentoRecorrencia = request.CodigoSituacaoCancelamentoRecorrencia ?? autorizacaoEncontrada.CodigoSituacaoCancelamentoRecorrencia;
            autorizacaoEncontrada.DataUltimaAtualizacao = dataHoraAtual;
            autorizacaoEncontrada.DataProximoPagamento = request.DataProximoPagamento ?? autorizacaoEncontrada.DataProximoPagamento;

             _autorizacaoRecorrenciaRepository.Update(autorizacaoEncontrada);
        }

        private async void InsereAtualizacaoAutorizacaoRecorrencia(AutorizacaoRecorrencia autorizacaoEncontrada, AlterarAutorizacaoCommand request, DateTime dataHoraAtual)
        {
            AtualizacaoAutorizacaoRecorrencia atualizacaoAutorizacao = new()
            {
                IdAutorizacao = request.IdAutorizacao,
                IdRecorrencia = autorizacaoEncontrada.IdRecorrencia,
                TipoSituacaoRecorrencia = request.TipoSituacaoRecorrencia,
                DataHoraSituacaoRecorrencia = request.DataHoraSituacaoRecorrencia.HasValue ? request.DataHoraSituacaoRecorrencia : DateTime.Now,
                DataUltimaAtualizacao = dataHoraAtual
            };

            await _autorizacaoRecorrenciaRepository.InsertAtualizacoesAutorizacaoRecorrencia(atualizacaoAutorizacao);
        }
    }
}
