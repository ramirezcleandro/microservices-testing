using Joseco.DDD.Core.Abstractions;
using Logistica.Application.RutaDistribucion.MarcarPuntoEntregado;
using LogisticaService.Domain.Repositories;
using LogisticaService.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Application.Tests.RutaDistribucion.MarcarPuntoEntregado
{
    public class MarcarPuntoEntregadoHandlerTests
    {
        private readonly Mock<IRutaDistribucionRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly MarcarPuntoEntregadoHandler _handler;

        public MarcarPuntoEntregadoHandlerTests()
        {
            _repoMock = new Mock<IRutaDistribucionRepository>();
            _uowMock = new Mock<IUnitOfWork>();

            _handler = new MarcarPuntoEntregadoHandler(
                _repoMock.Object,
                _uowMock.Object
            );
        }

        [Fact]
        public async Task Handle_PaqueteValido_DeberiaMarcarPuntoEntregado()
        {
            // Arrange
            var rutaId = Guid.NewGuid();
            var paqueteId = Guid.NewGuid();

            var ruta = new LogisticaService.Domain.Agregados.RutaDistribucion(
                rutaId,
                new DateOnly(2025, 1, 20),
                Guid.NewGuid(),
                new DireccionGeolocalizada("Almacén", -12.05, -77.04)
            );

            // Cumplimos reglas del dominio
            ruta.AgregarPaquete(paqueteId);
            ruta.Iniciar();

            _repoMock
                .Setup(r => r.GetByIdAsync(rutaId, It.IsAny<bool>()))
                .ReturnsAsync(ruta);

            _uowMock
                .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new MarcarPuntoEntregadoCommand(rutaId, paqueteId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(rutaId, result.Value);

            _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_RutaNoExiste_DeberiaRetornarNotFound()
        {
            // Arrange
            var rutaId = Guid.NewGuid();
            var paqueteId = Guid.NewGuid();

            _repoMock
                .Setup(r => r.GetByIdAsync(rutaId, It.IsAny<bool>()))
                .ReturnsAsync((LogisticaService.Domain.Agregados.RutaDistribucion)null);

            var command = new MarcarPuntoEntregadoCommand(rutaId, paqueteId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Ruta.NoEncontrada", result.Error.Code);

            _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_PaqueteNoPerteneceARuta_DeberiaRetornarFailure()
        {
            // Arrange
            var rutaId = Guid.NewGuid();
            var paqueteId = Guid.NewGuid();

            var ruta = new LogisticaService.Domain.Agregados.RutaDistribucion(
                rutaId,
                new DateOnly(2025, 1, 20),
                Guid.NewGuid(),
                new DireccionGeolocalizada("Almacén", -12.05, -77.04)
            );

            // Ruta válida, pero SIN ese paquete
            ruta.AgregarPaquete(Guid.NewGuid());
            ruta.Iniciar();

            _repoMock
                .Setup(r => r.GetByIdAsync(rutaId, It.IsAny<bool>()))
                .ReturnsAsync(ruta);

            var command = new MarcarPuntoEntregadoCommand(rutaId, paqueteId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Ruta.EntregaPuntoError", result.Error.Code);

            _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }



    }
}
