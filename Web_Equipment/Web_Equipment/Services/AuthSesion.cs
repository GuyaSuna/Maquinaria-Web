using Web_Equipment.DTO_s;

namespace Web_Equipment.Services;

public class AuthSession
{
    public string? Token { get; private set; }
    public string? Nombre { get; private set; }
    public string? Rol { get; private set; }
    public DateTime Expira { get; private set; }

    public bool EstaAutenticado => Token != null && DateTime.UtcNow < Expira;
        

    public void IniciarSesion(LoginResponseDto resultado)
    {
        Token = resultado.AccessToken;
        Nombre = resultado.Nombre;
        Rol = resultado.Rol;
        Expira = resultado.ExpiresAtUtc;
    }

    public void CerrarSesion()
    {
        Token = null;
        Nombre = null;
        Rol = null;
        Expira = default;
    }
}
