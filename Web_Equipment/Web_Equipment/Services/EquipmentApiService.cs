using System.Net.Http.Json;
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
    }
}