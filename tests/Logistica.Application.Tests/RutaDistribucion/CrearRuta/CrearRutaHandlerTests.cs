using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using Logistica.Application.RutaDistribucion.CrearRuta;
using LogisticaService.Domain.Repositories;
using Moq;


namespace Logistica.Application.Tests.RutaDistribucion.CrearRuta
{
    public class CrearRutaHandlerTests
    {
        private readonly Mock<IRutaDistribucionRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly CrearRutaHandler _handler;

        public CrearRutaHandlerTests()
        {
            _repoMock = new Mock<IRutaDistribucionRepository>();
            _uowMock = new Mock<IUnitOfWork>();

            _handler = new CrearRutaHandler(
                _repoMock.Object,
                _uowMock.Object
            );
        }

        [Fact]
        public async Task Handle_DeberiaCrearRutaCorrectamente()
        {
            // Arrange
            var command = new CrearRutaCommand(
                new DateOnly(2025, 1, 20),
                Guid.NewGuid(),
                "Almacén Central",
                -12.05,
                -77.04
            );

            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<LogisticaService.Domain.Agregados.RutaDistribucion>()
))
                .Returns(Task.CompletedTask);

            _uowMock
                .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsType<Result<Guid>>(result);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<LogisticaService.Domain.Agregados.RutaDistribucion>()),
                Times.Once
            );

            _uowMock.Verify(
                u => u.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }
}
