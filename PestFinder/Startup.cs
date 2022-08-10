using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PestFinder.Models;
using PestFinder.Migrations;
using PestFinder.Permission;

namespace PestFinder
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
      services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

      services.AddMvc();

      services.AddEntityFrameworkMySql()
        .AddDbContext<PestFinderContext>(options => options
        .UseMySql(Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(Configuration["ConnectionStrings:DefaultConnection"])));

      services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddUserStore<PestFinderContext>()
        .AddDefaultTokenProviders();
        
      services.AddControllersWithViews();  
      
      // services.AddDefaultIdentity<ApplicationUser>()
      //   .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders().AddDefaultUI(); 
      
      services.Configure<IdentityOptions>(options => 
      {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 0;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredUniqueChars = 0;
      });  
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseDeveloperExceptionPage();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(routes =>
      {
        routes.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
      });

      app.UseStaticFiles();
      
      app.Run(async (context) =>
      {
        await context.Response.WriteAsync("Hello World!");
      });
    }
  }
}