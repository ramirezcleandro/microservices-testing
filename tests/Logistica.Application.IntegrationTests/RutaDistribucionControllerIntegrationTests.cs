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
        public async Task FlujoCompleto_RutaHastaEntrega_OK()
        {
            // 1️⃣ Crear Ruta
            var crearRutaResponse = await _httpClient.PostAsJsonAsync(
                "/api/RutaDistribucion/crear",
                new
                {
                    fecha = DateOnly.FromDateTime(DateTime.Today),
                    personalEntregaId = Guid.NewGuid(),
                    direccionAlmacen = "Av. Siempre Viva 123",
                    latitud = -12.0464,
                    longitud = -77.0428
                });

            crearRutaResponse.EnsureSuccessStatusCode();
            var rutaId = await crearRutaResponse.Content.ReadFromJsonAsync<Guid>();
            rutaId.Should().NotBeEmpty();

            // 2️⃣ Agregar Paquete
            var paqueteId = Guid.NewGuid();

            var agregarPaqueteResponse = await _httpClient.PostAsJsonAsync(
                 $"/api/RutaDistribucion/{rutaId}/agregar-paquete",
                 new
                 {
                     paqueteId = paqueteId
                 });

            agregarPaqueteResponse.EnsureSuccessStatusCode();

            // 3️⃣ Iniciar Ruta
            var iniciarRutaResponse = await _httpClient.PostAsync(
                $"/api/RutaDistribucion/{rutaId}/iniciar",
                null);

            iniciarRutaResponse.EnsureSuccessStatusCode();

            // 4️⃣ Marcar Punto Entregado
            var marcarEntregadoResponse = await _httpClient.PostAsync(
                $"/api/RutaDistribucion/{rutaId}/puntos/{paqueteId}/entregado",
                null);


            marcarEntregadoResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Flujo_CrearAgregarPaqueteEIniciarRuta_OK()
        {
            // Crear ruta
            var crearResponse = await _httpClient.PostAsJsonAsync(
                "/api/RutaDistribucion/crear",
                new
                {
                    fecha = DateOnly.FromDateTime(DateTime.Today),
                    personalEntregaId = Guid.NewGuid(),
                    direccionAlmacen = "Av. Test 123",
                    latitud = -12,
                    longitud = -77
                });

            crearResponse.EnsureSuccessStatusCode();
            var rutaId = await crearResponse.Content.ReadFromJsonAsync<Guid>();

            // Agregar paquete (OBLIGATORIO)
            var paqueteId = Guid.NewGuid();

            var agregarResponse = await _httpClient.PostAsJsonAsync(
                $"/api/RutaDistribucion/{rutaId}/agregar-paquete",
                new { paqueteId });

            agregarResponse.EnsureSuccessStatusCode();

            // Iniciar ruta
            var iniciarResponse = await _httpClient.PostAsync(
                $"/api/RutaDistribucion/{rutaId}/iniciar",
                null);

            iniciarResponse.EnsureSuccessStatusCode();
        }




    }
}