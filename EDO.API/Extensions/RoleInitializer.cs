using EDO.Access.Models;
using Microsoft.AspNetCore.Identity;

namespace EDO.API;
public class RoleInitializer
{
    public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        string adminEmail = "goblindev@gmail.com";
        string adminPassword = "abdu123456";

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
            await userManager.AddToRolesAsync(user, new string[] { RoleConst.ADMIN });

        else
        {
            user = new ApplicationUser { LastName = "Nabijonov", FirstName = "Abdulaziz", Email = adminEmail, UserName = adminEmail };
            IdentityResult result = await userManager.CreateAsync(user, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, RoleConst.ADMIN);
            }
        }
    }
}