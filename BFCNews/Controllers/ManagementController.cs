using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;

namespace BFCNews.Controllers
{
   
    public class ManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(policy: "DHD")]
        public IActionResult Posts()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(string content,string title, List<IFormFile> images) 
        {
            var x = images;
            var b = content;
            var c = title;
            return await Task.FromResult<IActionResult>(Json("success"));
        }
    }
}
