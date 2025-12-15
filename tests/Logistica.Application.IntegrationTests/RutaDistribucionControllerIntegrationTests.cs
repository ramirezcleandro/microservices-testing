using FluentAssertions;
using Logistica.Application.IntegrationTests.Factories;
using System.Net;
using System.Net.Http.Json;

namespace Logistica.Application.IntegrationTests
{
    public class RutaDistribucionControllerIntegrationTests
    {
        private HttpClient _httpClient;
        public RutaDistribucionControllerIntegrationTests() {
            _httpClient = HttpClientFactory.createClient();
        }
        [Fact]
        public async Task CrearRuta_OK()
        {
            // Arrange (JSON alineado a CrearRutaCommand)
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "/api/RutaDistribucion/crear")
            {
                Content = JsonContent.Create(new
                {
                    fecha = DateOnly.FromDateTime(DateTime.Today),
                    personalEntregaId = Guid.NewGuid(),
                    direccionAlmacen = "Av. Siempre Viva 123",
                    latitud = -12.0464,
                    longitud = -77.0428
                })
            };

           
            var response = await _httpClient.SendAsync(request);

            
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            
            var guid = await response.Content.ReadFromJsonAsync<Guid>();

            guid.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CrearRuta_DeberiaRetornarBadRequest_CuandoDatosSonInvalidos()
        {
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "/api/RutaDistribucion/crear")
            {
                Content = JsonContent.Create(new
                {
                    fecha = "0001-01-01",
                    personalEntregaId = Guid.Empty,
                    direccionAlmacen = "",
                    latitud = 0,
                    longitud = 0
                })
            };

            var response = await _httpClient.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}