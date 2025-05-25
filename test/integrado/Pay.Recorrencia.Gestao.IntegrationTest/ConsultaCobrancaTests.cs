using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Pay.Recorrencia.Gestao.Application.Commands.ConsultaDetalheDadosCobranca;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;
using System.Net.Http;
using System.Net.Http.Json;
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

            var response = await _httpClient.PostAsJsonAsync("/ConsultaCobranca", request);

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

            var response = await _httpClient.PostAsJsonAsync("/ConsultaCobranca", request);

            //response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<MensagemPadraoResponse>(responseString);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(responseString);
            Assert.Equal("ERRO-PIXAUTO-017", data.Error.Code);
        }
    }
}
