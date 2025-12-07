using Joseco.DDD.Core.Abstractions;
using Logistica.Application.RutaDistribucion.AgregarPaqueteARuta;
using LogisticaService.Domain.Agregados;
using LogisticaService.Domain.Repositories;
using LogisticaService.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Application.Tests.RutaDistribucion.AgregarPaqueteARuta
{
    public class AgregarPaqueteARutaHandlerTests
    {
        private readonly Mock<IRutaDistribucionRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly AgregarPaqueteARutaHandler _handler;

        public AgregarPaqueteARutaHandlerTests()
        {
            _repoMock = new Mock<IRutaDistribucionRepository>();
            _uowMock = new Mock<IUnitOfWork>();

            _handler = new AgregarPaqueteARutaHandler(
                _repoMock.Object,
                _uowMock.Object
            );
        }

        [Fact]
        public async Task Handle_DeberiaAgregarPaqueteCorrectamente()
        {
            // Arrange
            var rutaId = Guid.NewGuid();
            var paqueteId = Guid.NewGuid();

            // simulamos una ruta válida
            var ruta = new LogisticaService.Domain.Agregados.RutaDistribucion(
                rutaId,
                new DateOnly(2025, 1, 20),
                Guid.NewGuid(),
                new DireccionGeolocalizada("A", 1, 1)
            );

            // simulamos que GetByIdAsync devuelve la ruta
            _repoMock
            .Setup(r => r.GetByIdAsync(rutaId, false))
            .ReturnsAsync(ruta);

            // simulamos adicionar punto sin problemas
            _repoMock
                .Setup(r => r.AddPuntoAsync(It.IsAny<PuntoEntrega>()))
                .Returns(Task.CompletedTask);

            _uowMock
                .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new AgregarPaqueteARutaCommand(rutaId, paqueteId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(rutaId, result.Value);

            _repoMock.Verify(r => r.GetByIdAsync(rutaId, false), Times.Once);

            _repoMock.Verify(r => r.AddPuntoAsync(It.IsAny<PuntoEntrega>()), Times.Once);
            _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
