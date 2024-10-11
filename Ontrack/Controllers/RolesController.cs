using Microsoft.AspNetCore.Mvc;

namespace Ontrack.Controllers
{
    public class RolesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
