using System.ComponentModel.DataAnnotations;
using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Domain.Validators
{
    public class IDsDesatlhesSolicRecorrenciaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var request = validationContext.ObjectInstance as GetSolicAutorizacaoRecDTO;

            string idSolicitacao = request.IdSolicRecorrencia;
            string idRecorrencia = request.IdRecorrencia;
            var status = ValidationResult.Success;

            if (string.IsNullOrEmpty(idSolicitacao) && string.IsNullOrEmpty(idRecorrencia))
            {
                status = new ValidationResult("Um dos campos idSolicitacao e idRecorrenciaprecisa precisa ser enviado");
            }
            return status;
        }
    }
}