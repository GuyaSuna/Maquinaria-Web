namespace Web_Equipment.DTO_s;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = "";
    public DateTime ExpiresAtUtc { get; set; }
    public string Nombre { get; set; } = "";
    public string Rol { get; set; } = "";
}
