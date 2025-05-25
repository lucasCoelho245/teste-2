using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/pix-automatico/conexao-db-test")]
    public class ConexaoDBTesteController : ControllerBase
    {
        //private readonly IConexaoDBRepository _produto;
        private readonly ILogger<ConexaoDBTesteController> _logger;

        private readonly string _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=TesteAPIs;Trusted_Connection=True;";

        public ConexaoDBTesteController(ILogger<ConexaoDBTesteController> logger, IConfiguration configuration)
        {
            //_produto = produto;
            _logger = logger;
            _connectionString = configuration.GetSection("DbConfig")["ConnectionString"];
        }

        [HttpGet("testar-conexao")]
        public async Task<IActionResult> TestarConexao()
        {
            try
            {
                // Tenta abrir uma conexão com o banco de dados
                using (var connection = new Microsoft.Data.SqlClient.SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sucess = connection.ConnectionString + " " + "Conexão com o banco de dados foi bem-sucedida!";
                    return Ok(sucess);
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                // Retorna erro caso a conexão falhe
                return StatusCode(500, $" {_connectionString} - Erro ao conectar ao banco de dados: {ex.Message}");
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] ConexaoDB produto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    try
        //    {
        //        await _produto.InserirAsync(produto);
        //        return CreatedAtAction(nameof(Post), new { produto.Nome }, produto);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Erro ao inserir produto no banco.");
        //        return StatusCode(500, new
        //        {
        //            message = "Erro interno ao inserir no banco.",
        //            error = ex.Message
        //        });
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        var produtos = await _produto.GetAllAsync();
        //        return produtos.Any() ? Ok(produtos) : NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Erro ao obter produtos do banco.");
        //        return StatusCode(500, new
        //        {
        //            message = "Erro interno ao consultar o banco.",
        //            error = ex.Message
        //        });
        //    }
        //}
    }
}
