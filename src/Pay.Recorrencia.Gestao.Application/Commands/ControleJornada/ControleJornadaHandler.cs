using AutoMapper;
using Confluent.Kafka;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using Pay.Recorrencia.Gestao.Domain.Services;
using System.Text;
using System.Text.Json;

namespace Pay.Recorrencia.Gestao.Application.Commands.ControleJornada
{
    public class ControleJornadaHandler : IRequestHandler<IncluirControleJornadaCommand, MensagemPadraoResponse>,
                                            IRequestHandler<AtualizarControleJornadaCommand, MensagemPadraoResponse>
    {
        private readonly IControleJornadaRepository _controleJornadaRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IKafkaProducerService _kafkaProducerService;

        private readonly bool _enviarEventoTopico;

        private readonly string[] tiposJornada_ParametroIdRecorrencia = { "Jornada 1", "Jornada 2", "Jornada 3", "Jornada 4" };
        private readonly string[] tiposJornada_ParametroIdE2E = { "AGND", "NTAG", "RIFL" };

        public ControleJornadaHandler(IControleJornadaRepository controleJornadaRepository, IMapper mapper, IConfiguration configuration, IKafkaProducerService kafkaProducerService)
        {
            _controleJornadaRepository = controleJornadaRepository;
            _mapper = mapper;
            _configuration = configuration;
            _kafkaProducerService = kafkaProducerService;

            _enviarEventoTopico = _configuration.GetValue<bool>("FlagEnviarEventosCrudJornadas", false);
        }

        public async Task<MensagemPadraoResponse> Handle(IncluirControleJornadaCommand request, CancellationToken cancellationToken)
        {
            var topics = _configuration.GetSection("Kafka:Producer:TopicList");
            var topicIncluir = topics.GetSection("IncluirControleJornada").Value ?? string.Empty;
            var topicIncluirDLQ = topics.GetSection("IncluirControleJornadaDLQ").Value ?? string.Empty;

            var entity = _mapper.Map<Domain.Entities.ControleJornada>(request);

            DateTime dataHoraAutal = DateTime.Now;
            entity.DataHoraCriacao = dataHoraAutal;

            try
            {
                string errosValidacao = ValidaCamposInclusao(request);

                if (errosValidacao.Length <= 0)
                {
                    (string idRecorrencia, string idE2E) = ObterValoresConsulta(request);

                    var entidadeAlterar = await _controleJornadaRepository.GetControle(request.TpJornada ?? string.Empty, idRecorrencia, idE2E);

                    if (entidadeAlterar.Count > 0)
                    {
                        await PostarMensagemTopico(JsonSerializer.Serialize(request), topicIncluirDLQ);

                        return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "", "Objeto já existe na tabela de controle de jornada"));
                    }
                    else
                    {
                        await _controleJornadaRepository.IncluirControle(entity);

                        await PostarMensagemTopico(JsonSerializer.Serialize(request), topicIncluir);

                        return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status200OK, "", ""));
                    } 
                }
                else
                {
                    await PostarMensagemTopico(JsonSerializer.Serialize(request), topicIncluirDLQ);

                    return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "", errosValidacao));
                }
            }
            catch (Exception)
            {
                await PostarMensagemTopico(JsonSerializer.Serialize(request), topicIncluirDLQ);

                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "", "Erro na inclusão da jornada"));
            }
        }

        

        public async Task<MensagemPadraoResponse> Handle(AtualizarControleJornadaCommand request, CancellationToken cancellationToken)
        {
            var topics = _configuration.GetSection("Kafka:Producer:TopicList");
            var topicAlterar = topics.GetSection("AlterarControleJornada").Value ?? string.Empty;
            var topicAlterarDLQ = topics.GetSection("AlterarControleJornadaDLQ").Value ?? string.Empty;

            var entity = _mapper.Map<Domain.Entities.ControleJornada>(request);

            try
            {
                string errosValidacao = ValidaCamposAlteracao(request);

                if (errosValidacao.Length <= 0)
                {
                    var entidadeAlterar = await _controleJornadaRepository.GetControle(request.TpJornada, request.IdRecorrencia, request.IdE2E);

                    if (entidadeAlterar.Count == 1)
                    {
                        await _controleJornadaRepository.AtualizarControle(entity);

                        await PostarMensagemTopico(JsonSerializer.Serialize(request), topicAlterar);

                        return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status200OK, string.Empty, string.Empty));
                    }
                    else
                    {
                        await PostarMensagemTopico(JsonSerializer.Serialize(request), topicAlterarDLQ);

                        return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-020", "Controle de jornada não encontrada."));
                    }
                }
                else
                {
                    await PostarMensagemTopico(JsonSerializer.Serialize(request), topicAlterarDLQ);

                    return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "", errosValidacao));
                }
            }
            catch (Exception)
            {
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "", "Erro na atualização da jornada"));
            }
        }

        private string ValidaCamposAlteracao(AtualizarControleJornadaCommand request)
        {
            StringBuilder sb = new StringBuilder();

            if (ValidarTipoJornadaObrigatorio(request.TpJornada))
            {
                AdicionarTexto(sb, "Tipo de jornada é requerido");
            }
            else
            {
                if (ValidarTipoJornadaCorreta(request.TpJornada))
                {
                    AdicionarTexto(sb, "Tipo de jornada informada está incorreta");
                }
            }

            return sb.ToString();
        }

        private string ValidaCamposInclusao(IncluirControleJornadaCommand request)
        {

            StringBuilder sb = new StringBuilder();

            if (ValidarTipoJornadaObrigatorio(request.TpJornada))
            {
                AdicionarTexto(sb, "Tipo de jornada é requerido");
            }
            else
            {
                if (ValidarTipoJornadaCorreta(request.TpJornada))
                {
                    AdicionarTexto(sb, "Tipo de jornada informada está incorreta");
                }
            }

            if (ValidarIdRecorrenciaObrigatorio(request.IdRecorrencia))
            {
                AdicionarTexto(sb, "Identificador da recorrência é requerido");
            }

            if (ValidarIdFimAFimObrigatorio(request.TpJornada, request.IdE2E))
            {
                AdicionarTexto(sb, "ID Fim a Fim é requerido");
            }

            if (ValidarIdConciliacaoObrigatorio(request.TpJornada, request.IdConciliacaoRecebedor))
            {
                AdicionarTexto(sb, "ID de Conciliação é requerido");
            }

            return sb.ToString();
        }

        private static bool ValidarTipoJornadaObrigatorio(string tpJornada)
        {
            return tpJornada.IsNullOrEmpty();
        }

        private bool ValidarTipoJornadaCorreta(string tpJornada)
        {
            return !tiposJornada_ParametroIdE2E.Contains(tpJornada, StringComparer.InvariantCultureIgnoreCase) &&
                                !tiposJornada_ParametroIdRecorrencia.Contains(tpJornada, StringComparer.InvariantCultureIgnoreCase);
        }

        private static bool ValidarIdRecorrenciaObrigatorio(string idRecorrencia)
        {
            return idRecorrencia.IsNullOrEmpty();
        }

        private bool ValidarIdFimAFimObrigatorio(string tpJornada, string idE2E)
        {
            return (new string[] { "Jornada 3", "Jornada 4" }.Contains(tpJornada, StringComparer.InvariantCultureIgnoreCase) ||
                             tiposJornada_ParametroIdE2E.Contains(tpJornada, StringComparer.InvariantCultureIgnoreCase)) &&
                             idE2E.IsNullOrEmpty();
        }

        private bool ValidarIdConciliacaoObrigatorio(string tpJornada, string idConciliacaoRecebedor)
        {
            return tiposJornada_ParametroIdE2E.Contains(tpJornada, StringComparer.InvariantCultureIgnoreCase) &&
                            idConciliacaoRecebedor.IsNullOrEmpty();
        }

        private void AdicionarTexto(StringBuilder sb, string texto)
        {
            string sep = sb.Length > 0 ? " | " : "";
            sb.Append(sep + texto);
        }

        private (string, string) ObterValoresConsulta(IncluirControleJornadaCommand request)
        {
            string idRecorrencia = tiposJornada_ParametroIdRecorrencia.Contains(request.TpJornada) ? request.IdRecorrencia : string.Empty;
            string idE2E = tiposJornada_ParametroIdE2E.Contains(request.TpJornada) ? request.IdE2E : string.Empty;

            return (idRecorrencia, idE2E);
        }

        private async Task PostarMensagemTopico(string mensagem, string nomeTopico)
        {
            if (_enviarEventoTopico && !string.IsNullOrEmpty(nomeTopico))
                await _kafkaProducerService.SendMessageWithParameterAsync(nomeTopico, mensagem);
        }
    }
}
