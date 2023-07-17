using Microsoft.AspNetCore.Mvc;

namespace BFCNews.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public Task<IActionResult> Add(string name)
        {
            string a = name;
            return Task.FromResult<IActionResult>(Json(a));
        }
    }
}
