using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.DTO.ObterDadosUsuario;
using Pay.Recorrencia.Gestao.Domain.Enums;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using Pay.Recorrencia.Gestao.Domain.Services;

namespace Pay.Recorrencia.Gestao.Application.Commands.ConfirmacaoAutorizacaoRecorr
{
    public sealed class ConfirmacaoAutorizacaoRecorrHandler 
        : IRequestHandler<ReceberConfirmacaoAutorizacaoRecorrCommand, MensagemPadraoResponse>
        , IRequestHandler<ValidarConfirmacaoCommand, MensagemPadraoResponse>
    {
        private readonly IHttpClient _httpClient;
        private readonly IPushService _pushService;
        private readonly IAutorizacaoRecorrenciaRepository _recorrenciaRepository;
        private readonly IMapper _mapper;
        private readonly string _urlAutorizacao;
        private readonly string _urlDadosUsuario;
        private readonly bool _flagMockPush;
        public ConfirmacaoAutorizacaoRecorrHandler(IHttpClient httpClient, IPushService pushClient, IAutorizacaoRecorrenciaRepository recorrenciaRepository, IConfiguration configuration, IMapper mapper)
        {
            _httpClient = httpClient;
            _pushService = pushClient;
            _recorrenciaRepository = recorrenciaRepository;
            _mapper = mapper;
            _urlAutorizacao = configuration.GetValue<string>("EndpointAutorizacaoRecorrencia") ?? string.Empty;
            _urlDadosUsuario = configuration.GetValue<string>("EndpointDadosUsuario") ?? string.Empty;
            _flagMockPush = configuration.GetValue<bool>("FlagMockPush");
        }

        public async Task<MensagemPadraoResponse> Handle(ReceberConfirmacaoAutorizacaoRecorrCommand request, CancellationToken cancellationToken)
        {
            var retorno = new MensagemPadraoResponse(StatusCodes.Status200OK, string.Empty, string.Empty);
            var textoPush = string.Empty;
            var requisicaoUpdate = new AlterarAutorizacaoCommand();

            var dadosAutorizacao = await _recorrenciaRepository.ConsultaAutorizacao(request.IdRecorrencia);

            if (dadosAutorizacao == null)
                throw new Exception("Autorização não encontrada.");

            requisicaoUpdate.IdAutorizacao = dadosAutorizacao.IdAutorizacao;
            requisicaoUpdate.IdRecorrencia = request.IdRecorrencia.ToString();
            requisicaoUpdate.SituacaoRecorrencia = request.SituacaoRecorrencia;
            requisicaoUpdate.DataHoraSituacaoRecorrencia = DateTime.Now;

            if (request.Status.ToUpper() == true.ToString().ToUpper())
            {
                requisicaoUpdate.TipoSituacaoRecorrencia = SituacaoRecorrencia.CFDB.ToString();

                textoPush = $"A autorização do Pix Automático em processamento para {request.NomeUsuarioRecebedor} foi confirmada. " +
                    $"Acesse [Pix Automático -> Autorizações ativas] para consultar os dados da autorização.";
            }
            else
            {
                requisicaoUpdate.TipoSituacaoRecorrencia = SituacaoRecorrencia.CCLD.ToString();
                requisicaoUpdate.SituacaoRecorrencia = SituacaoRecorrencia.CCLD.ToString();

                textoPush = $"{request.NomeUsuarioRecebedor} cancelou a solicitação de autorização do Pix Automático pendente de confirmação, devido a erro nos dados da recorrência.";
            }

            // ATUALIZAR DADOS
            var retornoAtualizarAutorizacao = await _httpClient.ExecutarRequisicaoAsync(_urlAutorizacao, HttpMethod.Put, requisicaoUpdate);
            if (retornoAtualizarAutorizacao == null || retornoAtualizarAutorizacao.StatusCode != HttpStatusCode.OK)
                throw new Exception("Não foi possível atualizar a situação da solicitação de recorrência.");


            //ENVIAR PUSH
            if (_flagMockPush)
            {
                Console.WriteLine($"Notificação push enviada para o usuario. Texto: {textoPush}");
            }
            else
            {
                try
                {
                    var cpf = new string(request.CpfCnpjUsuarioRecebedor.Where(char.IsDigit).ToArray());
                    var retornoObterRecebedor = await _httpClient.ExecutarRequisicaoAsync(_urlDadosUsuario, HttpMethod.Get, Convert.ToInt64(cpf));

                    if (retornoObterRecebedor == null
                        || string.IsNullOrEmpty(retornoObterRecebedor.Content)
                        || retornoObterRecebedor.StatusCode != HttpStatusCode.OK)
                        throw new Exception("Não foi possível obter os dados do usuário.");

                    var dadosUsuario = JsonSerializer.Deserialize<DadosUsuarioDTO>(retornoObterRecebedor.Content);

                    if (dadosUsuario == null || await _pushService.EnviarPush("Pix automático", textoPush, new string[1] { dadosUsuario.id }) == false)
                        throw new Exception("Não foi possível enviar notificação ao usuário.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao enviar notificação push. " + ex.Message, ex);
                }
            }
            
            return retorno;
        }

        public async Task<MensagemPadraoResponse> Handle(ValidarConfirmacaoCommand request, CancellationToken cancellationToken)
        {
            var retorno = new MensagemPadraoResponse(StatusCodes.Status200OK, string.Empty, string.Empty);
            var flgValido = true;
            var erros = new List<string>();
            var resultadoValidacao = new List<ValidationResult>();
            var requestConfirmacao = request.GetRequestConfirmacao();
            var contexto = new ValidationContext(requestConfirmacao, null, null);

            if (Validator.TryValidateObject(requestConfirmacao, contexto, resultadoValidacao, true) == false)
            {
                flgValido = false; 
                erros.Add(string.Join(", ", resultadoValidacao.Select(x => x.ErrorMessage)) + ".");
            }

            if (bool.TryParse(requestConfirmacao.Status, out bool status) == false)
            {
                flgValido = false;
                erros.Add("Valor inválido no campo \'status\'.");
            }

            if (status)
            {
                if (Enum.TryParse<SituacaoRecorrencia>(requestConfirmacao.SituacaoRecorrencia, true, out SituacaoRecorrencia result) == false)
                {
                    flgValido = false;
                    erros.Add("Valor inválido no campo \'situacaoOcorrencia\'");
                }
            }

            if (Int32.TryParse(requestConfirmacao.TpJornada, null, out int tpJornadaInt) == false)
            {
                flgValido = false; 
                erros.Add("Valor inválido no campo \'tpJornada\'");
            }
            else
            {
                if (tpJornadaInt < 1 || tpJornadaInt > 4)
                {
                    flgValido = false;
                    erros.Add("Valor inválido no campo \'tpJornada\'");
                }
            }

            if (flgValido == false)
            {
                retorno.StatusCode = StatusCodes.Status400BadRequest;
                retorno.Error.Message = string.Join(", ", erros);
            }
                 
            return await Task.FromResult(retorno);
        }
    }
}
