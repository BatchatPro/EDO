using EDO.Access.DTO;
using EDO.Access.Mapper ;
using EDO.Access.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDO.API.Controllers;
[Authorize(Roles = "admin")]
[Route("api/[controller]")]
[ApiController]
public class AdmintrationController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AdmintrationController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        this._userManager = userManager;
        this._roleManager = roleManager;
    }

    [HttpPost]
    [Route("User/CreateUser")]
    public async Task<IActionResult> CreateAccount([FromBody] RegistrationDTO userDTO)
    {
        try
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                PhoneNumber = userDTO.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (!result.Succeeded)
                throw new BadHttpRequestException(result.Errors.Select(x => x.Description).ToString());

            return Ok(userDTO);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("User/AddRoleToUser")]
    public async Task<IActionResult> AddRoleToUser([FromBody] UserRolesDTO userRolesDTO)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userRolesDTO.UserId);
            IList<string> addingRoles = new List<string>();
            foreach (var role in userRolesDTO.Roles)
                addingRoles.Add(role.Name);

            var result = await _userManager.AddToRolesAsync(user, addingRoles);

            if (!result.Succeeded)
                throw new BadHttpRequestException(result.Errors.Select(x => x.Description).ToString());
            var userRoles = await _userManager.GetRolesAsync(user);

            IList<RoleDTO> rolesDTOs = new List<RoleDTO>();
            foreach (var role in userRoles)
            {
                var userRole = await _roleManager.FindByNameAsync(role);
                rolesDTOs.Add(userRole.ConvertToDTO());
            }

            userRolesDTO.Roles = rolesDTOs;
            return Ok(userRolesDTO);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("User/RemoveRoleFromUser")]
    public async Task<IActionResult> RemoveRoleFromUser(UserRolesDTO userRolesDTO)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userRolesDTO.UserId);

            IList<string> removingRoles = new List<string>();

            foreach (var role in userRolesDTO.Roles)
                removingRoles.Add(role.Name);

            var result = await _userManager.RemoveFromRolesAsync(user, removingRoles);

            if (!result.Succeeded)
                throw new BadHttpRequestException(result.Errors.Select(x => x.Description).ToString());

            var userRoles = await _userManager.GetRolesAsync(user);

            IList<RoleDTO> rolesDTO = new List<RoleDTO>();
            foreach (var role in userRoles)
            {
                var userRole = await _roleManager.FindByNameAsync(role);
                rolesDTO.Add(userRole.ConvertToDTO());
            }

            userRolesDTO.Roles = rolesDTO;
            return Ok(userRolesDTO);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("User/ChangeUser")]
    public async Task<IActionResult> ChangeUser(ApplicationUserDTO userDTO)
    {
        try
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userDTO.Id);
            user.LastName = userDTO.LastName;
            user.FirstName = userDTO.FirstName;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.Email = userDTO.Email;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new { Successful = false, Errors = result.Errors.Select(x => x.Description) });

            return Ok(user.ConvertToDTO);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("User/GetAll")]
    public async Task<IActionResult> GetAll() => Ok((await _userManager.Users.ToListAsync()).ConvertToDTO());

}

