using BFCNews.Data;
using BFCNews.ModelsView;
using BFCNews.Service;
using BinhdienNews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.QuickInfo;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
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
        private IEmailSender _emailSender;

        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole>roleManager ,IFileService fileService, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _fileService = fileService;
            _context = context;
            _emailSender = emailSender;
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, lockoutOnFailure: true);

                    if (result.Succeeded && !await _userManager.IsLockedOutAsync(user))
                    {
                        var isRoleAdmin = await _userManager.IsInRoleAsync(user,"Admin");
                        var isRoleSuperAdmin = await _userManager.IsInRoleAsync(user, "SuperAdmin");
                        var isRoleUser = await _userManager.IsInRoleAsync(user, "User");
                        if (isRoleAdmin || isRoleSuperAdmin)
                        {
                            return RedirectToAction("Index", "Department");
                        }
                        if (isRoleUser)
                        {
                            var claimUser = await _userManager.GetClaimsAsync(user);
                            var DeparmentOfUser = _context.Users.Include(u => u.DepartmentUsers).ThenInclude(du => du.Department).Where(u=>u.Id==user.Id).FirstOrDefault();
                            var departmentOfUser = DeparmentOfUser?.DepartmentUsers?.FirstOrDefault()?.Department;
                            if (claimUser != null && claimUser.FirstOrDefault().Value=="Level3")
                            {
                                return RedirectToAction("Posts", "Management", new { department = departmentOfUser.Name});
                            }
                        }
                        return RedirectToAction("Index", "Home");
                    }

                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị khóa.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Đăng nhập không thành công. Vui lòng kiểm tra tên đăng nhập và mật khẩu.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại.");
                }
            }

            // Nếu ModelState không hợp lệ, trả về view với các thông báo lỗi.
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string activelink)
        {
            ViewBag.Activelink = activelink;
            var Roles = _roleManager.Roles.OrderBy(a => a.Name).ToList();
            ViewBag.Roles = Roles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(string userName, string Email,string role, IFormFile avatar, string fullName, string userClaim, string Position, List<int> Department)
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
                                    user.DepartmentUsers.Add(departmentUser);         
                                }
                            }
                            await _userManager.UpdateAsync(user);
                        }
                    
                        return await Task.FromResult<IActionResult>(Json(new { messager = "success"}));
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

        //Change Avatar 
       public async Task<IActionResult> ChangeAvatar(IFormFile newAvatar,string UserId)
        {
            if (UserId != null)
            {
                var currentUser = _userManager.FindByIdAsync(UserId).Result;
                if (newAvatar != null)
                {
                    var result = _fileService.SaveImage(newAvatar);
                    if (result.Item1 == 1)
                    {
                        var oldImage = currentUser.Avatar;
                        currentUser.Avatar = result.Item2;
                        await _userManager.UpdateAsync(currentUser);
                        var deleteResult = _fileService.DeleteImage(oldImage);
                        if (deleteResult == true)
                        {
                            return Json(new { message = "success" });
                        }
                    }
                }
                else
                {
                    return Json(new { message = "failed" });
                }

            }
            else {
                return Json(new { message = "failed" });
            }
            return Redirect("User/ChangeAvatar");
        }

        //logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return  RedirectToAction("Index", "Admin");
        }

        //UserDetail
        public async Task<IActionResult> UserDetails(string Id)
        {
            var roleAdmin = HttpContext.User.IsInRole("Admin");
            var roleSuperAdmin = HttpContext.User.IsInRole("SuperAdmin");
            string name = HttpContext.User.Identity.Name;
            string currentAccess= (await _userManager.FindByIdAsync(Id)).UserName;

            if(roleAdmin || roleSuperAdmin || name == currentAccess)
            {
                var user = await _context.Users.Include(u => u.DepartmentUsers).ThenInclude(du => du.Department).FirstOrDefaultAsync(u => u.Id == Id);
                ViewData["userDetail"] = user;
            }
            else
            {
                return RedirectToAction ("AccessDenied","Error");
            }
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callBackUrl = Url.Action("ResetPassword", "User",
                        new { userId = user.Id, code,model.Email }, protocol: HttpContext.Request.Scheme);
                    await _emailSender.SendEmailAsync(model.Email, "Xác nhận đặt lại mật khẩu",
                    $"Vui lòng xác nhận yêu cầu đặt lại mật khẩu bằng cách <a href='{callBackUrl}'>nhấn vào đây</a>.");
                    return View("ForgotPasswordConfirmation");
                }
                ModelState.AddModelError(string.Empty, "Email không tồn tại.");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ResetPassword(string code,string Email)
        {
            if (code == null)
            {
                return RedirectToAction("ForgotPassword");
            }
            ViewBag.Email = Email;
            ViewBag.Code = code;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Người dùng không tồn tại.");
                return View(model);
            }
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu và xác nhận mật khẩu không trùng khớp.");
                return View(model);
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        //Change Password
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return Json(new { success = false, errors = new List<string> { "Không tìm thấy tên người dùng." } });
                }
                var currenPasswordCheck =  _userManager.CheckPasswordAsync( user, model.CurrentPassword);
                if (await currenPasswordCheck !=true)
                {
                    return Json(new { success = false, errors = new List<string> { "Mật khẩu cũ không chính xác." } });
                }
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (changePasswordResult.Succeeded)
                {
                    return Json(new { success = true, message = "Đổi mật khẩu thành công" });
                }
                else
                {
                    // Lấy danh sách lỗi từ changePasswordResult.Errors và trả về dưới dạng JSON
                    var errors = changePasswordResult.Errors.Select(e => e.Description).ToList();
                    return Json(new { success = false, errors = errors });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
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
