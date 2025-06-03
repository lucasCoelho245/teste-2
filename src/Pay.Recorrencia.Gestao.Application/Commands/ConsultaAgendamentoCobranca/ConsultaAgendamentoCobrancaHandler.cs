using MediatR;
using Pay.Recorrencia.Gestao.Application.Services;
using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Application.Commands.ConsultaAgendamentoCobranca
{
    public class ConsultaAgendamentoCobrancaHandler : IRequestHandler<ConsultaAgendamentoCobrancaCommand, List<PixAgendamentoDTO>>
    {
        private readonly HttpClient _httpClient;
        private IJornadaService _jornada;

        public ConsultaAgendamentoCobrancaHandler(IJornadaService jornada)
        {
            _httpClient = new HttpClient();
            _jornada = jornada;
        }

        public async Task<List<PixAgendamentoDTO>> Handle(ConsultaAgendamentoCobrancaCommand request, CancellationToken cancellationToken)
        {
            if (!ValidateRequest(request))
                throw new ArgumentException("ERRO-PIXAUTO-017");

            var dto = new ListarPixAgendadosRequestDTO
            {
                NrSpb = "Nº SPB Crefisa",
                Agencia = request.AgenciaUsuarioPagador,
                IdTipoConta = request.IdTipoContaPagador,
                Conta = request.ContaUsuarioPagador
            };

            var listaPixAgendados = await ListarPixAgendados(dto);
  
            var listaAgendamentos = await ListarFiltrarAgendamentos(listaPixAgendados, request);

            return listaAgendamentos;
        }

        public async Task<List<ListarPixAgendadosResponseDTO>> ListarPixAgendados(ListarPixAgendadosRequestDTO requestDTO)
        {
            //var response = await _httpClient.PostAsJsonAsync("api/consulta/ListarPixAgendados", body);
            //response.EnsureSuccessStatusCode();

            //var result = JsonSerializer.Deserialize<IEnumerable<ListarPixAgendadosResponseDTO>>(await response.Content.ReadAsStringAsync());

            var responseDTO = new List<ListarPixAgendadosResponseDTO>
            {
                new ListarPixAgendadosResponseDTO
                {
                    ListaOperacaoCompleta = new List<string>
                    {
                        "1",
                        "2",
                        "3"
                    },
                    IdOperacao = "12345",
                    IdRecorrencia = "REC123",
                    VlOperacao = 150.75m,
                    VlDocumento = 150.75m,
                    DtVencimento = DateTime.Parse("2025-05-20"),
                    IdFimAFim = "FIM123",
                    IdInicPagto = "INIC456",
                    IdConciliacaoRecebedor = "CONC789",
                    IdPayload = "PAYLOAD123",
                    IdRevisaoPayload = "REV456",
                    DtPagto = DateTime.Parse("2025-05-21"),
                    DtHrOperacao = DateTime.Parse("2025-05-15T10:30:00"),
                    DhLiquidacao = DateTime.Parse("2025-05-22T15:00:00"),
                    NmPessoaRecebedor = "João Silva",
                    IdTipoPessoaRecebedor = "F",
                    NrCpfCnpjPessoaRecebedor = "12345678901",
                    NrSpbRecebedor = "SPB123",
                    NrAgenciaRecebedor = "0001",
                    IdTipoContaRecebedor = "CC",
                    NrContaRecebedor = "123456",
                    NmPessoaPagador = "Maria Oliveira",
                    IdTipoPessoaPagador = "F",
                    NrCpfCnpjPessoaPagador = "98765432100",
                    NrSpbPagador = "SPB456",
                    NrAgenciaPagador = "0002",
                    IdTipoContaPagador = "CP",
                    NrContaPagador = "654321",
                    IdTipoSituacaoAtual = "PENDENTE",
                    DescricaoSituacaoAtual = "Pagamento pendente",
                    IdSpi = "SPI123",
                    IdInstFinanc = "INST456",
                    IdFilialInst = "FILIAL789",
                    NrDoctosGeradoContaJson = "DOC123",
                    NrSpbControlador = "SPBCTRL123",
                    NrDocumentoOrigem = "DOCORIG456",
                    TxChave = "CHAVEPIX123",
                    TxDescricao = "Pagamento de conta",
                    FlDevolucao = "N",
                    IcPermiteAlterarValor = true,
                    LstRetorno = new List<ListarPixAgendadosRetornoResponseDTO>
                    {
                        new ListarPixAgendadosRetornoResponseDTO
                        {
                            Codigo = "001",
                            Descricao = "Sucesso"
                        },
                        new ListarPixAgendadosRetornoResponseDTO
                        {
                            Codigo = "002",
                            Descricao = "Erro no processamento"
                        }
                    },
                    Codigo = "001",
                    Descricao = "Mock",
                    TpFinalidade = "Pagamento",
                    LstTipoValor = new List<ListarPixAgendadosTipoValorResponseDTO>
                    {
                        new ListarPixAgendadosTipoValorResponseDTO
                        {
                            VlTipo = "100.00",
                            TpPagto = "PIX"
                        },
                        new ListarPixAgendadosTipoValorResponseDTO
                        {
                            VlTipo = "50.75",
                            TpPagto = "TED"
                        }
                    },
                    VlTipo = "VlTipo",
                    TpPagmto = "TpPagmto",
                    ParcelaAtual = 1,
                    TotalParcelas = 3,
                    MotivoDevolucao = "Saldo insuficiente"
                }
            };

            return responseDTO;
        }

        private async Task<List<PixAgendamentoDTO>> ListarFiltrarAgendamentos(List<ListarPixAgendadosResponseDTO> listDTO, ConsultaAgendamentoCobrancaCommand request)
        {
            var dtoResponse = new List<PixAgendamentoDTO>();

            if (listDTO.Any())
            {
                foreach (var dto in listDTO)
                {
                    if (ValidarFiltro(request.NomeUsuarioRecebedor, dto.NmPessoaRecebedor))
                    {
                        var jornadaDTO = new JornadaDTO
                        {
                            IdRecorrencia = dto.IdRecorrencia,
                            IdConciliacaoRecebedor = dto.IdConciliacaoRecebedor,
                        };

                        //var requestJornada = await _jornada.GetByAnyFilterAsync(jornadaDTO);

                        var requestJornada = new JornadaDTO
                        {
                            TpJornada = "JornadaTipo1",
                            IdRecorrencia = "REC123",
                            IdE2E = "E2E67890",
                            IdConciliacaoRecebedor = "CONC789",
                            SituacaoJornada = "Agendado",
                            DtAgendamento = DateTime.Now,
                            VlAgendamento = 1500.75m,
                            DtPagamento = DateTime.Now,
                            DataHoraCriacao = DateTime.Now,
                            DataUltimaAtualizacao = DateTime.Now.AddHours(-2) // Última atualização há 2 horas

                        };

                        //if (requestJornada.Data?.SituacaoJornada == "Agendado")
                        if (requestJornada.SituacaoJornada == "Agendado")
                        {
                            var responseDTO = new PixAgendamentoDTO
                            {
                                IdOperacao = dto.IdOperacao,
                                IdRecorrencia = dto.IdRecorrencia,
                                VlOperacao = dto.VlOperacao,
                                DtPagto = dto.DtPagto,
                                NomeUsuarioRecebedor = dto.NmPessoaRecebedor
                            };

                            dtoResponse.Add(responseDTO);
                        }
                    }
                }
            }

            if (dtoResponse.Count > 0)
                dtoResponse = (List<PixAgendamentoDTO>)dtoResponse.OrderByDescending(x => x.DtPagto).ToList();

            return dtoResponse;
        }

        private bool ValidarFiltro(string? nomeUsuarioRecebedor, string nmPessoaRecebedor)
        {
            if (!string.IsNullOrEmpty(nomeUsuarioRecebedor))
            {
                if (!string.IsNullOrEmpty(nmPessoaRecebedor) && !nomeUsuarioRecebedor.ToUpper().Contains(nmPessoaRecebedor.ToUpper()))
                    return false;
            }

            return true;
        }

        private bool ValidateRequest(ConsultaAgendamentoCobrancaCommand request)
        {
            string[] tipoContaPagador = { "CACC", "SLRY", "SVGS", "TRAN", "CAHO", "CCTE", "DBMO", "DBMI", "DORD" };

            var requestValido = (!string.IsNullOrEmpty(request.ContaUsuarioPagador)
                || !string.IsNullOrEmpty(request.NomeUsuarioRecebedor)
                || !string.IsNullOrEmpty(request.AgenciaUsuarioPagador));

            var tipoContaPagadorValido = tipoContaPagador.Contains(request.IdTipoContaPagador);

            if (requestValido && tipoContaPagadorValido)
            {
                if (string.IsNullOrEmpty(request.ContaUsuarioPagador))
                    return false;

                //else if (!string.IsNullOrEmpty(request.NomeUsuarioRecebedor))
                //{
                //    //validacoes caso não for vazio
                //    //Obs: campo nullable, então precisa saber o padrão do usuario
                //}
                //else if (!string.IsNullOrEmpty(request.AgenciaUsuarioPagador))
                //{
                //    //validacoes caso não for vazio
                //    //Obs: campo nullable, então precisa saber o padrão do usuario
                //}
            }
            else
                return false;

            return true;
        }
    }
}
