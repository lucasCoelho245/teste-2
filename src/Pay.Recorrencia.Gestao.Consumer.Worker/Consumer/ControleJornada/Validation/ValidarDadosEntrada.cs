using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.ControleJornada.Validation.IValidation;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.ControleJornada.Validation
{
    public class ValidarDadosEntrada : IValidationJornada
    {

        private readonly IValidationJornada _proxima;

        public ValidarDadosEntrada(IValidationJornada proxima)
        {
            _proxima = proxima;
        }

        public string? Validar(ControleJornadaEntrada dados)
        {
            var motivo = ValidarDadoEntrada(dados);
            return string.IsNullOrEmpty(motivo) ? _proxima.Validar(dados) : motivo;
        }

        private string ValidarDadoEntrada(ControleJornadaEntrada dados)
        {
            string motivoRejeicao = string.Empty;

            motivoRejeicao = ValidarCamposObrigatorios(dados);
            if (!string.IsNullOrEmpty(motivoRejeicao))
                return motivoRejeicao;


            motivoRejeicao = ValidateDominio(dados);
            if (!string.IsNullOrEmpty(motivoRejeicao))
                return motivoRejeicao;


            return motivoRejeicao;
        }

        public string ValidarCamposObrigatorios(ControleJornadaEntrada dados)
        {
            if (string.IsNullOrWhiteSpace(dados.TpJornada))
                return "Campo obrigatório: TpJornada.";

            if (string.IsNullOrWhiteSpace(dados.IdRecorrencia))
                return "Campo obrigatório: IdRecorrencia.";

            return string.Empty;
        }

        public string ValidateDominio(ControleJornadaEntrada dados)
        {

            var tiposComIdFimAFim = new[] { "Jornada 3", "Jornada 4", "AGNT", "NTAG", "RIFL" };
            if (!tiposComIdFimAFim.Contains(dados.IdE2E))
                return "Valor inválido para IdFimAFim. Deve ser um dos valores permitidos.";

            var tiposComIdConciliacao = new[] { "AGND", "NTAG", "RIFL" };
            if (!tiposComIdConciliacao.Contains(dados.IdConciliacaoRecebedor))
                return "Valor inválido para IdConciliacaoRecebedor. Deve ser um dos valores permitidos.";

            return string.Empty;
        }
    }
}
