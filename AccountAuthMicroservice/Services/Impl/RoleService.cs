using AccountAuthMicroservice.Models;
using AccountAuthMicroservice.Exceptions;
using AccountAuthMicroservice.Repositories;
using AccountAuthMicroservice.Repositories.Interface;
using AccountAuthMicroservice.ViewModels.Request;

namespace AccountAuthMicroservice.Services.Impl;

public class RoleService : IRoleService
{
    private IRepository<Role> _roleRepository;
    private IPersistence _persistence;

    public RoleService(IRepository<Role> roleRepository, IPersistence persistence)
    {
        _roleRepository = roleRepository;
        _persistence = persistence;
    }

    public async Task CreateRoleAsync(RoleRequestDto roleRequestDto, string roleId)
    {
        if (!roleId.Equals("1")) throw new UnauthorizedException("Akses ditolak");
        
        var findById = await _roleRepository.FindById(roleRequestDto.Id);
        if (findById != null) throw new BadRequestException("Gagal membuat role, id sudah tersedia");
        
        Role role = new Role
        {
            Id = roleRequestDto.Id,
            Name = roleRequestDto.Name
        };

        await _roleRepository.Save(role);
        await _persistence.SaveChangesAsync();
    }

    public async Task UpdateRole(string id, string name, string roleId)
    {
        if (!roleId.Equals("1")) throw new UnauthorizedException("Akses ditolak");
        
        var findById = await _roleRepository.FindById(id);
        if (findById == null) throw new NotFoundException("Role id tidak ditemukan");

        findById.Name = name;
        _roleRepository.Update(findById);
        await _persistence.SaveChangesAsync();
    }

    public async Task<List<RoleRequestDto>> ListRole(string roleId)
    {
        if (!roleId.Equals("1")) throw new UnauthorizedException("Akses ditolak");
        
        var findAll = await _roleRepository.FindAll();
        List<RoleRequestDto> result = new List<RoleRequestDto>();
        foreach (var role in findAll)
        {
            result.Add(new RoleRequestDto{Id = role.Id, Name = role.Name});
        }

        return result;
    }

    public async Task<RoleRequestDto> FindRoleById(string id)
    {
        var role = await _roleRepository.FindById(id);
        if (role == null) throw new NotFoundException("Data role tidak ditemukan");
        return new RoleRequestDto
        {
            Id = role.Id,
            Name = role.Name,
        };
    }
}