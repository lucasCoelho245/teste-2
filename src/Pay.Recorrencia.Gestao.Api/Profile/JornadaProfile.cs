using Pay.Recorrencia.Gestao.Application.Commands.ControleJornada;
using Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Detalhes;
using Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Lista;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;

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
            CreateMap<IncluirControleJornadaCommand, ControleJornada>();
            CreateMap<AtualizarControleJornadaCommand, ControleJornada>();
        }
    }
}
