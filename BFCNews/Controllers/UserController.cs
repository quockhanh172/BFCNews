using BFCNews.Data;
using BinhdienNews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.QuickInfo;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Xml.Linq;

namespace BFCNews.Controllers
{
    
    public class UserController : Controller
    {
        public ApplicationDbContext _context;
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
            _context = context;
        }   
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string Username, string Password)
        {
            var user = await _userManager.FindByNameAsync(Username);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, Password, true, lockoutOnFailure: true);

                if (result.Succeeded && await _userManager.IsLockedOutAsync(user) == false)
                {
                    return RedirectToAction("Index", "Admin");
                }
                if (await _userManager.IsLockedOutAsync(user))
                {
                    return RedirectToAction("AccountLocked", "Error");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            else
            {
                return RedirectToAction("AccountLocked", "Error");
            }
            
        }

        
        public IActionResult Register()
        {
            var Roles = _roleManager.Roles.OrderBy(a => a.Name).ToList();
            ViewBag.Roles = Roles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(string userName, string Email,string role, IFormFile avatar, string fullName, string userClaim,string adminClaim, string Position, List<int> Department)
        {
            if (userName != null)
            {
                var userPost = await _userManager.FindByNameAsync(userName);
                if (userPost == null)
                {
                    ApplicationUser user = new ApplicationUser();
                    user.FullName = fullName;
                    user.UserName = userName;
                    user.Email = Email;
                    user.EmailConfirmed = true;
                    user.PhoneNumberConfirmed=true;
                    if (avatar != null)
                    {
                        var result = _fileService.SaveImage(avatar);
                        if (result.Item1 == 1)
                        {
                            user.Avatar = result.Item2;
                        }
                    }
                    string password = "Binhdien@123";
                    var result1 = await _userManager.CreateAsync(user, password);

                    if (result1.Succeeded && role=="User")
                    {
                        await _userManager.AddToRoleAsync(user, role);
                        if(userClaim != null) {                     
                                await _userManager.AddClaimAsync(user, new Claim("Permission", userClaim));
                        }
                    if(Department!=null) {
                            foreach (var departmentId in Department)
                            {
                                var currentDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.Id == departmentId);
                                if (currentDepartment != null)
                                {
                                    var departmentUser = new DepartmentUser
                                    {
                                        Position = Position,
                                        Department = currentDepartment,
                                        User = user,
                                        Status = true
                                    };
                                    await _context.DepartmentUsers.AddAsync(departmentUser);
                                }
                            }

                            await _context.SaveChangesAsync();
                    }
                    
                        return await Task.FromResult<IActionResult>(Json(new { messager = "success"}));
                    }
                    else if (adminClaim != null && role=="Admin")
                    {
                        await _userManager.AddToRoleAsync(user, role);
                        await _userManager.AddClaimAsync(user, new Claim("PermissionAdmin", adminClaim));
                        return await Task.FromResult<IActionResult>(Json(new { messager = "success" }));
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, role);
                        return await Task.FromResult<IActionResult>(Json(new { user = user, messager = "success" }));
                    }       
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

        //Lock Account

        [HttpPost]
        public async Task<IActionResult> LockDownAccount(string username)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName==username);
            if (user != null)
            {
                if (!await _userManager.IsLockedOutAsync(user))
                {
                    await _userManager.SetLockoutEnabledAsync(user, true);
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue); // Khóa người dùng vĩnh viễn

                    return Json(new { messager = "Success" });
                }
                else
                {
                    await _userManager.SetLockoutEnabledAsync(user, false);

                    return Json(new { messager = "Success" });
                }
            }
            else
            {
                return Json(new { messager = "Error" });
            }

        }

        //logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return  RedirectToAction("Index", "Admin");
        }

        //valid password
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
