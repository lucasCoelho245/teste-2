using Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.PayPagamentoProcessado.Validation.IValidation;
using Pay.Recorrencia.Gestao.Domain.Entities;
using System.Net.Http.Json;

namespace Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.PayPagamentoProcessado.Validation
{
    public class ValidadorExistenciaRecorrencia(ILogger<ValidadorExistenciaRecorrencia> logger) : IValidacaoSolicitacao
    {
        private readonly ILogger<ValidadorExistenciaRecorrencia> _logger = logger;

        public string Validar(SolicitacaoRecorrenciaEntrada dados)
        {
            return ValidarExistenciaRecorrencia(dados.IdSolicRecorrencia, dados.IdRecorrencia);
        }

        private string ValidarExistenciaRecorrencia(string idSolicRecorrencia, string idRecorrencia)
        {
            var resultadoHist009 = ConsultarHist009Async(idSolicRecorrencia).GetAwaiter().GetResult();

            if (resultadoHist009 != null && (resultadoHist009.Situacao == "PDNG" || resultadoHist009.Situacao == "APRV"))
            {
                _logger.LogError("{IdRecorrencia} - A solicitação de recorrência não pode ser processada, pois a situação é PDNG ou APRV.", idRecorrencia);
                return "AM05";
            }

            var resultadoHist038 = ConsultarHist038Async(idRecorrencia).GetAwaiter().GetResult();

            if (resultadoHist038 != null && (resultadoHist038.Situacao == "PDNG" || resultadoHist038.Situacao == "INPR" || resultadoHist038.Situacao == "CFDB"))
            {
                _logger.LogError("{IdRecorrencia} - A solicitação de recorrência não pode ser processada, pois a situação é PDNG, INPR ou CFDB.", idRecorrencia);
                return "AM05";
            }

            return string.Empty;
        }
        
        private async Task<DadosHist009?> ConsultarHist009Async(string idSolicitacao)
        {
            try
            {
                using var httpClient = new HttpClient();
                var url = $"https://url.pix.gov.br/hist-009/{idSolicitacao}";   // Substitua pela URL correta da API
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[Hist009] ❌ Erro: {response.StatusCode}");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<DadosHist009>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Hist009] 🚨 Exceção: {ex.Message}");
                return null;
            }
        }

        private async Task<DadosHist038?> ConsultarHist038Async(string idRecorrencia)
        {
            try
            {
                using var httpClient = new HttpClient();
                var url = $"https://url.pix.gov.br/hist-038/{idRecorrencia}"; // URL fictícia, substitua pela URL real
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[Hist038] ❌ Erro: {response.StatusCode}");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<DadosHist038>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Hist038] 🚨 Exceção: {ex.Message}");
                return null;
            }
        }
    }
}
