
using System.Net.Http.Json;
using Web_Equipment.Models;

namespace Web_Equipment.Services;

public class PagoApiService
{
    private readonly HttpClient _http;

    public PagoApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> CrearPagoAsync(string equipmentId)
    {
        var response = await _http.PostAsync(
            $"api/pagos/crear/{equipmentId}",
            null
        );

        response.EnsureSuccessStatusCode();

        var pago = await response.Content
            .ReadFromJsonAsync<PagoResponse>();

        if (string.IsNullOrWhiteSpace(pago?.Url))
        {
            throw new Exception(
                "La API no devolvió una URL de pago."
            );
        }

        return pago.Url;
    }
}
