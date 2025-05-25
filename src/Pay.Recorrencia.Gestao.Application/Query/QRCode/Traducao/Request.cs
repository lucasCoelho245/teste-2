using MediatR;
using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Application.Query.QRCode.Traducao
{
    public class TraducaoQRCodeRequest : TraducaoQRCodeDTO, IRequest<TraducaoQRCodeResponse>
    { }
}