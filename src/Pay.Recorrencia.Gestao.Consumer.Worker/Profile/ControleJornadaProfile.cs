using Pay.Recorrencia.Gestao.Application.Commands.ControleJornada;
using Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Lista;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Consumer.Worker.Profile
{
    public class ControleJornadaProfile : AutoMapper.Profile
    {
        public ControleJornadaProfile()
        {
            CreateMap<AtualizarControleJornadaCommand, ControleJornada>();
            CreateMap<IncluirControleJornadaCommand, ControleJornada>();
            CreateMap<ControleJornadaEntrada, MensagemControleJornada>();
            CreateMap<ListaControleJornadaAgendamentoAutorizacaoRequest, JornadaAutorizacaoAgendamentoDTO>();
            CreateMap<Jornada, ControleJornadaEntrada>();
        }
    }
}
