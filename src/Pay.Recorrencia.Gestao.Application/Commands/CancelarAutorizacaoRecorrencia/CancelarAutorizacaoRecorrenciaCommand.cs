using System.ComponentModel.DataAnnotations;
using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;

namespace Pay.Recorrencia.Gestao.Application.Commands.CancelarAutorizacaoRecorrencia;

public class CancelarAutorizacaoRecorrenciaCommand : IRequest<MensagemPadraoResponse>
{
     /// <summary>
    /// Participante do recebedor, com 8 dígitos
    /// </summary>
    [Required(ErrorMessage = "O ISPB é obrigatório")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "O ISPB deve ter exatamente 8 dígitos")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "O ISPB deve conter apenas 8 dígitos numéricos")]
    public string Ispb { get; set; }

    /// <summary>
    /// ID de Idempotência no formato: "ICxxxxxxxxyyyyMMddkkkkkkkkkkk"
    /// IC: fixo (2 caracteres)
    /// xxxxxxxx: ISPB do agente
    /// yyyyMMdd: data de criação
    /// kkkkkkkkkk: sequencial alfanumérico único por data (11 caracteres)
    /// </summary>
    [Required(ErrorMessage = "O ID de Informação de Cancelamento é obrigatório")]
    [StringLength(29, MinimumLength = 29, ErrorMessage = "O ID de Informação de Cancelamento deve ter exatamente 29 caracteres")]
    [RegularExpression(@"^IC\d{8}\d{8}[a-zA-Z0-9]{11}$", ErrorMessage = "O ID de Informação de Cancelamento deve seguir o padrão: IC + 8 dígitos ISPB + 8 dígitos data + 11 caracteres alfanuméricos")]
    public string IdInformacaoCancelamento { get; set; }

    /// <summary>
    /// ID da recorrência original
    /// </summary>
    [Required(ErrorMessage = "O ID da Recorrência é obrigatório")]
    public string IdRecorrencia { get; set; }

    /// <summary>
    /// CNPJ do cliente recebedor, solicitante do cancelamento (14 dígitos, sem formatação)
    /// </summary>
    [Required(ErrorMessage = "O CPF/CNPJ do Solicitante do Cancelamento é obrigatório")]
    [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter exatamente 14 dígitos")]
    [RegularExpression(@"^\d{14}$", ErrorMessage = "O CNPJ deve conter apenas 14 dígitos numéricos")]
    public string NrCpfCnpjSolicitanteCancelamento { get; set; }

    /// <summary>
    /// Motivo do Cancelamento: ACCL, CPCL, DCSD, ERSL, FRUD, NRES, PCFD, SLCR
    /// </summary>
    [Required(ErrorMessage = "O Motivo do Cancelamento é obrigatório")]
    [RegularExpression(@"^(ACCL|CPCL|DCSD|ERSL|FRUD|NRES|PCFD|SLCR)$", 
        ErrorMessage = "O Motivo do Cancelamento deve ser um dos seguintes valores: ACCL, CPCL, DCSD, ERSL, FRUD, NRES, PCFD, SLCR")]
    public string IdMotivo { get; set; }

    /// <summary>
    /// Indicativo de mensagem recebida (1 = mensagem enviada pelo Bacen)
    /// </summary>
    [Required(ErrorMessage = "A Direção é obrigatória")]
    [RegularExpression("^1$", ErrorMessage = "A Direção deve ser '1'")]
    public string Direcao { get; set; }

}