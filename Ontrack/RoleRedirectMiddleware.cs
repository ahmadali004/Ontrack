using Microsoft.AspNetCore.Identity;
using Ontrack.Areas.Identity.Data;

public class RoleRedirectMiddleware
{
    private readonly RequestDelegate _next;

    public RoleRedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if user is authenticated and session flag for redirect is not set
        if (context.User.Identity.IsAuthenticated && context.Session.GetString("HasRedirected") == null)
        {
            var userManager = context.RequestServices.GetService<UserManager<OntrackUser>>();
            var user = await userManager.GetUserAsync(context.User);

            if (user != null)
            {
                var userType = user.UserType;

                if (userType == "Parent" && context.Request.Path != "/Parents/StudentDetails")
                {
                    context.Session.SetString("HasRedirected", "true"); // Set session flag
                    context.Response.Redirect("/Parents/StudentDetails");
                    return;
                }
                else if (userType == "Teacher" && context.Request.Path != "/Teachers/LandingPage")
                {
                    context.Session.SetString("HasRedirected", "true"); // Set session flag
                    context.Response.Redirect("/Teachers/LandingPage");
                    return;
                }
                else if (userType == "Admin" && context.Request.Path != "/Admin/Dashboard")
                {
                    context.Session.SetString("HasRedirected", "true"); // Set session flag
                    context.Response.Redirect("/Admin/Dashboard");
                    return;
                }
            }
        }

        // Continue to the next middleware if no redirect is required
        await _next(context);
    }
}
