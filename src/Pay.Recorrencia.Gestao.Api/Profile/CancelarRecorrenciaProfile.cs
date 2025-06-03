using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Api.Profile;

public class CancelarRecorrenciaProfile: AutoMapper.Profile
{
    public CancelarRecorrenciaProfile()
    {
        CreateMap<AutorizacaoRecorrencia, RecorrenciaUnificadaDto>();
        CreateMap<SolicAutorizacaoRecNonPagination, RecorrenciaUnificadaDto>();
    }
}