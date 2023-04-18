using EDO.Access.Models;
using Microsoft.AspNetCore.Identity;

namespace EDO.API;
public class RoleInitializer
{
    public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        string adminEmail = "goblindev";
        string adminPassword = "12345678a";

        var roles = new Dictionary<string, string>()
            {
                {RoleConst.ADMIN, "Adminstrator"},
                {RoleConst.NEWUSER, "NewUser"},
                {RoleConst.MAIN, "Main"},
                {RoleConst.DIVISION, "Division"},
                {RoleConst.STUFF, "Stuff"},

            };

        foreach (var role in roles)
            if (await roleManager.FindByNameAsync(role.Key) == null)
                await roleManager.CreateAsync(new ApplicationRole(role.Key, role.Value));
        ApplicationUser user = await userManager.FindByNameAsync(adminEmail);
        if (user != null)
        {
            await userManager.AddToRolesAsync(user, new string[] { RoleConst.ADMIN });
            await userManager.AddToRolesAsync(user, new string[] { RoleConst.MAIN });
            await userManager.AddToRolesAsync(user, new string[] { RoleConst.DIVISION });
            await userManager.AddToRolesAsync(user, new string[] { RoleConst.STUFF });
        }

        else
        {
            user = new ApplicationUser { LastName = "Nabijonov", FirstName = "Abdulaziz",ThirdName = "Axroriddin o'g'li", PhoneNumber="+998932505255", Email = adminEmail, UserName = adminEmail };
            IdentityResult result = await userManager.CreateAsync(user, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, RoleConst.ADMIN);
            }
        }
    }
}