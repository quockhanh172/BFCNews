using BFCNews.Data;
using BinhdienNews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BFCNews.Controllers
{
    public class PositionController : Controller
    {
        public ApplicationDbContext _context;
        public PositionController (ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
           var Positions = _context.Positions.OrderBy(x=>x.Id).ToList();
            ViewBag.Positions = Positions;
            return View();
        }

        [HttpPost]
        public Task<IActionResult> Add(string name)
        {
            Position position = new Position();
            position.Name = name;
            position.Status = true;
            string a = name;
            if (a != null)
            {
                if (_context.Positions.FirstOrDefault(a => a.Name == name) == null)
                {
                    _context.Positions.Add(position);
                    _context.SaveChanges();
                    return Task.FromResult<IActionResult>(Json(new { department = position, messager = "success" }));
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
        public Task<IActionResult> Edit(int id, string positionname, bool status)
        {
            if (id != 0)
            {
                var position = _context.Positions.Find(id);
                if (position.Name == positionname && position.Status == status)
                {
                    return Task.FromResult<IActionResult>(Json(new { messager = "do nothing" }));
                }
                else
                {
                    position.Status = status;
                    position.Name = positionname;
                    _context.Positions.Update(position);
                    _context.SaveChanges();
                    return Task.FromResult<IActionResult>(Json(new { messager = "success", position = position }));
                }

            }
            else
            {
                return Task.FromResult<IActionResult>(Json(new { messager = "some thing went wrong" }));
            }
        }
        public Task<IActionResult> Delete(int id)
        {
            if (id != 0)
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
