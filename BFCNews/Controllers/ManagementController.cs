using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using BFCNews.Models;
using BFCNews.Data;
using Microsoft.AspNetCore.Identity;
using BinhdienNews.Models;
using BFCNews.Service;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace BFCNews.Controllers
{

    [Authorize(Policy = "DHD")]
    public class ManagementController : Controller
    {
        private ApplicationDbContext _context;
        private SignInManager<ApplicationUser> _SignInManager;
        private UserManager<ApplicationUser> _UserManager;
        private IFileUploadService _IFileUploadService;
        public ManagementController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IFileUploadService iFileUploadService)
        {
            _context = context;
            _SignInManager = signInManager;
            _UserManager = userManager;
            _IFileUploadService = iFileUploadService;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Posts()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Posts(string department)
        {
            if (_SignInManager.IsSignedIn(User))
            {
                var user = await _UserManager.FindByNameAsync(User.Identity.Name);
                if (department != null)
                {
                    ViewBag.CurrentDepartment = department;
                    bool hasRole = User.IsInRole("Admin");
                    bool hasRole1 = User.IsInRole("SuperAdmin");
                    if (department == "All")
                    {
                        if (hasRole || hasRole1)
                        {
                            var Posts = _context.Posts.Include(p => p.Files)
                            .Include(p => p.Department)
                            .Include(p => p.User).ToList();
                            ViewBag.Posts = Posts;
                            return View();
                        }
                        else
                        {
                            return RedirectToAction("AccessDenied", "Error");
                        }

                    }
                    else
                    {
                        var currentuser = _context.Users.Include(u => u.DepartmentUsers).ThenInclude(ud => ud.Department).FirstOrDefault(u => u.Id == user.Id);
                        var DeparmentOfUser = currentuser.DepartmentUsers.Select(ud => ud.Department).FirstOrDefault();
                        if (DeparmentOfUser != null)
                        {
                            if (DeparmentOfUser.Name == department || hasRole || hasRole1)
                            {
                                var posts = _context.Posts.Where(p => p.Department.Name == department).Include(p => p.Files)
                                           .Include(p => p.Department)
                                           .Include(p => p.User).ToList();
                                ViewBag.Posts = posts;
                                return View();
                            }
                            else
                            {
                                return RedirectToAction("AccessDenied", "Error");
                            }
                        }
                        else
                        {
                            if (hasRole || hasRole1)
                            {
                                var posts = _context.Posts.Where(p => p.Department.Name == department).Include(p => p.Files)
                                           .Include(p => p.Department)
                                           .Include(p => p.User).ToList();
                                ViewBag.Posts = posts;
                                return View();
                            }
                            else
                            {
                                return RedirectToAction("AccessDenied", "Error");
                            }
                        }


                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            var departmentList = _context.Departments.ToList(); 
            return View(departmentList);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string Content, string Summary, string Title, List<IFormFile> OfficeFile)
        {
            var content = Content;
            var summary = Summary;
            var title = Title;
            var file = OfficeFile;

            var post = new Post();

            if (_SignInManager.IsSignedIn(User)) {
                var user = await _UserManager.FindByNameAsync(User.Identity.Name);
                var Department = _context.Users.Include(u => u.DepartmentUsers).ThenInclude(DU => DU.Department).Where(u => u.Id == user.Id).Select(u => u.DepartmentUsers.Select(DU => DU.Department)).FirstOrDefault().FirstOrDefault();
                bool hasPermission = User.Claims.Any(c => c.Type == "Permission" &&
                                               (c.Value == "Level1" || c.Value == "Level2" || c.Value == "Level3"));
                bool hasRole = User.IsInRole("Admin");
                bool hasRole1 = User.IsInRole("SuperAdmin");

                if (hasPermission || hasRole || hasRole1)
                {
                    bool checkTitle = _context.Posts.Any(p => p.Title == Title);
                    if (checkTitle == false)
                    {
                        post.Content = Content;
                        post.Summary = Summary;
                        post.Title = Title;
                        post.User = user;
                        post.Status = true;
                        post.Department = Department;
                        post.PublishedDate = DateTime.Now;

                        var a = new List<string>();
                        if (file != null)
                        {
                            foreach (var item in file)
                            {
                                var result = _IFileUploadService.UploadFileAsync(item);
                                if (result.Item1 == 1)
                                {
                                    a.Add(result.Item2);
                                }
                            }
                        }
                        var b = new List<FileOfPost>();
                        foreach (var item in a)
                        {
                            var x = new FileOfPost
                            {
                                Image = item,
                                Post = post // Liên kết tệp với đối tượng 'post'
                            };
                            b.Add(x);
                        }
                        post.Files = b;
                        _context.Add(post);
                        _context.SaveChanges();
                        return Json(new { messager = "success" });
                    }
                    else
                    {
                        return Json(new { messager = "available" });
                    }

                }
                else
                {

                    return Json(new { messager = "accessDenied" });
                }
            }
            else
            {
                return Json(new { messager = "accessDenied" });
            }

        }

        //DeletePost
        [HttpPost]
        public Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                var Post = _context.Posts.Include(p => p.Files).Where(p=>p.Id==id).FirstOrDefault();
                if (Post.Files != null)
                {
                    foreach (var item in Post.Files)
                    {
                        _IFileUploadService.DeleteImage(item.Image);
                    }
                }
                _context.Remove(Post);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Task.FromResult<IActionResult>(Json(new { messager = "success" }));
                }
                else
                {
                    return Task.FromResult<IActionResult>(Json(new { messager = "some thing went wrong" }));
                }
            }
            else
            {
                return Task.FromResult<IActionResult>(Json(new { messager = "some thing went wrong" }));
            }

        }

        [HttpPost]
        public IActionResult PostDetail(int id)
        {
            var x = id;
            var post= _context.Posts.Include(p => p.Files).FirstOrDefault(p=>p.Id==id);
            if (post == null)
            {
                return Json(new { messager = "error" });
            }
            else
            {
                List<string> files = new List<string>();
                if(post.Files != null)
                {
                    foreach (var file in post.Files)
                    {
                        files.Add(file.Image);
                    }
                }          
                return Json(new { Content = post.Content, files = files,date=post.PublishedDate,title=post.Title,messager="success" });
            }            
        }

        [HttpPost]
        public IActionResult GetPostEdit(int id)
        {
            if (id > 0)
            {
                var post = _context.Posts.Include(p => p.Files).FirstOrDefault(p => p.Id == id);
                if (post == null)
                {
                    return Json(new { messager = "error" });
                }
                else
                {
                    List<string> files = new List<string>();
                    if (post.Files != null)
                    {
                        foreach (var file in post.Files)
                        {
                            files.Add(file.Image);
                        }
                    }
                    return Json(new { Content = post.Content, files = files, summary = post.Summary, title = post.Title, messager = "success" });
                }
            }
            else
            {
                return Json(new { messager = "error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string Content, string SummaryEdit, string TitleEdit, List<IFormFile> OfficeFileEdit, int id)
        {
            var content = Content;
            var summary = SummaryEdit;
            var title = TitleEdit;
            var file = OfficeFileEdit;

            var oldPost = _context.Posts.Include(p=>p.User).Include(p=>p.Files).Where(p=>p.Id==id).FirstOrDefault();
            var post = new Post();
            if (_SignInManager.IsSignedIn(User))
            {
                var user = await _UserManager.FindByNameAsync(User.Identity.Name);
                var Department = _context.Users.Include(u => u.DepartmentUsers).ThenInclude(DU => DU.Department).Where(u => u.Id == user.Id).Select(u => u.DepartmentUsers.Select(DU => DU.Department)).FirstOrDefault().FirstOrDefault();
                bool hasPermission = User.Claims.Any(c => c.Type == "Permission" &&
                                               (c.Value == "Level1" || c.Value == "Level2" || c.Value == "Level3"));
                bool hasRole = User.IsInRole("Admin");
                bool hasRole1 = User.IsInRole("SuperAdmin");

                if (hasPermission || hasRole || hasRole1)
                {
                    bool checkTitle = _context.Posts.Any(p => p.Title == TitleEdit);
                    if (checkTitle == false|| oldPost.Title==title)
                    {
                        oldPost.Content = Content;
                        oldPost.Summary = SummaryEdit;
                        oldPost.Title = TitleEdit;
                        oldPost.User = user;
                        oldPost.Status = true;
                        oldPost.Department = Department;
                        oldPost.PublishedDate = DateTime.Now;

                        var a = new List<string>();
                        var b = new List<FileOfPost>();
                        if (file.Count>0)
                        {
                            var OldfFile = oldPost.Files;
                            foreach (var OF in OldfFile)
                            {
                                _IFileUploadService.DeleteImage(OF.Image);
                            }
                            foreach (var item in file)
                            {
                                var result = _IFileUploadService.UploadFileAsync(item);
                                if (result.Item1 == 1)
                                {
                                    a.Add(result.Item2);
                                }
                            }
                            foreach (var item in a)
                            {
                                var x = new FileOfPost
                                {
                                    Image = item,
                                    Post = oldPost // Liên kết tệp với đối tượng 'post'
                                };
                                b.Add(x);
                            }
                            oldPost.Files = b;
                        }
                        _context.Update(oldPost);
                        _context.SaveChanges();
                        return Json(new { messager = "success" });
                    }
                    else
                    {
                        return Json(new { messager = "available" });
                    }

                }
                else
                {

                    return Json(new { messager = "accessDenied" });
                }
            }
            else
            {
                return Json(new { messager = "accessDenied" });
            }

        }
    }
}
