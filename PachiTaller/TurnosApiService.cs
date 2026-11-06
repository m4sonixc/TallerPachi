using System.Net.Http.Json;
using System.Text.Json;

namespace PachiTaller
{
    public class TurnosApiService
    {
        private readonly HttpClient _httpClient;

        // Reemplaza esta URL con la que te muestra la consola (HTTPS)
        private const string BaseUrl = "https://localhost:7287/api/";

        public TurnosApiService()
        {
            // Configuración para saltar la validación del certificado en pruebas locales (Android/Windows)
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            _httpClient = new HttpClient(handler) { BaseAddress = new Uri(BaseUrl) };
        }

        public async Task<List<TurnoModel>> ObtenerTurnosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("turnos");
                if (response.IsSuccessStatusCode)
                {
                    var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return await response.Content.ReadFromJsonAsync<List<TurnoModel>>(opciones) ?? new List<TurnoModel>();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Error HTTP: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al conectar a la API: {ex.Message}");
            }

            return new List<TurnoModel>();
        }
    }
}