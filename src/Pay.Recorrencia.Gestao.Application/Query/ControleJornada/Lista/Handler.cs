using AutoMapper;
using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Lista
{
    public class ListaControleJornadaHandler :  IRequestHandler<ListaControleJornadaRequest, ListaControleJornadaResponse>,
                                                IRequestHandler<ListaControleJornadaAgendamentoAutorizacaoRequest, ListaControleJornadaResponse>
            

    {
        private IJornadaRepository _jornadaRepository { get; }
        private IMapper _mapper { get; }
        public ListaControleJornadaHandler(IJornadaRepository jornadaRepository, IMapper mapper)
        {
            _jornadaRepository = jornadaRepository;
            _mapper = mapper;
        }
        public async Task<ListaControleJornadaResponse> Handle(ListaControleJornadaRequest request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<Domain.DTO.JornadaDTO>(request);

            var controleJornada = await _jornadaRepository.GetAllAsync(dto);

            bool statusBool = controleJornada.Items.Any();
            string status = statusBool ? "OK" : "";

            var response = new ListaControleJornadaResponse()
            {
                Status = status,
                StatusCode = statusBool ? 200 : 404,
                Data = new ItemsData()
                {
                    Items = controleJornada.Items,
                    Pagination = new Pagination()
                    {
                        TotalItems = controleJornada.TotalItems,
                        TotalPages = (int)Math.Ceiling((double)controleJornada.TotalItems / request.PageSize),
                        PageSize = request.PageSize,
                        CurrentPage = request.Page
                    },
                },

                Message = statusBool ? "" : "Nenhuma jornada encontrada"
            };
            return await Task.FromResult(response);
        }

        public async Task<ListaControleJornadaResponse> Handle(ListaControleJornadaAgendamentoAutorizacaoRequest request, CancellationToken cancellationToken)
        {
            try 
            {
                var dto = _mapper.Map<Domain.DTO.JornadaAutorizacaoAgendamentoDTO>(request);

                var controleJornada = await _jornadaRepository.GetByAnyFilterAsync(dto);

                bool statusBool = controleJornada.Items.Any();
                string status = statusBool ? "OK" : "";

                var response = new ListaControleJornadaResponse()
                {
                    Status = status,
                    StatusCode = statusBool ? 200 : 404,
                    Data = new ItemsData()
                    {
                        Items = controleJornada.Items,
                        Pagination = new Pagination()
                        {
                            TotalItems = controleJornada.TotalItems,
                            TotalPages = (int)Math.Ceiling((double)controleJornada.TotalItems / request.PageSize),
                            PageSize = request.PageSize,
                            CurrentPage = request.Page
                        },
                    },

                    Message = statusBool ? "" : "Nenhuma jornada encontrada"
                };
                return await Task.FromResult(response);
            } catch (Exception ex)
            {
                return new ListaControleJornadaResponse()
                {
                    Status = "ERROR",
                    StatusCode = 500,
                    Data = null,
                    Message = ex.Message
                };
            }
        }
    }
}
