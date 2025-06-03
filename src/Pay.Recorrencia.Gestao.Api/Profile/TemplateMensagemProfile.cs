using Pay.Recorrencia.Gestao.Application.Commands.TemplateMensagem;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Api.Profile
{
    public class TemplateMensagemProfile : AutoMapper.Profile
    {
        public TemplateMensagemProfile()
        {
            CreateMap<IncluirTemplateMensagemCommand, TemplateMensagem>();
        }
    }
}
