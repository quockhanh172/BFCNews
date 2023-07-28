using Microsoft.AspNetCore.Mvc;

namespace BFCNews.Controllers
{
    public class Error : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
