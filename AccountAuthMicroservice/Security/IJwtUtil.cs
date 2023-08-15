using AccountAuthMicroservice.Models;

namespace AccountAuthMicroservice.Security;

public interface IJwtUtil
{
    string GenerateToken(Account account);
}