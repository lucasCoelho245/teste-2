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
            var requestBody = new GetListaSolicAutorizacaoRecDTO
            {
                CpfCnpjUsuarioPagador = "98765432100",
                SituacaoSolicRecorrencia = "",
                NomeUsuarioRecebedor = "",
                ContaUsuarioPagador = "12134"
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/v1.0/pix-automatico/solicitacoes", requestContent);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<ApiMetaDataPaginatedResponse>(responseString);

            Assert.True(jsonData.Status);
            Assert.True(jsonData.Data.Items.Any());
        }
        [Fact]
        public async Task GetSolicitacoes_Checa_SituacaoInformada()
        {
            var requestBody = new GetListaSolicAutorizacaoRecDTO
            {
                CpfCnpjUsuarioPagador = "98765432100",
                SituacaoSolicRecorrencia = "CCLD",
                NomeUsuarioRecebedor = "",
                ContaUsuarioPagador = "12134"
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/v1.0/pix-automatico/solicitacoes", requestContent);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<ApiMetaDataPaginatedResponse>(responseString);

            Assert.True(jsonData.Status);
            Assert.True(jsonData.Data.Items.Any());

            foreach (var obj in jsonData.Data.Items)
            {
                SolicAutorizacaoRecList item = obj.ToObject<SolicAutorizacaoRecList>();
                Assert.True(item.SituacaoSolicRecorrencia == requestBody.SituacaoSolicRecorrencia);
            }
        }
        [Fact]
        public async Task GetSolicitacoes_Checa_NomeInformado()
        {
            var requestBody = new GetListaSolicAutorizacaoRecDTO
            {
                CpfCnpjUsuarioPagador = "98765432100",
                SituacaoSolicRecorrencia = "",
                NomeUsuarioRecebedor = "Silva",
                ContaUsuarioPagador = "12134"
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/v1.0/pix-automatico/solicitacoes", requestContent);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<ApiMetaDataPaginatedResponse>(responseString);

            Assert.True(jsonData.Status);
            Assert.True(jsonData.Data.Items.Any());

            foreach (var obj in jsonData.Data.Items)
            {
                SolicAutorizacaoRecList item = obj.ToObject<SolicAutorizacaoRecList>();
                Assert.True(Regex.IsMatch(item.NomeUsuarioRecebedor, Regex.Escape(requestBody.NomeUsuarioRecebedor), RegexOptions.IgnoreCase));
            }
        }
        [Fact]
        public async Task GetSolicitacoes_Checa_TodosDadosInformados()
        {
            var requestBody = new GetListaSolicAutorizacaoRecDTO
            {
                CpfCnpjUsuarioPagador = "98765432100",
                SituacaoSolicRecorrencia = "CCLD",
                NomeUsuarioRecebedor = "Silva",
                ContaUsuarioPagador = "12134"
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/v1.0/pix-automatico/solicitacoes", requestContent);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<ApiMetaDataPaginatedResponse>(responseString);

            Assert.True(jsonData.Status);
            Assert.True(jsonData.Data.Items.Any());

            foreach (var obj in jsonData.Data.Items)
            {
                SolicAutorizacaoRecList item = obj.ToObject<SolicAutorizacaoRecList>();
                Assert.True(Regex.IsMatch(item.NomeUsuarioRecebedor, Regex.Escape(requestBody.NomeUsuarioRecebedor), RegexOptions.IgnoreCase));
                Assert.True(item.SituacaoSolicRecorrencia == requestBody.SituacaoSolicRecorrencia);
            }
        }
        [Fact]
        public async Task GetSolicitacoes_RetornaOK_QuandoPaginado()
        {
            var requestBody = new GetListaSolicAutorizacaoRecDTO
            {
                CpfCnpjUsuarioPagador = "98765432100",
                SituacaoSolicRecorrencia = "",
                NomeUsuarioRecebedor = "",
                ContaUsuarioPagador = "12134"
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/v1.0/pix-automatico/solicitacoes?pageSize=2", requestContent);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<ApiMetaDataPaginatedResponse>(responseString);

            Assert.True(jsonData.Status);
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
            var response = await _client.PostAsync("/v1.0/pix-automatico/solicitacoes", requestContent);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetDetalheSolicitacao_RetornaOK_CodigoValido()
        {
            string idSolicitacao = "REC01";
            var response = await _client.GetAsync($"/v1.0/pix-automatico/solicitacoes/{idSolicitacao}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<ApiMetaDataPaginatedResponse>(responseString);

            SolicitacaoRecorrencia item = jsonData.Data.Items.First().ToObject<SolicitacaoRecorrencia>();

            Assert.True(jsonData.Status);
            Assert.Single(jsonData.Data.Items);
            Assert.Equal(idSolicitacao, item.IdSolicRecorrencia);
        }
        [Fact]
        public async Task GetDetalheSolicitacao_RetornaErro_CodigoInValido()
        {
            string idSolicitacao = "12345";
            var response = await _client.GetAsync($"/v1.0/pix-automatico/solicitacoes/{idSolicitacao}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<ApiMetaDataPaginatedResponse>(responseString);

            Assert.False(jsonData.Status);
            Assert.Empty(jsonData.Data.Items);
            Assert.Equal("ERRO-PIXAUTO-003", jsonData.Message);
        }
    }
}
