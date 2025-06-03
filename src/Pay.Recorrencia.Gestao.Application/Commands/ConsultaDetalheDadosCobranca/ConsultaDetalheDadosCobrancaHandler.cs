using MediatR;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pay.Recorrencia.Gestao.Application.Commands.ConsultaDetalheDadosCobranca
{
    public class ConsultaDetalheDadosCobrancaHandler : IRequestHandler<ConsultaDetalheDadosCobrancaCommand, DetalheDadosCobranca>
    {
        private readonly HttpClient _httpClient;

        public ConsultaDetalheDadosCobrancaHandler()
        {
            // TODO: definir BaseUrl
            _httpClient = new HttpClient();
        }

        public async Task<DetalheDadosCobranca> Handle(ConsultaDetalheDadosCobrancaCommand request, CancellationToken cancellationToken)
        {
            var pixAgendados = await ListarPixAgendados(new ListarPixAgendadosRequestDTO
            {
                NrSpb = "61033106",
                Agencia = request.AgenciaUsuarioPagador,
                IdTipoConta = request.IdTipoContaPagador,
                Conta = request.ContaUsuarioPagador,
                IdRecorrencia = request.IdRecorrencia,
            });


            var autorizacaoRecorrenciaLista = await ConsultaAutorizacaoRecorrencia(new ConsultaAutorizacaoRecorrenciaRequestDTO
            {
                IdRecorrencia = request.IdRecorrencia
            });

            return new DetalheDadosCobranca
            {
                IdOperacao = pixAgendados.First().IdOperacao,
                IdRecorrencia = pixAgendados.First().IdRecorrencia,
                VlOperacao = pixAgendados.First().VlOperacao,
                DtPagto = pixAgendados.First().DtPagto,
                NomeUsuarioRecebedor = autorizacaoRecorrenciaLista.NomeUsuarioRecebedor,
                CpfCnpjUsuarioRecebedor = autorizacaoRecorrenciaLista.CpfCnpjUsuarioRecebedor,
                ParticipanteDoUsuarioRecebedor = autorizacaoRecorrenciaLista.ParticipanteDoUsuarioRecebedor,
                CpfCnpjUsuarioPagador = autorizacaoRecorrenciaLista.CprfCnpjUsuarioPagador,
                NumeroContrato = autorizacaoRecorrenciaLista.NumeroContrato,
                DescObjetoContrato = autorizacaoRecorrenciaLista.DescObjetoContrato,
            };
        }

        public async Task<IEnumerable<ListarPixAgendadosResponseDTO>> ListarPixAgendados(ListarPixAgendadosRequestDTO body)
        {
            //var response = await _httpClient.PostAsJsonAsync("api/consulta/ListarPixAgendados", body);
            //response.EnsureSuccessStatusCode();

            //var result = JsonSerializer.Deserialize<IEnumerable<ListarPixAgendadosResponseDTO>>(await response.Content.ReadAsStringAsync());


            return new List<ListarPixAgendadosResponseDTO>
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
                    IdRecorrencia = "67890",
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
        }

        public async Task<ConsultaAutorizacaoRecorrenciaResponseDTO> ConsultaAutorizacaoRecorrencia(ConsultaAutorizacaoRecorrenciaRequestDTO request)
        {
            return new ConsultaAutorizacaoRecorrenciaResponseDTO
            {
                IdRecorrencia = "REC12345",
                SituacaoRecorrencia = "Ativa",
                TipoRecorrencia = "Mensal",
                TipoFrequencia = "Fixa",
                DataInicialAutorizacaoRecorrencia = DateTime.Parse("2025-01-01"),
                DataFinalAutorizacaoRecorrencia = DateTime.Parse("2025-12-31"),
                CodigoMoedaAutorizacaoRecorrencia = "BRL",
                ValorRecorrencia = 500.00m,
                MotivoRejeicaoRecorrencia = "Nenhum",
                NomeUsuarioRecebedor = "João Silva",
                CpfCnpjUsuarioRecebedor = "12345678901",
                ParticipanteDoUsuarioRecebedor = "Banco XYZ",
                CodMunIBGE = "3550308",
                CprfCnpjUsuarioPagador = "98765432100",
                ContaUsuarioPagador = "123456",
                AgenciaUsuarioPagador = "0001",
                ParticipanteDoUsuarioPagador = "Banco ABC",
                NomeDevedor = "Maria Oliveira",
                CpfCnpjDevedor = "11223344556",
                NumeroContrato = "CONTRATO789",
                DescObjetoContrato = "Serviço de assinatura mensal",
                CodigoSituacaoCancelamentoRecorrencia = "N/A",
                DataHoraCriacaoRecorr = DateTime.Parse("2025-05-01T10:00:00"),
                DataUltimaAtualizacao = DateTime.Parse("2025-05-10T15:30:00"),
                FlagPermiteNotificacao = "Sim",
                FlagValorMaximoAutorizado = "Não",
                TpTentativa = "Automática"
            };
        }
    }
}
