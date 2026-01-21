using Joseco.DDD.Core.Abstractions;
using Logistica.Application.RutaDistribucion.IniciarRuta;
using LogisticaService.Domain.Repositories;
using LogisticaService.Domain.ValueObjects;
using LogisticaService.Domain.Agregados;
using Moq;

namespace Logistica.Application.Tests.RutaDistribucion.IniciarRuta
{
    public class IniciarRutaHandlerTests
    {

        private readonly Mock<IRutaDistribucionRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly IniciarRutaHandler _handler;

        public IniciarRutaHandlerTests()
        {
            _repoMock = new Mock<IRutaDistribucionRepository>();
            _uowMock = new Mock<IUnitOfWork>();

            _handler = new IniciarRutaHandler(
                _repoMock.Object,
                _uowMock.Object
            );
        }

        [Fact]
        public async Task Handle_RutaValida_DeberiaIniciarRutaCorrectamente()
        {
            // Arrange
            var rutaId = Guid.NewGuid();

            var ruta = new LogisticaService.Domain.Agregados.RutaDistribucion(
                rutaId,
                new DateOnly(2025, 1, 20),
                Guid.NewGuid(),
                new DireccionGeolocalizada("Almacén", -12.05, -77.04)
            );

            // 👇 AQUÍ MISMO (en el Arrange)
            ruta.AgregarPaquete(Guid.NewGuid());

            _repoMock
             .Setup(r => r.GetByIdAsync(rutaId, It.IsAny<bool>()))
             .ReturnsAsync(ruta);


            _uowMock
                .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new IniciarRutaCommand(rutaId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(rutaId, result.Value);

            _repoMock.Verify(r => r.GetByIdAsync(rutaId, It.IsAny<bool>()), Times.Once);
            _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_RutaNoExiste_DeberiaRetornarNotFound()
        {
            // Arrange
            var rutaId = Guid.NewGuid();

            _repoMock
                .Setup(r => r.GetByIdAsync(rutaId, It.IsAny<bool>()))
                .ReturnsAsync((LogisticaService.Domain.Agregados.RutaDistribucion)null);

            var command = new IniciarRutaCommand(rutaId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Ruta.NoEncontrada", result.Error.Code);

            _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_RutaYaIniciada_DeberiaRetornarFailure()
        {
            // Arrange
            var rutaId = Guid.NewGuid();

            var ruta = new LogisticaService.Domain.Agregados.RutaDistribucion(
                rutaId,
                new DateOnly(2025, 1, 20),
                Guid.NewGuid(),
                new DireccionGeolocalizada("Almacén", -12.05, -77.04)
            );

            // Cumplimos reglas del dominio
            ruta.AgregarPaquete(Guid.NewGuid());
            ruta.Iniciar(); // ahora la ruta YA NO está en estado Creada

            _repoMock
                .Setup(r => r.GetByIdAsync(rutaId, It.IsAny<bool>()))
                .ReturnsAsync(ruta);

            var command = new IniciarRutaCommand(rutaId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Ruta.IniciarError", result.Error.Code);

            _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }




    }
}
