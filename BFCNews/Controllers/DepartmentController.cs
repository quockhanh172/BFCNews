using BFCNews.Data;
using BinhdienNews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BFCNews.Controllers
{
    [Authorize(Policy = "VipManager")]
    public class DepartmentController : Controller

    {
        public ApplicationDbContext _context;
        public DepartmentController (ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string activelink)
        {
            ViewBag.Activelink = activelink;
            var departments = _context.Departments.OrderBy(x=>x.Id).ToList();
            if (departments !=null)
            {
                ViewBag.Departments = departments;
            }
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

        [HttpPost]
        public Task<IActionResult> Edit(int id,string departmentname, bool status)
        {
            if (id != 0)
            {
                var department = _context.Departments.Find(id);
                if(department.Name==departmentname && department.Status == status) {
                    return Task.FromResult<IActionResult>(Json(new { messager = "do nothing" }));
                }
                else
                {
                    department.Status = status;
                    department.Name = departmentname;
                    _context.Departments.Update(department);
                    _context.SaveChanges();
                    return Task.FromResult<IActionResult>(Json(new { messager = "success",department=department }));
                }

            }
            else
            {
                return Task.FromResult<IActionResult>(Json(new { messager = "some thing went wrong" }));
            }
        }

        public Task<IActionResult> Delete(int id)
        {
            if(id != 0)
            {
                var department = _context.Departments.Find(id);
                _context.Remove(department);
                _context.SaveChanges();
                return Task.FromResult<IActionResult>(Json(new { messager = "success" }));
            }
            else
            {
                return Task.FromResult<IActionResult>(Json(new { messager = "some thing went wrong" }));
            }
        }

    }
}
