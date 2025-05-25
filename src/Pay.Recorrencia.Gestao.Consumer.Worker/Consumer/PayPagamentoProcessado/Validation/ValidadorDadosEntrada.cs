using Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.PayPagamentoProcessado.Validation.IValidation;
using Pay.Recorrencia.Gestao.Domain.Entities;
using System.Text.RegularExpressions;

namespace Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.PayPagamentoProcessado.Validation
{
    internal class ValidadorDadosEntrada(IValidacaoSolicitacao proxima, ILogger<ValidadorDadosEntrada> logger) : IValidacaoSolicitacao
    {
        private readonly ILogger<ValidadorDadosEntrada> _logger = logger;
        private readonly IValidacaoSolicitacao _proxima = proxima;

        public string Validar(SolicitacaoRecorrenciaEntrada dados)
        {
            var motivo = ValidarDadosEntrada(dados);
            return string.IsNullOrEmpty(motivo) ? _proxima.Validar(dados) : motivo;
        }
        private string ValidarDadosEntrada(SolicitacaoRecorrenciaEntrada dados)
        {
            string motivoRejeicao = ValidarCamposObrigatorios(dados);
            motivoRejeicao = ValidateCpfCnpjFormat(dados.CpfCnpjUsuarioRecebedor);

            if (!string.IsNullOrEmpty(dados.CpfCnpjDevedor))
                motivoRejeicao = ValidateCpfCnpjFormat(dados.CpfCnpjDevedor);

            motivoRejeicao = ValidateDominio(dados.TipoFrequencia);

            motivoRejeicao = ValidarDatas(dados);

            motivoRejeicao = ValidarIdRecorrencia(dados.IdRecorrencia);

            return motivoRejeicao;
        }

        public string ValidarCamposObrigatorios(SolicitacaoRecorrenciaEntrada entrada)
        {
            var propriedadesRequired = typeof(SolicitacaoRecorrenciaEntrada)
                .GetProperties()
                .Where(p => p.CustomAttributes.Any(a => a.AttributeType.Name == "RequiredMemberAttribute"));

            foreach (var prop in propriedadesRequired)
            {
                var valor = prop.GetValue(entrada) as string;
                if (string.IsNullOrWhiteSpace(valor))
                {
                    _logger.LogError("CH16 - Campo obrigatório não preenchido: {Campo}", prop.Name);
                    return "CH16";
                }
            }
            return string.Empty; // Todos os campos obrigatórios estão preenchidos
        }

        private string ValidateCpfCnpjFormat(string input)
        {
            bool result = Regex.IsMatch(input, @"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$|^\d{2}\.?\d{3}\.?\d{3}/?\d{4}-?\d{2}$");

            if (result)
                return string.Empty;
            else
            {
                _logger.LogError("CH16 - Formato inválido para CPF/CNPJ: {Input}", input);
                return "CH16";
            }
        }

        private string ValidateDominio(string tipoFrequencia)
        {
            var dominiosValidos = new[] { "MIAN", "MNTH", "QURT", "WEEK", "YEAR" };
            if (!dominiosValidos.Contains(tipoFrequencia))
            {
                _logger.LogError("CH16 - TipoFrequencia inválido. Domínios válidos: MIAN, MNTH, QURT, WEEK, YEAR. Tipo fornecido: {TipoFrequencia}", tipoFrequencia);
                return "CH16";
            }

            return string.Empty;
        }

        private string ValidarDatas(SolicitacaoRecorrenciaEntrada dados)
        {
            if (!DateTime.TryParse(dados.DataInicialRecorrencia, out var dataInicial) ||
                !DateTime.TryParse(dados.DataFinalRecorrencia, out var dataFinal))
            {
                _logger.LogError("CH16 - Formato inválido para DataInicialRecorrencia ou DataFinalRecorrencia.");
                return "CH16";
            }

            if (dataFinal <= dataInicial)
            {
                _logger.LogError("CH16 - A data final da recorrência deve ser maior que a data inicial.");
                return "CH16";
            }

            if (!DateTime.TryParse(dados.DataHoraCriacaoSolicRecorr, out var dataCriacao) ||
                !DateTime.TryParse(dados.DataHoraExpiracaoSolicRecorr, out var dataExpiracao))
            {
                _logger.LogError("CH16 - Formato inválido para DataHoraCriacaoSolicRecorr ou DataHoraExpiracaoSolicRecorr.");
                return "CH16";
            }

            if (dataExpiracao <= dataCriacao)
            {
                _logger.LogError("CH16 - DataHoraExpiracaoSolicRecorr deve ser maior que DataHoraCriacaoSolicRecorr.");
                return "CH16";
            }

            if ((dataExpiracao - dataCriacao).TotalDays > 30)
            {
                _logger.LogError("CH16 - DataHoraExpiracaoSolicRecorr não pode exceder 30 dias após a criação.");
                return "CH16";
            }

            if (dataExpiracao <= DateTime.UtcNow)
            {
                _logger.LogError("MD20 - A data de expiração da solicitação deve ser futura.");
                return "MD20";
            }

            return string.Empty;
        }

        private string ValidarIdRecorrencia(string IdRecorrencia)
        {
            Regex regexId = new(@"^(RR|RN|CR|CN)\d{8}\d{8}[a-zA-Z0-9]{11}$");
            if (!regexId.IsMatch(IdRecorrencia))
            {
                _logger.LogError("AP04 - O idRecorrencia está fora do padrão esperado. Id fornecido: {IdRecorrencia}", IdRecorrencia);
                return "AP04";
            }
            return string.Empty;
        }
    }
}
