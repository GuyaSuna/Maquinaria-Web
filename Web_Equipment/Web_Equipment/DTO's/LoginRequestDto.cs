
 using System.ComponentModel.DataAnnotations;
namespace Web_Equipment.DTO_s
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; } = "";
    }
}