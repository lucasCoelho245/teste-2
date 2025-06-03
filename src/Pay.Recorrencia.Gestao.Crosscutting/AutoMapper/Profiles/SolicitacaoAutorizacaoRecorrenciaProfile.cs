using AutoMapper;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;


namespace Pay.Recorrencia.Gestao.Crosscutting.AutoMapper.Profiles
{
    public class SolicitacaoAutorizacaoRecorrenciaProfile : Profile
    {
        public SolicitacaoAutorizacaoRecorrenciaProfile()
        {
            CreateMap<SolicitacaoAutorizacaoRecorrenciaDetalhes, SolicitacaoAutorizacaoRecorrenciaDetalhesDTO>()
                .ForMember(dest => dest.IndicadorValorMin, opt => opt.MapFrom(src => new[] { "T", "true", "TRUE", "t", "1", "v", "V" }.Any(item => item == src.IndicadorValorMin)));
        }
    }
}