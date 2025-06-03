using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Pay.Recorrencia.Gestao.Application.Commands.ConsultaAgendamentoCobranca;
using Pay.Recorrencia.Gestao.Application.Commands.ConsultaDetalheDadosCobranca;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Pay.Recorrencia.Gestao.IntegrationTest
{
    public class ConsultaCobrancaTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public ConsultaCobrancaTests(WebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Theory]
        [InlineData("CACC")]
        [InlineData("SLRY")]
        [InlineData("SVGS")]
        [InlineData("TRAN")]
        [InlineData("CAHO")]
        [InlineData("CCTE")]
        [InlineData("DBMO")]
        [InlineData("DBMI")]
        [InlineData("DORD")]
        public async Task ConsultaDetalhesDadosCobranca_EnviandoDadosValidos_ReturnsOk(string idTipoContaPagador)
        {
            var request = new ConsultaDetalheDadosCobrancaCommand
            {
                AgenciaUsuarioPagador = "0001",
                IdTipoContaPagador = idTipoContaPagador,
                ContaUsuarioPagador = "11111",
                IdRecorrencia = "20001223",
                IdOperacao = "234"
            };

            var response = await _httpClient.PostAsJsonAsync("v1/pix-automatico/consulta-cobranca/detalhe", request);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<DetalheDadosCobranca>(responseString);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseString);
        }

        [Theory]
        [InlineData("IdTipoContaPagador", "")]
        [InlineData("ContaUsuarioPagador", "")]
        [InlineData("IdRecorrencia", "")]
        [InlineData("IdOperacao", "")]
        [InlineData("IdTipoContaPagador", null)]
        [InlineData("ContaUsuarioPagador", null)]
        [InlineData("IdRecorrencia", null)]
        [InlineData("IdOperacao", null)]
        public async Task ConsultaDetalhesDadosCobranca_EnviandoDadosInvalidos_ReturnsBadRequest(string campo, string valor)
        {
            var request = new ConsultaDetalheDadosCobrancaCommand
            {
                AgenciaUsuarioPagador = "0001",
                IdTipoContaPagador = "CACC",
                ContaUsuarioPagador = "11111",
                IdRecorrencia = "20001223",
                IdOperacao = "234"
            };
            request.GetType().GetProperty(campo).SetValue(request, valor);

            var response = await _httpClient.PostAsJsonAsync("v1/pix-automatico/consulta-cobranca/detalhe", request);

            //response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<MensagemPadraoResponse>(responseString);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(responseString);
            Assert.Equal("ERRO-PIXAUTO-002", data.Error.Code);
        }

        [Theory]
        [InlineData("CACC")]
        [InlineData("SLRY")]
        [InlineData("SVGS")]
        [InlineData("TRAN")]
        [InlineData("CAHO")]
        [InlineData("CCTE")]
        [InlineData("DBMO")]
        [InlineData("DBMI")]
        [InlineData("DORD")]
        public async Task ConsultaAgendamentoCobranca_EnviandoDadosValidos_ReturnsOk(string idTipoContaPagador)
        {
            // Arrange
            var request = new ConsultaAgendamentoCobrancaCommand
            {
                AgenciaUsuarioPagador = "1234",
                IdTipoContaPagador = idTipoContaPagador,
                ContaUsuarioPagador = "56789",
                NomeUsuarioRecebedor = "João Silva"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("v1/pix-automatico/consulta-cobranca/agendamento", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseContent);

            var agendamentos = JsonConvert.DeserializeObject<List<PixAgendamentoDTO>>(responseContent);
            Assert.NotEmpty(agendamentos);
            Assert.All(agendamentos, agendamento =>
            {
                Assert.NotNull(agendamento.IdOperacao);
                Assert.NotNull(agendamento.IdRecorrencia);
                Assert.NotNull(agendamento.NomeUsuarioRecebedor);
            });
        }

        [Theory]
        [InlineData("IdTipoContaPagador", "")]
        [InlineData("ContaUsuarioPagador", "")]
        public async Task ConsultaAgendamentoCobranca_EnviandoDadosInvalidos_ReturnsBadRequest(string campo, string valor)
        {
            // Arrange
            var request = new ConsultaAgendamentoCobrancaCommand
            {
                AgenciaUsuarioPagador = "1234",
                IdTipoContaPagador = "SLRY",
                ContaUsuarioPagador = "56789",
                NomeUsuarioRecebedor = "João Silva"
            };

            request.GetType().GetProperty(campo).SetValue(request, valor);

            var response = await _httpClient.PostAsJsonAsync("v1/pix-automatico/consulta-cobranca/agendamento", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("ERRO-PIXAUTO-017", responseContent);
        }

        [Fact]
        public async Task ConsultaAgendamentoCobranca_ReturnsNotFound_WhenNoDetailsAreFound()
        {
            // Arrange
            var request = new ConsultaAgendamentoCobrancaCommand
            {
                AgenciaUsuarioPagador = "9999", // Dados que não retornam resultados
                IdTipoContaPagador = "CACC",
                ContaUsuarioPagador = "00000",
                NomeUsuarioRecebedor = "Usuário Inexistente"
            };

            var response = await _httpClient.PostAsJsonAsync("v1/pix-automatico/consulta-cobranca/agendamento", request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
