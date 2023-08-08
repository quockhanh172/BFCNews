using BFCNews.Data;
using BinhdienNews.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using System.Data;
using System.Security.Claims;

namespace BFCNews.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        public RoleController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var Roles = _roleManager.Roles.OrderByDescending(a => a.Name).ToList();
            ViewBag.Roles = Roles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add (string Role, List<string> Permission)
        {
            if (Role == null || Permission==null)
            {
                return await Task.FromResult<IActionResult>(Json(new { messager = "failed" }));
            }
            else
            {
                bool roleExists = await _roleManager.RoleExistsAsync(Role);
                if (roleExists == false)
                {
                    var role = new IdentityRole();
                    role.Name = Role;
                   var result =await _roleManager.CreateAsync(role);
                    foreach(var item in Permission)
                    {
                        if (result.Succeeded)
                        {
                            await _roleManager.AddClaimAsync(role, new Claim(item, item));
                    }
                    }
                    
                    return await Task.FromResult<IActionResult>(Json(new { role = Role, messager = "success" }));
                }
                else
                {
                    return await Task.FromResult<IActionResult>(Json(new { messager = "available" }));
                }
            }     
        }


        [HttpPost]
        public async Task<IActionResult> Edit(string name, string id)
        {
            if (name == null)
            {
                return await Task.FromResult<IActionResult>(Json(new { messager = "failed" }));
            }
            else
            {
                var roleExists = await _roleManager.FindByIdAsync(id);
                if (roleExists.Name != name )
                {
                    roleExists.Name= name;
                    await _roleManager.UpdateAsync(roleExists);
                    return await Task.FromResult<IActionResult>(Json(new { role = roleExists, messager = "success" }));
                }
                else
                {
                    return await Task.FromResult<IActionResult>(Json(new { messager = "donothing" }));
                }
            }
        }
    }
}
