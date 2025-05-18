using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Pay.Recorrencia.Gestao.Application.Services;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Test
{
    public class JornadaServiceTests
    {
        private readonly Mock<IJornadaRepository> _repoMock;
        private readonly JornadaService _service;

        public JornadaServiceTests()
        {
            _repoMock = new Mock<IJornadaRepository>();
            _service = new JornadaService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldMapAllFields()
        {
            // Arrange
            var entity = new Jornada
            {
                Id = 1, 
                TpJornada = "JornadaX",
                IdRecorrencia = "Rec123",
                IdE2E = "E2E456",
                IdConciliacaoRecebedor = "Conc789",
                SituacaoJornada = "Ativa",
                DtAgendamento = DateTime.Parse("2025-05-17T08:00:00"),
                VlAgendamento = 100.5m,
                DtPagamento = DateTime.Parse("2025-05-18T09:00:00"),
                DataHoraCriacao = DateTime.Parse("2025-05-15T10:00:00"),
                DataUltimaAtualizacao = DateTime.Parse("2025-05-16T11:00:00")
            };
            _repoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new[] { entity });

            // Act
            var dtos = (await _service.GetAllAsync()).ToList();

            // Assert
            Assert.Single(dtos);
            var dto = dtos[0];
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal(entity.TpJornada, dto.TpJornada);
            Assert.Equal(entity.IdRecorrencia, dto.IdRecorrencia);
            Assert.Equal(entity.IdE2E, dto.IdE2E);
            Assert.Equal(entity.IdConciliacaoRecebedor, dto.IdConciliacaoRecebedor);
            Assert.Equal(entity.SituacaoJornada, dto.SituacaoJornada);
            Assert.Equal(entity.DtAgendamento, dto.DtAgendamento);
            Assert.Equal(entity.VlAgendamento, dto.VlAgendamento);
            Assert.Equal(entity.DtPagamento, dto.DtPagamento);
            Assert.Equal(entity.DataHoraCriacao, dto.DataHoraCriacao);
            Assert.Equal(entity.DataUltimaAtualizacao, dto.DataUltimaAtualizacao);
        }

        [Fact]
        public async Task GetByJornadaERecorrenciaAsync_Existing_ReturnsDto()
        {
            // Arrange
            var entity = new Jornada
            {
                Id = 42,
                TpJornada = "J1",
                IdRecorrencia = "R1"
            };
            _repoMock
                .Setup(r => r.GetByTpJornadaAndIdRecorrenciaAsync("J1", "R1"))
                .ReturnsAsync(entity);

            // Act
            var dto = await _service.GetByJornadaERecorrenciaAsync("J1", "R1");

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal(entity.TpJornada, dto.TpJornada);
            Assert.Equal(entity.IdRecorrencia, dto.IdRecorrencia);
        }

        [Fact]
        public async Task GetByJornadaERecorrenciaAsync_NotExisting_ReturnsNull()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetByTpJornadaAndIdRecorrenciaAsync("J1", "R1"))
                .ReturnsAsync((Jornada)null);

            // Act
            var dto = await _service.GetByJornadaERecorrenciaAsync("J1", "R1");

            // Assert
            Assert.Null(dto);
        }

        [Fact]
        public async Task GetByJornadaE2EAsync_Existing_ReturnsDto()
        {
            // Arrange
            var entity = new Jornada
            {
                Id = 99,
                TpJornada = "AGND",
                IdE2E = "E999"
            };
            _repoMock
                .Setup(r => r.GetByTpJornadaAndIdE2EAsync("AGND", "E999"))
                .ReturnsAsync(entity);

            // Act
            var dto = await _service.GetByJornadaE2EAsync("AGND", "E999");

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal(entity.TpJornada, dto.TpJornada);
            Assert.Equal(entity.IdE2E, dto.IdE2E);
        }

        [Fact]
        public async Task GetByJornadaE2EAsync_NotExisting_ReturnsNull()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetByTpJornadaAndIdE2EAsync("AGND", "E999"))
                .ReturnsAsync((Jornada)null);

            // Act
            var dto = await _service.GetByJornadaE2EAsync("AGND", "E999");

            // Assert
            Assert.Null(dto);
        }

        [Fact]
        public async Task GetByAnyFilterAsync_WithResults_ReturnsList()
        {
            // Arrange
            var entity = new Jornada
            {
                Id = 7,
                TpJornada = "XYZ"
            };
            _repoMock
                .Setup(r => r.GetByAnyFilterAsync("XYZ", "R2", "E3", "C4"))
                .ReturnsAsync(new[] { entity });

            // Act
            var dtos = (await _service.GetByAnyFilterAsync("XYZ", "R2", "E3", "C4")).ToList();

            // Assert
            Assert.Single(dtos);
            Assert.Equal(entity.Id, dtos[0].Id);
            Assert.Equal(entity.TpJornada, dtos[0].TpJornada);
        }

        [Fact]
        public async Task GetByAnyFilterAsync_NoResults_ReturnsEmptyList()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetByAnyFilterAsync("XYZ", "R2", "E3", "C4"))
                .ReturnsAsync(Array.Empty<Jornada>());

            // Act
            var dtos = (await _service.GetByAnyFilterAsync("XYZ", "R2", "E3", "C4")).ToList();

            // Assert
            Assert.Empty(dtos);
        }
    }
}
