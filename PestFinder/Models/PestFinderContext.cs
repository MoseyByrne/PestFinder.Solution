using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PestFinder.Migrations;

namespace PestFinder.Models
{
  public class PestFinderContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Pest> Pests { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<PestLocation> PestLocation { get;  set; }

    public PestFinderContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLazyLoadingProxies();
    }
  }
}