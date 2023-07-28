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
        private IFileService _fileService;
        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole>roleManager ,IFileService fileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _fileService = fileService;
        }   
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string Username, string Password)
        {
            var user = await _userManager.FindByNameAsync(Username);

            var result = await _signInManager.PasswordSignInAsync(user, Password, true, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult Register()
        {
            var Roles = _roleManager.Roles.OrderBy(a => a.Name).ToList();
            ViewBag.Roles = Roles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add (string userName, string Email,string role, IFormFile avatar, string fullName)
        {
            ApplicationUser user = new ApplicationUser();
            var currentRole = await _roleManager.FindByIdAsync(role);
            user.FullName = fullName;
            user.UserName = userName;
            user.Email = Email;
            if(avatar!= null)
            {
                var result = _fileService.SaveImage(avatar);
                if (result.Item1 == 1)
                {
                    user.Avatar = result.Item2;
                }
            }
            string password = "Binhdien@123";
            var CurrentAccount= await _userManager.CreateAsync(user,password);
            if(CurrentAccount.Succeeded) {
                await _userManager.AddToRoleAsync(user, currentRole.Name);
            }
            return await Task.FromResult<IActionResult>(Json(new {user=user, messager = "success" }));;
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return  RedirectToAction("Index", "Admin");
        }

    }
}
