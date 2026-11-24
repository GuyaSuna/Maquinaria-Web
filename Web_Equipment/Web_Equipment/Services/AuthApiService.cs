using Web_Equipment.DTO_s;

namespace Web_Equipment.Services;

public class AuthApiService
{
    private readonly HttpClient _http;
    private readonly AuthSession _session;

    public AuthApiService(
        HttpClient http,
        AuthSession session)
    {
        _http = http;
        _session = session;
    }

    public async Task<bool> LoginAsync(LoginRequestDto dto)
    {
        var response = await _http.PostAsJsonAsync(
            "api/auth/login",
            dto
        );

        if (!response.IsSuccessStatusCode)
            return false;

        var resultado = await response.Content
            .ReadFromJsonAsync<LoginResponseDto>();

        if (resultado == null)
            return false;

        _session.IniciarSesion(resultado);

        return true;
    }

    public void Logout()
    {
        _session.CerrarSesion();
    }
}
