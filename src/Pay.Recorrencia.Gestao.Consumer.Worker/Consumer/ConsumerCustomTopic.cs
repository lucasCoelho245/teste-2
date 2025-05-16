using Confluent.Kafka;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer.Interface;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Validators;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pay.Recorrencia.Gestao.Consumer.Worker.Consumer
{
    public class ConsumerCustomTopic : IConsumerOperation
    {
        private readonly ILogger<ConsumerCustomTopic> _logger;

        public ConsumerCustomTopic(ILogger<ConsumerCustomTopic> logger)
        {
            _logger = logger;
        }

        public void Consume(string topic, int partition, string message, Headers headers, string topicWithEnvironment, long offSet)
        {
            SolicitacaoRecorrencia? dadosSimulados = ConvertJsonToSolicitacaoRecorrencia(message);

            if (dadosSimulados == null)
            {
                _logger.LogError("Erro: A mensagem JSON não pôde ser convertida para SolicitacaoRecorrencia.");
                return;
            }

            var erros = SolicitacaoRecorrenciaValidator.Validar(dadosSimulados);

            if (erros.Count == 0)
            {
                Console.WriteLine("🎉 Todos os dados estão válidos!");

                // Gerar o payload da PAIN.012
                //var payload = PayloadBuilder.Build(dadosSimulados);

                // Enviar o payload para a API de autorização
                //await EnviarPayloadPain012Async(payload);
            }
            else
            {
                Console.WriteLine("❌ Foram encontrados os seguintes erros:");
                foreach (var erro in erros)
                {
                    Console.WriteLine($" - {erro}");
                }
            }
        }

        public SolicitacaoRecorrencia? ConvertJsonToSolicitacaoRecorrencia(string json)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ignora diferenças de maiúsculas/minúsculas nos nomes das propriedades
                };

                return JsonSerializer.Deserialize<SolicitacaoRecorrencia>(json, options);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Erro ao desserializar JSON: {ex.Message}");
                return null;
            }
        }

        static async Task EnviarPayloadPain012Async(Dictionary<string, object> payload)
        {
            // Usando HttpClientFactory para uma melhor gestão de conexões
            using var httpClient = new HttpClient();

            // URL fictícia - substitua com a URL correta
            var url = "https://proxy.spb.gov.br/api/Entrada/AutorizarRecorrencia";

            try
            {
                // Enviando a requisição com o payload
                var response = await httpClient.PostAsJsonAsync(url, payload);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("✅ Payload PAIN.012 enviado com sucesso!");
                }
                else
                {
                    Console.WriteLine($"❌ Falha ao enviar PAIN.012: {response.StatusCode}");
                    var respostaErro = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"🔍 Detalhes: {respostaErro}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Captura erro de rede ou falha ao acessar a URL
                Console.WriteLine($"🚨 Erro ao enviar a requisição PAIN.012: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Captura outras exceções gerais
                Console.WriteLine($"🚨 Erro inesperado: {ex.Message}");
            }
        }
    }
}
