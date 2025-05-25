using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using Pay.Recorrencia.Gestao.Application.Services;

namespace Pay.Recorrencia.Gestao.Test
{
    // Interface do serviço (simplificada para contexto)
    public interface IJornadaService
    {
        Task<ListaJornadaPaginada<JornadaList>> GetAllAsync(JornadaDTO request);
        Task<JornadaNonPagination> GetByTpJornadaAndIdRecorrenciaAsync(JornadaDTO request);
        Task<ListaJornadaPaginada<Jornada>> GetByAnyFilterAsync(JornadaDTO request);
        Task<JornadaNonPagination> GetByTpJornadaAndIdE2EAsync(JornadaDTO request);
    }

    // Implementação mínima para testes
    public class JornadaService : IJornadaService
    {
        private readonly IJornadaRepository _repo;

        public JornadaService(IJornadaRepository repo)
        {
            _repo = repo;
        }

        public async Task<ListaJornadaPaginada<JornadaList>> GetAllAsync(JornadaDTO request)
        {
            return await _repo.GetAllAsync(request);
        }

        public async Task<JornadaNonPagination> GetByTpJornadaAndIdRecorrenciaAsync(JornadaDTO request)
        {
            return await _repo.GetByTpJornadaAndIdRecorrenciaAsync(
                new JornadaAutorizacaoDTO
                {
                    TpJornada = request.TpJornada,
                    IdRecorrencia = request.IdRecorrencia
                });
        }

        public async Task<ListaJornadaPaginada<Jornada>> GetByAnyFilterAsync(JornadaDTO request)
        {
            return await _repo.GetByAnyFilterAsync(
                new JornadaAutorizacaoAgendamentoDTO
                {
                    TpJornada = request.TpJornada,
                    IdRecorrencia = request.IdRecorrencia,
                    IdE2E = request.IdE2E,
                    IdConciliacaoRecebedor = request.IdConciliacaoRecebedor
                });
        }

        public async Task<JornadaNonPagination> GetByTpJornadaAndIdE2EAsync(JornadaDTO request)
        {
            return await _repo.GetByTpJornadaAndIdE2EAsync(
                new JornadaAgendamentoDTO
                {
                    TpJornada = request.TpJornada,
                    IdE2E = request.IdE2E
                });
        }
    }

    public class JornadaServiceTests
    {
        private readonly Mock<IJornadaRepository> _repoMock;
        private readonly IJornadaService _service;

        public JornadaServiceTests()
        {
            _repoMock = new Mock<IJornadaRepository>();
            _service = new JornadaService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsPagedList()
        {
            var listaMock = new List<JornadaList>
            {
                new JornadaList { TpJornada = "Tipo1", IdRecorrencia = "Rec1" },
                new JornadaList { TpJornada = "Tipo2", IdRecorrencia = "Rec2" }
            };

            var pagedMock = new ListaJornadaPaginada<JornadaList>
            {
                Items = listaMock,
                TotalItems = listaMock.Count
            };

            _repoMock.Setup(r => r.GetAllAsync(It.IsAny<JornadaDTO>()))
                .ReturnsAsync(pagedMock);

            var result = await _service.GetAllAsync(new JornadaDTO());

            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Equal(pagedMock.TotalItems, result.TotalItems);
            Assert.Equal(listaMock.Count, result.Items.Count());
        }

        [Fact]
        public async Task GetByTpJornadaAndIdRecorrenciaAsync_Existing_ReturnsJornada()
        {
            var entityMock = new Jornada
            {
                TpJornada = "Jornada1",
                IdRecorrencia = "Rec001"
            };

            _repoMock.Setup(r => r.GetByTpJornadaAndIdRecorrenciaAsync(It.IsAny<JornadaAutorizacaoDTO>()))
                .ReturnsAsync(new JornadaNonPagination { Data = entityMock });

            var result = await _service.GetByTpJornadaAndIdRecorrenciaAsync(new JornadaDTO { TpJornada = "Jornada1", IdRecorrencia = "Rec001" });

            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(entityMock.TpJornada, result.Data.TpJornada);
            Assert.Equal(entityMock.IdRecorrencia, result.Data.IdRecorrencia);
        }

        [Fact]
        public async Task GetByAnyFilterAsync_WithResults_ReturnsPagedList()
        {
            var listaMock = new List<Jornada>
            {
                new Jornada { TpJornada = "TipoX" },
                new Jornada { TpJornada = "TipoY" }
            };

            var pagedMock = new ListaJornadaPaginada<Jornada>
            {
                Items = listaMock,
                TotalItems = listaMock.Count
            };

            _repoMock.Setup(r => r.GetByAnyFilterAsync(It.IsAny<JornadaAutorizacaoAgendamentoDTO>()))
                .ReturnsAsync(pagedMock);

            var result = await _service.GetByAnyFilterAsync(new JornadaDTO());

            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Equal(2, result.Items.Count());
        }

        [Fact]
        public async Task GetByAnyFilterAsync_NoResults_ReturnsEmptyList()
        {
            var pagedMock = new ListaJornadaPaginada<Jornada>
            {
                Items = new List<Jornada>(),
                TotalItems = 0
            };

            _repoMock.Setup(r => r.GetByAnyFilterAsync(It.IsAny<JornadaAutorizacaoAgendamentoDTO>()))
                .ReturnsAsync(pagedMock);

            var result = await _service.GetByAnyFilterAsync(new JornadaDTO());

            Assert.NotNull(result);
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalItems);
        }

        [Fact]
        public async Task GetByTpJornadaAndIdE2EAsync_Existing_ReturnsJornada()
        {
            var entityMock = new Jornada
            {
                TpJornada = "AGND",
                IdE2E = "E999"
            };

            _repoMock.Setup(r => r.GetByTpJornadaAndIdE2EAsync(It.IsAny<JornadaAgendamentoDTO>()))
                .ReturnsAsync(new JornadaNonPagination { Data = entityMock });

            var result = await _service.GetByTpJornadaAndIdE2EAsync(new JornadaDTO { TpJornada = "AGND", IdE2E = "E999" });

            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(entityMock.TpJornada, result.Data.TpJornada);
            Assert.Equal(entityMock.IdE2E, result.Data.IdE2E);
        }

        [Fact]
        public async Task GetByTpJornadaAndIdE2EAsync_NotFound_ReturnsNullData()
        {
            _repoMock.Setup(r => r.GetByTpJornadaAndIdE2EAsync(It.IsAny<JornadaAgendamentoDTO>()))
                .ReturnsAsync(new JornadaNonPagination { Data = null });

            var result = await _service.GetByTpJornadaAndIdE2EAsync(new JornadaDTO { TpJornada = "X", IdE2E = "Y" });

            Assert.NotNull(result);
            Assert.Null(result.Data);
        }
    }
}
