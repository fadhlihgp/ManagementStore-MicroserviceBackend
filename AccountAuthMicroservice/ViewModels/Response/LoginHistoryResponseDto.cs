namespace AccountAuthMicroservice.ViewModels.Response;

public class LoginHistoryResponseDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public string Store { get; set; }
    public string LastLogin { get; set; }
}