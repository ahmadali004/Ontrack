using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ontrack.Areas.Identity.Data;
using Ontrack.Models;

namespace Ontrack.Data;

public class OntrackContext : IdentityDbContext<OntrackUser>
{
    public OntrackContext(DbContextOptions<OntrackContext> options)
        : base(options)
    { }
          public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Examination> Examinations { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

     
    }
}