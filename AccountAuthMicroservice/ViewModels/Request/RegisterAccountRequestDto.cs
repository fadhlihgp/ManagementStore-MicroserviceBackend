using System.ComponentModel.DataAnnotations;

namespace AccountAuthMicroservice.ViewModels.Request;

public class RegisterAccountRequestDto
{
    [Required(ErrorMessage = "Kolom id identitas tidak boleh kosong")]
    public string IdentityNumber { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string UserName { get; set; }
    public string NoHp { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}