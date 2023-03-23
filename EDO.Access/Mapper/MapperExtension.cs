using EDO.Access.DTO;
using EDO.Access.Models;

namespace EDO.Access.Mapper;

public static class MapperExtension
{
    public static ApplicationUserDTO ConvertToDTO(this ApplicationUser applicationUser)
    {
        if (applicationUser == null)
            return null;
        return new ApplicationUserDTO()
        {
            Email = applicationUser.Email,
            FirstName = applicationUser.FirstName,
            Id = applicationUser.Id,
            LastName = applicationUser.LastName,
            PhoneNumber = applicationUser.PhoneNumber,
            UserName = applicationUser.UserName
        };
    }
    public static ApplicationUser ConvertToEntity(this ApplicationUserDTO applicationUserDTO)
    {
        if (applicationUserDTO == null)
            return null;
        return new ApplicationUser()
        {
            Id = applicationUserDTO.Id,
            UserName = applicationUserDTO.UserName,
            LastName = applicationUserDTO.LastName,
            FirstName = applicationUserDTO.FirstName,
            Email = applicationUserDTO.Email,
            PhoneNumber = applicationUserDTO.PhoneNumber,
        };
    }
    public static ApplicationUser ConvertToEntity(this RegistrationDTO registrationUserDTO)
    {
        if (registrationUserDTO == null)
            return null;
        return new ApplicationUser()
        {
            UserName = registrationUserDTO.UserName,
            LastName = registrationUserDTO.LastName,
            FirstName = registrationUserDTO.FirstName,
            Email = registrationUserDTO.Email,
            PhoneNumber = registrationUserDTO.PhoneNumber,
        };
    }
    public static IEnumerable<ApplicationUser> ConvertToEntity(this IEnumerable<ApplicationUserDTO> statusDTO) =>
        statusDTO.Select(applicationUser => new ApplicationUser
        {
            Email = applicationUser.Email,
            FirstName = applicationUser.FirstName,
            Id = applicationUser.Id,
            LastName = applicationUser.LastName,
            PhoneNumber = applicationUser.PhoneNumber,
            UserName = applicationUser.UserName
        });
    public static IEnumerable<ApplicationUserDTO> ConvertToDTO(this IEnumerable<ApplicationUser> statusDTO) =>
        statusDTO.Select(applicationUser => new ApplicationUserDTO
        {
            Email = applicationUser.Email,
            FirstName = applicationUser.FirstName,
            Id = applicationUser.Id,
            LastName = applicationUser.LastName,
            PhoneNumber = applicationUser.PhoneNumber,
            UserName = applicationUser.UserName,
        });
    #region Role
    public static ApplicationRole ConvertToEntity(this RoleDTO roleDTO)
    {
        if (roleDTO == null)
            return null;
        return new ApplicationRole()
        {
            Name = roleDTO.Name,
        };
    }
    public static RoleDTO ConvertToDTO(this ApplicationRole applicationRole)
    {
        if (applicationRole == null)
            return null;
        return new RoleDTO()
        {
            Name = applicationRole.Name,
        };
    }
    public static IEnumerable<ApplicationRole> ConvertToEntity(this IEnumerable<RoleDTO> statusDTO) =>
        statusDTO.Select(address => new ApplicationRole
        {
            Name = address.Name,
        });
    public static IEnumerable<RoleDTO> ConvertToDTO(this IEnumerable<ApplicationRole> statusDTO) =>
        statusDTO.Select(address => new RoleDTO
        {
            Name = address.Name,
        });
    #endregion

}
