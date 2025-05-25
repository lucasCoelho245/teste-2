using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Pay.Recorrencia.Gestao.test.integrado.IntegrationTest
{
    public class SolicAutorizacaoRecControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SolicAutorizacaoRecControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetSolicitacoes_RetornaOK_ComDadosValidos()
        {
            var queryParams = "cpfCnpjUsuarioPagador=98765432100&contaUsuarioPagador=1234";
            var response = await _client.GetAsync($"/v1.0/pix-automatico/solicitacao-autorizacao-recorrencia?{queryParams}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<ApiMetaDataPaginatedResponse>(responseString);

            Assert.Equal("OK", jsonData.Status);
            Assert.True(jsonData.Data.Items.Any());
        }
        [Fact]
        public async Task GetSolicitacoes_Checa_SituacaoInformada()
        {
            var queryParams = "cpfCnpjUsuarioPagador=98765432100&contaUsuarioPagador=1234&situacaoSolicRecorrencia=PDNG";
            var response = await _client.GetAsync($"/v1.0/pix-automatico/solicitacao-autorizacao-recorrencia?{queryParams}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<ApiMetaDataPaginatedResponse>(responseString);

            Assert.Equal("OK", jsonData.Status);
            Assert.True(jsonData.Data.Items.Any());

            foreach (var obj in jsonData.Data.Items)
            {
                SolicAutorizacaoRecList item = obj.ToObject<SolicAutorizacaoRecList>();
                Assert.Equal("PDNG", item.SituacaoSolicRecorrencia);
            }

        }

        [Fact]
        public async Task GetSolicitacoes_Checa_NomeInformado()
        {
            var queryParams = "cpfCnpjUsuarioPagador=98765432100&contaUsuarioPagador=1234&situacaoSolicRecorrencia=PDNG&nomeUsuarioRecebedor=";
            var response = await _client.GetAsync($"/v1.0/pix-automatico/solicitacao-autorizacao-recorrencia?{queryParams}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<ApiMetaDataPaginatedResponse>(responseString);

            Assert.Equal("OK", jsonData.Status);
            Assert.True(jsonData.Data.Items.Any());

            foreach (var obj in jsonData.Data.Items)
            {
                SolicAutorizacaoRecList item = obj.ToObject<SolicAutorizacaoRecList>();
                Assert.True(Regex.IsMatch(item.NomeUsuarioRecebedor, Regex.Escape("Silva"), RegexOptions.IgnoreCase));
            }

        }
        [Fact]
        public async Task GetSolicitacoes_Checa_TodosDadosInformados()
        {
            var queryParams = "cpfCnpjUsuarioPagador=98765432100&contaUsuarioPagador=1234&situacaoSolicRecorrencia=PDNG&nomeUsuarioRecebedor=";
            var response = await _client.GetAsync($"/v1.0/pix-automatico/solicitacao-autorizacao-recorrencia?{queryParams}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<ApiMetaDataPaginatedResponse>(responseString);

            Assert.Equal("OK", jsonData.Status);
            Assert.True(jsonData.Data.Items.Any());

            foreach (var obj in jsonData.Data.Items)
            {
                SolicAutorizacaoRecList item = obj.ToObject<SolicAutorizacaoRecList>();
                Assert.True(Regex.IsMatch(item.NomeUsuarioRecebedor, Regex.Escape("Silva"), RegexOptions.IgnoreCase));
                Assert.Equal("PDNG", item.SituacaoSolicRecorrencia);
            }
        }
        [Fact]
        public async Task GetSolicitacoes_RetornaOK_QuandoPaginado()
        {
            var queryParams = "cpfCnpjUsuarioPagador=98765432100&contaUsuarioPagador=1234&agenciaUsuarioPagador=&situacaoSolicRecorrencia=PDNG&nomeUsuarioRecebedor=&pageSize=1";
            var response = await _client.GetAsync($"/v1.0/pix-automatico/solicitacao-autorizacao-recorrencia?{queryParams}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<ApiMetaDataPaginatedResponse>(responseString);

            Assert.Equal("OK", jsonData.Status);
            Assert.Single(jsonData.Data.Items);
        }

        [Fact]
        public async Task GetSolicitacoes_RetornaOK_DadosInvalidos()
        {
            var requestBody = new GetListaSolicAutorizacaoRecDTO
            {
                CpfCnpjUsuarioPagador = "12345",
                SituacaoSolicRecorrencia = "",
                NomeUsuarioRecebedor = "",
                ContaUsuarioPagador = "12134"
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/v1.0/pix-automatico/solicitacao-autorizacao-recorrencia", requestContent);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetDetalheSolicitacao_RetornaOK_CodigoValido()
        {
            string idSolicitacao = "REC01";
            var response = await _client.GetAsync($"/v1.0/pix-automatico/solicitacao-autorizacao-recorrencia/{idSolicitacao}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<ApiMetaDataNonPaginatedResponse<SolicitacaoRecorrencia>>(responseString);

            Assert.Equal("OK", jsonData.Status);
            Assert.Equal(idSolicitacao, jsonData.Data.IdSolicRecorrencia);
        }

        [Fact]
        public async Task GetDetalheSolicitacao_RetornaErro_CodigoInValido()
        {
            string idSolicitacao = "12345";
            var response = await _client.GetAsync($"/v1.0/pix-automatico/solicitacao-autorizacao-recorrencia/{idSolicitacao}");

            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<ApiMetaDataNonPaginatedResponse<SolicitacaoRecorrencia>>(responseString);

            Assert.Null(jsonData.Data);
            Assert.Equal(HttpStatusCode.NotFound.ToString(), jsonData.Status);
        }
    }
}
