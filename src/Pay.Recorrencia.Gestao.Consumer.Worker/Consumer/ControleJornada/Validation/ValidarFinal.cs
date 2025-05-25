using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.ControleJornada.Validation.IValidation;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.ControleJornada.Validation
{
    public class ValidarFinal : IValidationJornada
    {
        public string? Validar(ControleJornadaEntrada dados)
        {
            return string.Empty;
        }
    }
}
