using Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoRecorrencia;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Consumer.Worker.Profile;

public class RecorrenciaProfile : AutoMapper.Profile
{
    public RecorrenciaProfile()
    {
        // Mapeia todas as propriedades com mesmo nome
        CreateMap<SolicitacaoRecorrenciaEntrada, ProcessamentoRecorrencia>();

        // Mapeamento customizado para IncluirSolicitacaoRecorrenciaCommand
        CreateMap<SolicitacaoRecorrenciaEntrada, IncluirSolicitacaoRecorrenciaCommand>()
            .ForMember(dest => dest.DataInicialRecorrencia,
                opt => opt.MapFrom(src => DateTime.Parse(src.DataInicialRecorrencia)))
            .ForMember(dest => dest.DataFinalRecorrencia,
                opt => opt.MapFrom(src => string.IsNullOrEmpty(src.DataFinalRecorrencia) ? (DateTime?)null : DateTime.Parse(src.DataFinalRecorrencia)))
            .ForMember(dest => dest.ValorFixoSolicRecorrencia,
                opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ValorFixoSolicRecorrencia) ? (decimal?)null : decimal.Parse(src.ValorFixoSolicRecorrencia)))
            .ForMember(dest => dest.ValorMinRecebedorSolicRecorr,
                opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ValorMinRecebedorSolicRecorr) ? (decimal?)null : decimal.Parse(src.ValorMinRecebedorSolicRecorr)))
            .ForMember(dest => dest.DataHoraCriacaoRecorr,
                opt => opt.MapFrom(src => DateTime.Parse(src.DataHoraCriacaoRecorr)))
            .ForMember(dest => dest.DataHoraCriacaoSolicRecorr,
                opt => opt.MapFrom(src => DateTime.Parse(src.DataHoraCriacaoSolicRecorr)))
            .ForMember(dest => dest.DataHoraExpiracaoSolicRecorr,
                opt => opt.MapFrom(src => DateTime.Parse(src.DataHoraExpiracaoSolicRecorr)))
            // Propriedades que não existem em SolicitacaoRecorrenciaEntrada podem ser ignoradas ou configuradas conforme a regra de negócio
            .ForMember(dest => dest.TipoRecorrencia, opt => opt.MapFrom(_ => "RCUR")) // Valor fixo conforme regex do destino
            .ForMember(dest => dest.SituacaoSolicRecorrencia, opt => opt.MapFrom(_ => "PNDG")) // Valor default, ajuste conforme necessário
            .ForMember(dest => dest.CodigoMoedaSolicRecorr, opt => opt.MapFrom(_ => "BRL")) // Valor default, ajuste conforme necessário
            .ForMember(dest => dest.IndicadorValorMin, opt => opt.Ignore()) // Ignorar se não houver correspondência
            .ForMember(dest => dest.DataUltimaAtualizacao, opt => opt.MapFrom(_ => DateTime.UtcNow)) // Exemplo: data atual
            .ForMember(dest => dest.IdAutorizacao, opt => opt.Ignore()); // Ignorar se não houver correspondência

        CreateMap<IncluirSolicitacaoRecorrenciaCommand, SolicitacaoRecorrencia>();
        CreateMap<AtualizarSolicitacaoRecorrenciaCommand, SolicitacaoAutorizacaoRecorrenciaUpdateDTO>();
    }
}