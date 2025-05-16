using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pay.Recorrencia.Gestao.Domain.Services;
using Pay.Recorrencia.Gestao.Producer.Models;

namespace Pay.Recorrencia.Gestao.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KafkaMessageController : ControllerBase
    {
        private readonly ILogger<KafkaMessageController> _logger;
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly InputParametersKafkaProducer _kafkaSettings;

        public KafkaMessageController(ILogger<KafkaMessageController> logger, IKafkaProducerService kafkaProducerService, IOptions<InputParametersKafkaProducer> kafkaSettings)
        {
            _logger = logger;
            _kafkaProducerService = kafkaProducerService;
            _kafkaSettings = kafkaSettings.Value;
        }

        [HttpPost(Name = "send-message")]
        public async Task<IActionResult> SendMessage(string message)
        {
            try
            {
                await _kafkaProducerService.SendMessageAsync(_kafkaSettings.Producer.Topic, message);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar mensagem");
                return StatusCode(500);
            }
        }
    }
}
