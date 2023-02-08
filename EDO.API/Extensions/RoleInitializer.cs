using EDO.Access;
using Microsoft.AspNetCore.Identity;

namespace EDO.API;
public class RoleInitializer
{
    public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        string adminEmail = "mamatkulov@uzcsd.uz";
        string adminPassword = "lpnz2xoflf6c";

        var roles = new Dictionary<string, string>()
            {
                {RoleConst.Administrator, "Администратор"},

            };

        foreach (var role in roles)
            if (await roleManager.FindByNameAsync(role.Key) == null)
                await roleManager.CreateAsync(new ApplicationRole(role.Key, role.Value));
        ApplicationUser user = await userManager.FindByNameAsync(adminEmail);
        if (user != null)
            await userManager.AddToRolesAsync(user, new string[] { RoleConst.Administrator });

        else
        {
            user = new ApplicationUser { LastName = "Mamatqulov", FirstName = "Sarvarbek", Email = adminEmail, UserName = adminEmail };
            IdentityResult result = await userManager.CreateAsync(user, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, RoleConst.Administrator);
            }
        }
    }
}