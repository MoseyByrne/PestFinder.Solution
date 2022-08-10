using Microsoft.AspNetCore.Identity;
using PestFinder.Constants;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PestFinder.Models;

namespace PestFinder.Seeds
{
  public static class DefaultUsers
  {
    public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      var defaultUser = new ApplicationUser
      {
        Email = "example@gmail.com",
        EmailConfirmed = true
      };
      if (userManager.Users.All(u => u.Id != defaultUser.Id))
      {
        var user = await userManager.FindByEmailAsync(defaultUser.Email);
        if (user == null)
        {
          await userManager.CreateAsync(defaultUser, "password");
          await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
          await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
        }
        await roleManager.SeedClaimsForAdmin();
      }
    }
    
    private async static Task SeedClaimsForAdmin(this RoleManager<IdentityRole> roleManager)
    {
      var adminRole =  await roleManager.FindByNameAsync("Admin");
      await roleManager.AddPermissionClaim(adminRole, "Locations");
    }
    
    public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
    {
      var allClaims = await roleManager.GetClaimsAsync(role);
      var allPermissions = Permissions.GeneratePermissionsForModule(module);
      foreach (var permission in allPermissions)
      {
        if (!allClaims.Any(a=> a.Type == "Permission" && a.Value == permission))
        {
          await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
        }
      }
    }
  }
}