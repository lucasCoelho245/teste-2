using Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoRecorrencia;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Api.Profile
{
    public class SolicitacaoAutorizacaoRecorrenciaProfile : AutoMapper.Profile
    {
        public SolicitacaoAutorizacaoRecorrenciaProfile()
        {
            CreateMap<IncluirSolicitacaoRecorrenciaCommand, SolicitacaoRecorrencia>();
            CreateMap<AtualizarSolicitacaoRecorrenciaCommand, SolicitacaoAutorizacaoRecorrenciaUpdateDTO>();
        }
    }
}
