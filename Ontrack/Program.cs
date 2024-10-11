using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Ontrack.Areas.Identity.Data;
using Ontrack.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSession();

// Configure DbContexts
builder.Services.AddDbContext<OntrackContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<OntrackUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Allow sign-in without email confirmation
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<OntrackContext>();

var app = builder.Build();

// Seed roles and admin user if they don't exist
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<OntrackUser>>();

    async Task SeedRolesAndAdminAsync()
    {
        string[] roleNames = { "Admin", "Teacher", "Parent" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var adminUser = new OntrackUser
        {
            UserName = "admin@ontrack.com",
            Email = "admin@ontrack.com"
        };

        var userPassword = "Teafourty@1!";
        var admin = await userManager.FindByEmailAsync(adminUser.Email);

        if (admin == null)
        {
            var createAdmin = await userManager.CreateAsync(adminUser, userPassword);
            if (createAdmin.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }

    SeedRolesAndAdminAsync().GetAwaiter().GetResult();
}

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
 