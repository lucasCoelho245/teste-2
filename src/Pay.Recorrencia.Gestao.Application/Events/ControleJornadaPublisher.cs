using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pay.Recorrencia.Gestao.Domain.Events;

namespace Pay.Recorrencia.Gestao.Application.Events
{
    public class ControleJornadaPublisher : IControleJornadaPublisher
    {
        private readonly ILogger<ControleJornadaPublisher> _logger;

        public ControleJornadaPublisher(ILogger<ControleJornadaPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublicarAsync(ControleJornadaEvent evento)
        {
            // Aqui seria a lógica de envio para fila/evento real (ex: Kafka, Azure, etc).
            // No momento, apenas simula com log.
            _logger.LogInformation("📤 Evento 'Atualizar controle de jornada' enviado com sucesso:");
            _logger.LogInformation(" - tpJornada: {TpJornada}", evento.TpJornada);
            _logger.LogInformation(" - idRecorrencia: {IdRecorrencia}", evento.IdRecorrencia);
            _logger.LogInformation(" - situacaoJornada: {SituacaoJornada}", evento.SituacaoJornada);
            _logger.LogInformation(" - dataUltimaAtualizacao: {DataUltimaAtualizacao}", evento.DataUltimaAtualizacao);

            return Task.CompletedTask;
        }
    }
}
