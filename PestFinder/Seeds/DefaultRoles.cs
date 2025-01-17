using Microsoft.AspNetCore.Identity;
using PestFinder.Constants;
using System.Threading.Tasks;
using PestFinder.Models;

namespace PestFinder.Seeds
{
  public static class DefaultRoles
  {
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
      await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
    }
  }
}