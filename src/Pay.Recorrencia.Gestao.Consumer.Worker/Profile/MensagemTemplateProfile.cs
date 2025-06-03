
using Pay.Recorrencia.Gestao.Application.Commands.TemplateMensagem;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Consumer.Worker.Profile
{
    public class MensagemTemplateProfile : AutoMapper.Profile
    {
        public MensagemTemplateProfile()
        {
            CreateMap<IncluirTemplateMensagemCommand, TemplateMensagem>();
        }
    }
}
