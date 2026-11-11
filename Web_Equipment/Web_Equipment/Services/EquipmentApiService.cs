using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using Web_Equipment.DTO_s;
using Web_Equipment.Models;

namespace Web_Equipment.Services
{
    public class EquipmentApiService
    {
        private readonly HttpClient _http;

        public EquipmentApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Equipment>> GetAllAsync()
        {
            var equipos = await _http.GetFromJsonAsync<List<Equipment>>
                ("api/equipment");
                
            return equipos ?? new List<Equipment>();
        }

        public async Task<Equipment?> GetByIdAsync(string id)
        {
            var equipo = await _http.GetFromJsonAsync<Equipment>
                ($"api/equipment/{id}");
            return equipo;
        }


        public async Task<Equipment> PostEquipment(Equipment eq)
        {
            var response = await _http.PostAsJsonAsync(
                "api/equipment", eq);

            response.EnsureSuccessStatusCode();

            var equipoCreado = await response.Content
                .ReadFromJsonAsync<Equipment>();

            return equipoCreado ?? throw new Exception("No se pudo obtener el equipo.");
        }


        public async Task UpdateEquipment (string id,Equipment eq)
        {
            var response = await _http.PutAsJsonAsync($"api/equipment/{id}", eq);
            response.EnsureSuccessStatusCode();
        }


        public async Task DeleteAsync(string id)
        {
         var response = await _http.DeleteAsync($"api/equipment/{id}");
         response.EnsureSuccessStatusCode();
        }


        public async Task UploadImageAsync(
            string id,
            IBrowserFile imagen)
        {
            using var formulario = new MultipartFormDataContent();

            using var stream = imagen.OpenReadStream(
                maxAllowedSize: 5 * 1024 * 1024);

            using var contenido = new StreamContent(stream);

            contenido.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(imagen.ContentType);
                  

            formulario.Add(contenido, "file", imagen.Name);

            var response = await _http.PostAsync(
                $"api/equipment/{id}/imagen",
                formulario);

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
                    $"nombre={Uri.EscapeDataString(nombre)}");
            }

            if (!string.IsNullOrWhiteSpace(estado))
            {
                parametros.Add(
                    $"estado={Uri.EscapeDataString(estado)}");
            }

            if (!string.IsNullOrWhiteSpace(ubicacion))
            {
                parametros.Add(
                    $"ubicacion={Uri.EscapeDataString(ubicacion)}");
            }

            parametros.Add($"pagina={pagina}");
            parametros.Add($"tamano={tamano}");

            string queryString = string.Join("&", parametros);

            var resultado = await _http.GetFromJsonAsync<PaginaResultadoDto>(
                $"api/equipment/buscar?{queryString}");

            return resultado ?? new PaginaResultadoDto();
        }


    }
}