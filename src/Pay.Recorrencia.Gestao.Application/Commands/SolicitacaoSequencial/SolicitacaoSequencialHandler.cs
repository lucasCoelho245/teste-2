using MediatR;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoSequencial
{
    public class SolicitacaoSequencialHandler : IRequestHandler<SolicitacaoSequencialCommand, long>
    {
        private ISolicitacaoSequencialRepository _solicitacaoSequencialRepository;

        public SolicitacaoSequencialHandler(ISolicitacaoSequencialRepository solicitacaoSequencialRepository)
        {
            _solicitacaoSequencialRepository = solicitacaoSequencialRepository;
        }

        public async Task<long> Handle(SolicitacaoSequencialCommand request, CancellationToken cancellationToken)
        {
            long novoSequencial;
            var dataHoje = DateTime.UtcNow.Date;

            var sequencialAtual = await _solicitacaoSequencialRepository.GetSequencialAsync(dataHoje);

            if (sequencialAtual.HasValue)
            {
                novoSequencial = sequencialAtual.Value + 1;
                await _solicitacaoSequencialRepository.UpdateSequencialByDateAsync(dataHoje, novoSequencial);
            }
            else
            {
                novoSequencial = 0;
                await _solicitacaoSequencialRepository.CreateSequencialAsync(dataHoje);
            }

            return await Task.FromResult(novoSequencial);
        }
    }
}
