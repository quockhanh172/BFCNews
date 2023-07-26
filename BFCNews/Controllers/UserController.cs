using BFCNews.Data;
using BinhdienNews.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BFCNews.Controllers
{
    
    public class UserController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole>roleManager )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }   
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            var Roles = _roleManager.Roles.OrderByDescending(a => a.Name).ToList();
            ViewBag.Roles = Roles;
            return View();
        }
        public async Task<IActionResult> Add (string userName, string Email,string role, IFormFile avatar)
        {
            var currentRole = await _roleManager.FindByIdAsync(role);
            await _roleManager.CreateAsync(currentRole);
            return RedirectToAction("Index", "Home");
        }
    }
}
