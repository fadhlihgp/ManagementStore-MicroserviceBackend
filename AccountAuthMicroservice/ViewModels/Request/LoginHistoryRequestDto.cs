namespace AccountAuthMicroservice.ViewModels.Request;

public class LoginHistoryRequestDto
{
    public string AccountId { get; set; }
    public DateTime LastLogin{ get; set; }
}