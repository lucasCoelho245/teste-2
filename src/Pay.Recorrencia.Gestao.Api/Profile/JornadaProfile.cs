using Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Detalhes;
using Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Lista;
using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Api.Profile
{
    public class JornadaProfile : AutoMapper.Profile
    {
        public JornadaProfile()
        {
            CreateMap<DetalhesControleJornadaAgendamentoResquest, JornadaAgendamentoDTO>();
            CreateMap<DetalhesControleJornadaAutorizacaoResquest, JornadaAutorizacaoDTO>();
            CreateMap<ListaControleJornadaAgendamentoAutorizacaoRequest, JornadaAutorizacaoAgendamentoDTO>();
            CreateMap<ListaControleJornadaRequest, JornadaDTO>();
        }
    }
}
