using Microsoft.AspNetCore.Mvc;

namespace BFCNews.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
