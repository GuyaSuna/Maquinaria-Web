
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web_Equipment.DTO_s;
using Web_Equipment.Models;

namespace Web_Equipment.Services
{
    public class EquipmentApiService
    {
        private readonly HttpClient _http;
        private readonly AuthSession _session;

        public EquipmentApiService(
            HttpClient http,
            AuthSession session)
        {
            _http = http;
            _session = session;
        }

        private HttpRequestMessage CrearPeticion(
            HttpMethod metodo,
            string url)
        {
            if (!_session.EstaAutenticado)
            {
                throw new UnauthorizedAccessException(
                    "Tenes que iniciar sesión.");
            }

            var request = new HttpRequestMessage(
                metodo,
                url
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _session.Token
                );

            return request;
        }

        public async Task<List<Equipment>> GetAllAsync()
        {
            using var request = CrearPeticion(
                HttpMethod.Get,
                "api/equipment"
            );

            using var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var equipos = await response.Content
                .ReadFromJsonAsync<List<Equipment>>();

            return equipos ?? new List<Equipment>();
        }

        public async Task<Equipment?> GetByIdAsync(string id)
        {
            using var request = CrearPeticion(
                HttpMethod.Get,
                $"api/equipment/{id}"
            );

            using var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<Equipment>();
        }

        public async Task<Equipment> PostEquipment(Equipment eq)
        {
            using var request = CrearPeticion(
                HttpMethod.Post,
                "api/equipment"
            );

            request.Content = JsonContent.Create(eq);

            using var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var equipoCreado = await response.Content
                .ReadFromJsonAsync<Equipment>();

            return equipoCreado
                ?? throw new Exception(
                    "No se pudo obtener el equipo.");
        }

        public async Task UpdateEquipment(
            string id,
            Equipment eq)
        {
            using var request = CrearPeticion(
                HttpMethod.Put,
                $"api/equipment/{id}"
            );

            request.Content = JsonContent.Create(eq);

            using var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string id)
        {
            using var request = CrearPeticion(
                HttpMethod.Delete,
                $"api/equipment/{id}"
            );

            using var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }


        public async Task UploadImageAsync(
            string id,
            IBrowserFile imagen)
        {
            using var request = CrearPeticion(
                HttpMethod.Post,
                $"api/equipment/{id}/imagen"
            );

            using var formulario = new MultipartFormDataContent();

            using var stream = imagen.OpenReadStream(
                maxAllowedSize: 5 * 1024 * 1024
            );

            using var contenido = new StreamContent(stream);

            contenido.Headers.ContentType =
                new MediaTypeHeaderValue(
                    imagen.ContentType
                );

            formulario.Add(
                contenido,
                "file",
                imagen.Name
            );

            request.Content = formulario;

            using var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        public async Task<PaginaResultadoDto> BuscarAsync(
            string? nombre,
            string? estado,
            string? ubicacion,
            int pagina,
            int tamano)
        {
            var parametros = new List<string>();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                parametros.Add(
                    $"nombre={Uri.EscapeDataString(nombre)}"
                );
            }

            if (!string.IsNullOrWhiteSpace(estado))
            {
                parametros.Add(
                    $"estado={Uri.EscapeDataString(estado)}"
                );
            }

            if (!string.IsNullOrWhiteSpace(ubicacion))
            {
                parametros.Add(
                    $"ubicacion={Uri.EscapeDataString(ubicacion)}"
                );
            }

            parametros.Add($"pagina={pagina}");
            parametros.Add($"tamano={tamano}");

            string queryString = string.Join("&", parametros);

            using var request = CrearPeticion(
                HttpMethod.Get,
                $"api/equipment/buscar?{queryString}"
            );

            using var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var resultado = await response.Content
                .ReadFromJsonAsync<PaginaResultadoDto>();

            return resultado ?? new PaginaResultadoDto();
        }
    }
}
