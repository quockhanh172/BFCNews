using BFCNews.Data;
using BinhdienNews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BFCNews.Controllers
{
    public class DepartmentController : Controller
    {
        public ApplicationDbContext _context;
        public DepartmentController (ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var departments = _context.Departments.OrderBy(x=>x.Name).ToList();
            ViewBag.Departments = departments;
            return View();
        }

        [HttpPost]
        public Task<IActionResult> Add(string name)
        {
            Department department = new Department();
            department.Name = name;
            department.Status = true;
            string a = name;
            if (a != null)
            {
                if (_context.Departments.FirstOrDefault(a => a.Name == name) == null)
                {
                    _context.Departments.Add(department);
                    _context.SaveChanges();
                    return Task.FromResult<IActionResult>(Json(new { department = department, messager = "success" }));
                }
                else
                {
                    return Task.FromResult<IActionResult>(Json("available"));
                }
            }
            else
            {
                return Task.FromResult<IActionResult>(Json("something went wrong"));
            }
        }

    }
}
