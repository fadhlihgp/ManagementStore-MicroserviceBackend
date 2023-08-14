using System.ComponentModel.DataAnnotations;

namespace AccountAuthMicroservice.ViewModels.Request;

public class RegisterStoreRequestDto
{
    public string NameStore { get; set; }
    public string AddressStore { get; set; }
    
    public string NameMember { get; set; }
    public string AddressMember { get; set; }
    public string IdentityNumberMember { get; set; }
    public string UserName { get; set; }
    [EmailAddress(ErrorMessage = "Format email tidak valid")]
    public string Email { get; set; }
    public string Password { get; set; }
    public string NoHp { get; set; }
}