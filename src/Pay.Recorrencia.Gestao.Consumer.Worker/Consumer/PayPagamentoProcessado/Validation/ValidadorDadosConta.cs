using Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.PayPagamentoProcessado.Validation.IValidation;
using Pay.Recorrencia.Gestao.Domain.Entities;
using WebService_EBServicesBar;

namespace Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.PayPagamentoProcessado.Validation
{
    public class ValidadorDadosConta(IValidacaoSolicitacao proxima, ILogger<ValidadorDadosConta> logger) : IValidacaoSolicitacao
    {
        private readonly ILogger<ValidadorDadosConta> _logger = logger;
        private readonly IValidacaoSolicitacao _proxima = proxima;

        public string Validar(SolicitacaoRecorrenciaEntrada dados)
        {
            var motivo = ValidarDadosContaAsync(dados);
            return string.IsNullOrEmpty(motivo) ? _proxima.Validar(dados) : motivo;
        }
        private string ValidarDadosContaAsync(SolicitacaoRecorrenciaEntrada dados)
        {
            List<string> listaSituacoesConta1 = ["ECS", "ENC", "EMF", "INA"];
            List<string> listaSituacoesConta2 = ["BPR", "BCI", "BFR", "BFT", "BOC", "BLQ", "BOB", "BQJ"];

            using (EBServicesBarClient client = new())
            {
                consultaContasTitularResponse retorno = client.consultaContasTitularAsync(codCliente: "", cnpj_Cpf: "", codColigada: "", nroConta: dados.ContaUsuarioPagador, codAgencia: "", modalidade: "", situacao: "").GetAwaiter().GetResult();

                //todo: um titular pode ter mais de uma conta?

                string cnpjCpf_Titular1 = retorno?.consultaContasTitularReturn?.FirstOrDefault()?.cnpjCpf_Titular1 ?? string.Empty;
                string cnpjCpf_Titular2 = retorno?.consultaContasTitularReturn?.FirstOrDefault()?.cnpjCpf_Titular2 ?? string.Empty;
                string situacao = retorno?.consultaContasTitularReturn?.FirstOrDefault()?.situacao ?? string.Empty;

                if (retorno == null)
                {
                    _logger.LogError("Erro: Conta {ContaUsuarioPagador} não encontrada.", dados.ContaUsuarioPagador);
                    return "AC01";
                }
                else if (!dados.CpfCnpjUsuarioPagador.Equals(cnpjCpf_Titular1) || !dados.CpfCnpjUsuarioPagador.Equals(cnpjCpf_Titular2))
                {
                    _logger.LogError("Erro: CpfCnpjUsuarioPagador {CpfCnpjUsuarioPagador} não corresponde a nenhum dos titular da conta {ContaUsuarioPagador}.", dados.CpfCnpjUsuarioPagador, dados.ContaUsuarioPagador);
                    return "AC02";
                }
                else if (listaSituacoesConta1.Contains(situacao))
                {
                    _logger.LogError("Erro: Conta {ContaUsuarioPagador} está com situação {Situacao}.", dados.ContaUsuarioPagador, situacao);
                    return "AC04";
                }
                else if (listaSituacoesConta2.Contains(situacao))
                {
                    _logger.LogError("Erro: Conta {ContaUsuarioPagador} está com situação {Situacao}.", dados.ContaUsuarioPagador, situacao);
                    return "AC06";
                }
                else if (dados.AgenciaUsuarioPagador != "0001")
                {
                    _logger.LogError("Erro: Agência {AgenciaUsuarioPagador} não corresponde a agencia 0001.", dados.AgenciaUsuarioPagador);
                    return "AP03";
                }
            }

            return string.Empty;
        }
    }
}
