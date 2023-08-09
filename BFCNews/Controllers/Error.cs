using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFCNews.Controllers
{
    [AllowAnonymous]
    public class Error : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult PermissionDenied()
        {
            return View();
        }
    }
}
