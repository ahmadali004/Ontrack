using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Ontrack.Areas.Identity.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ontrack.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Ontrack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<OntrackUser> _userManager;

        public UsersController(UserManager<OntrackUser> userManager)
        {
            _userManager = userManager; // Injecting UserManager
        }

        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync(); // Retrieve all users
            return View(users); // Pass users to the view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterParent(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new OntrackUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    UserType = "Parent"
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Parent");
                    return RedirectToAction("Index", "Parents");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterTeacher(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new OntrackUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    UserType = "Teacher"
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Teacher");
                    return RedirectToAction("Index", "Teachers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult RegisterParent()
        {
            return View(new RegisterViewModel());
        }
        [Authorize(Roles = "Admin")]
        public IActionResult RegisterTeacher()
        {
            return View(new RegisterViewModel());
        }

    }
}
