using AccountAuthMicroservice.Entities;
using AccountAuthMicroservice.ViewModels;
using AccountAuthMicroservice.ViewModels.Request;

namespace AccountAuthMicroservice.Services;

public interface IRoleService
{
    Task CreateRoleAsync(RoleRequestDto roleRequestDto, string roleId);
    Task UpdateRole(string id, string name, string roleId);
    Task<List<RoleRequestDto>> ListRole(string roleId);
    Task<RoleRequestDto> FindRoleById(string id);
}