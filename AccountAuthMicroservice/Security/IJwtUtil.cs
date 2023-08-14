using AccountAuthMicroservice.Entities;

namespace AccountAuthMicroservice.Security;

public interface IJwtUtil
{
    string GenerateToken(Account account);
}