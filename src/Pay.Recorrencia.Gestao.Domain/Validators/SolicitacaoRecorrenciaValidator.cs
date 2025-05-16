using Pay.Recorrencia.Gestao.Domain.Entities;
using System.Text.RegularExpressions;

namespace Pay.Recorrencia.Gestao.Domain.Validators
{
    public static class SolicitacaoRecorrenciaValidator
    {
        public static List<string> Validar(SolicitacaoRecorrencia dados)
        {
            var erros = new List<string>();

            // Validação para o campo CpfCnpjUsuarioRecebedor
            if (string.IsNullOrEmpty(dados.CpfCnpjUsuarioRecebedor) || !Regex.IsMatch(dados.CpfCnpjUsuarioRecebedor, @"^\d{11}|\d{14}$"))
            {
                erros.Add("CPF ou CNPJ do usuário recebedor inválido.");
            }

            // Validação para o campo CpfCnpjDevedor
            if (string.IsNullOrEmpty(dados.CpfCnpjDevedor) || !Regex.IsMatch(dados.CpfCnpjDevedor, @"^\d{11}|\d{14}$"))
            {
                erros.Add("CPF ou CNPJ do devedor inválido.");
            }

            //// Validação para o campo DataInicialRecorrencia
            //if (dados.DataInicialRecorrencia == DateTime.MinValue)
            //{
            //    erros.Add("Data de início da recorrência inválida.");
            //}

            //// Validação para o campo DataFinalRecorrencia
            //if (dados.DataFinalRecorrencia == DateTime.MinValue || dados.DataFinalRecorrencia <= dados.DataInicialRecorrencia)
            //{
            //    erros.Add("Data de término da recorrência inválida ou menor que a data inicial.");
            //}

            //// Validação para o campo ValorFixoSolicRecorrencia (não pode ser vazio)
            //if (string.IsNullOrEmpty(dados.ValorFixoSolicRecorrencia) || !decimal.TryParse(dados.ValorFixoSolicRecorrencia, out decimal valorFixo) || valorFixo <= 0)
            //{
            //    erros.Add("Valor fixo da solicitação de recorrência inválido.");
            //}

            //// Validação para o campo ValorMinRecebedorSolicRecorr
            //if (string.IsNullOrEmpty(dados.ValorMinRecebedorSolicRecorr) || !decimal.TryParse(dados.ValorMinRecebedorSolicRecorr, out decimal valorMinimo) || valorMinimo <= 0)
            //{
            //    erros.Add("Valor mínimo do recebedor inválido.");
            //}

            //// Validação para o campo IdRecorrencia
            //if (string.IsNullOrEmpty(dados.IdRecorrencia))
            //{
            //    erros.Add("ID da recorrência não pode ser vazio.");
            //}

            // Validação para o campo NomeUsuarioRecebedor
            if (string.IsNullOrEmpty(dados.NomeUsuarioRecebedor))
            {
                erros.Add("Nome do usuário recebedor não pode ser vazio.");
            }

            return erros;
        }
    }
}
