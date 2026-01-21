using Joseco.DDD.Core.Abstractions;
using Logistica.Application.RutaDistribucion.OptimizarRuta;
using LogisticaService.Domain.Repositories;
using LogisticaService.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Application.Tests.RutaDistribucion.OptimizarRuta
{
    public class OptimizarRutaHandlerTests
    {
        private readonly Mock<IRutaDistribucionRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly OptimizarRutaHandler _handler;
        public OptimizarRutaHandlerTests()
        {
            _repoMock = new Mock<IRutaDistribucionRepository>();
            _uowMock = new Mock<IUnitOfWork>();

            _handler = new OptimizarRutaHandler(
                _repoMock.Object,
                _uowMock.Object
            );
        }

        [Fact]
        public async Task Handle_RutaValida_DeberiaOptimizarRutaCorrectamente()
        {
            // Arrange
            var rutaId = Guid.NewGuid();

            var ruta = new LogisticaService.Domain.Agregados.RutaDistribucion(
                rutaId,
                new DateOnly(2025, 1, 20),
                Guid.NewGuid(),
                new DireccionGeolocalizada("Almacén", -12.05, -77.04)
            );

            // Preparar dominio
            var paquete1 = Guid.NewGuid();
            var paquete2 = Guid.NewGuid();

            ruta.AgregarPaquete(paquete1);
            ruta.AgregarPaquete(paquete2);

            var nuevoOrden = new Dictionary<Guid, int>
            {
                { paquete1, 2 },
                { paquete2, 1 }
            };

            _repoMock
                .Setup(r => r.GetByIdAsync(rutaId, It.IsAny<bool>()))
                .ReturnsAsync(ruta);

            _uowMock
                .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new OptimizarRutaCommand(rutaId, nuevoOrden);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(rutaId, result.Value);

            _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
