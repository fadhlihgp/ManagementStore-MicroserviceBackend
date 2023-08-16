namespace AccountAuthMicroservice.ViewModels.Request;

public class AccountRequestDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public string NoHp { get; set; }
    public string RoleId { get; set; }
    public string MemberId { get; set; }
}