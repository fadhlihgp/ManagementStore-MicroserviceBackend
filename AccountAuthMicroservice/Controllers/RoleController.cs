using System.Net;
using AccountAuthMicroservice.Services;
using AccountAuthMicroservice.ViewModels;
using AccountAuthMicroservice.ViewModels.Request;
using AccountAuthMicroservice.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountAuthMicroservice.Controllers;

[ApiController]
[Authorize]
[Route("api/account/role")]
public class RoleController : ControllerBase
{
    private IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateRole([FromBody] RoleRequestDto roleRequestDto)
    {
        var roleId = User.FindFirst("RoleId")?.Value;
        
        await _roleService.CreateRoleAsync(roleRequestDto, roleId);
        ResultResponseDto result = new ResultResponseDto
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Berhasil membuat role",
            Data = null
        };
        return Created("api/account/create", result);
    }

    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> UpdateRole([FromBody] RoleRequestDto roleRequestDto)
    {
        var roleId = User.FindFirst("RoleId")?.Value;
        
        await _roleService.UpdateRole(roleRequestDto.Id, roleRequestDto.Name, roleId);
        ResultResponseDto result = new ResultResponseDto
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Berhasil memperbarui data role",
            Data = null
        };
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> ListRole()
    {
        var roleId = User.FindFirst("RoleId")?.Value;
        
        ResultResponseDto result = new ResultResponseDto
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Berhasil mendapatkan data role",
            Data = await _roleService.ListRole(roleId)
        };
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> FindRoleById([FromRoute] string id)
    {
        ResultResponseDto result = new ResultResponseDto
        {
            StatusCode = 200,
            Message = "Berhasil mendapatkan data role",
            Data = await _roleService.FindRoleById(id)
        };
        return Ok(result);
    }
}