using System.ComponentModel.DataAnnotations;
using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Domain.Validators
{
    public class DtExpiracaoAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var request = validationContext.ObjectInstance as GetListaSolicAutorizacaoRecDTO;

            DateTime dtExpiracaoInicio = request.DtExpiracaoInicio;
            DateTime dtExpiracaoFim = request.DtExpiracaoFim;
            var status = ValidationResult.Success;

            if (dtExpiracaoInicio != DateTime.MinValue && dtExpiracaoFim == DateTime.MinValue)
            {
                status = new ValidationResult("O campo dtExpiracaoFim precisa ser enviado");
            }
            else if(dtExpiracaoInicio == DateTime.MinValue && dtExpiracaoFim != DateTime.MinValue)
            {
                status = new ValidationResult("O campo dtExpiracaoInicio precisa ser enviado");
            }
            return status;
        }
    }
}