
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
        using var response = await _http.PostAsync(
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


    public async Task<List<PagoRevision>> ObtenerPagosAsync(
        string equipmentId)
    {
        var pagos = await _http
            .GetFromJsonAsync<List<PagoRevision>>(
                $"api/pagos/equipment/{equipmentId}"
            );

        return pagos ?? new List<PagoRevision>();
    }

    public async Task<PagoRevision> ActualizarEstadoAsync(
        int pagoId)
    {
        using var response = await _http.PostAsync(
            $"api/pagos/{pagoId}/actualizar-estado",
            null
        );

        response.EnsureSuccessStatusCode();

        var pago = await response.Content.ReadFromJsonAsync<PagoRevision>();
            
        return pago ?? throw new Exception(
            "La api no devolvió el estado actualizado."
        );
    }
}
