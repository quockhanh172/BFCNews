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
        public async Task<IActionResult> Add(string userName, string Email,string role, IFormFile avatar, string fullName)
        {
            if (userName != null)
            {
                var userPost = await _userManager.FindByNameAsync(userName);
                if (userPost == null)
                {
                    var a = _userManager.FindByNameAsync(userName);
                    ApplicationUser user = new ApplicationUser();
                    var currentRole = await _roleManager.FindByIdAsync(role);
                    user.FullName = fullName;
                    user.UserName = userName;
                    user.Email = Email;
                    if (avatar != null)
                    {
                        var result = _fileService.SaveImage(avatar);
                        if (result.Item1 == 1)
                        {
                            user.Avatar = result.Item2;
                        }
                    }
                    string password = "Binhdien@123";
                    var CurrentAccount = await _userManager.CreateAsync(user, password);
                    if (CurrentAccount.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, currentRole.Name);
                    }
                    return await Task.FromResult<IActionResult>(Json(new { user = user, messager = "success" }));
                }
                else {
                        return await Task.FromResult<IActionResult>(Json(new { messager = "available" }));   
                }

            }
            else
            {
                return Json(new { messager = "Error" });
            }
            
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return  RedirectToAction("Index", "Admin");
        }

        private bool IsPasswordValid(string password)
        {
            // Define the password requirements
            int requiredLength = 8;
            int requiredUniqueChars = 1;
            bool requireDigit = true;
            bool requireLowercase = true;
            bool requireNonAlphanumeric = true;
            bool requireUppercase = true;

            // Check the length
            if (password.Length < requiredLength)
                return false;

            // Check for the required unique characters
            if (password.Distinct().Count() < requiredUniqueChars)
                return false;

            // Check for the required character types
            if (requireDigit && !password.Any(char.IsDigit))
                return false;
            if (requireLowercase && !password.Any(char.IsLower))
                return false;
            if (requireNonAlphanumeric && !password.Any(c => !char.IsLetterOrDigit(c)))
                return false;
            if (requireUppercase && !password.Any(char.IsUpper))
                return false;

            // All requirements passed, password is valid
            return true;
        }
    }
}
