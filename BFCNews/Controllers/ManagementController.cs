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
using System.Globalization;
using BFCNews.ModelsView;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

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
                                                      .Include(p => p.User)
                                                      .Take(8)
                                                      .OrderByDescending(p => p.PublishedDate)
                                                      .ToList();
                            ViewBag.Posts = Posts;
                            int totalPosts = _context.Posts.Count();
                            ViewBag.Count = totalPosts;
                            return View();
                        }
                        else
                        {
                            return RedirectToAction("AccessDenied", "Error");
                        }

                    }
                    else if(_context.Categories.Any(c=>c.Name == department)){
                        if(hasRole || hasRole1)
                        {
                            var posts = _context.Posts.Include(p=>p.Category)
                                                      .Include(p=>p.Files)
                                                      .Where(p=>p.Category.Name==department)
                                                      .OrderByDescending(p => p.PublishedDate)
                                                      .Take(8)
                                                      .ToList();
                            var totalPosts= _context.Posts.Include(p => p.Category).Include(p => p.Files).Where(p => p.Category.Name == department).Count();
                            ViewBag.Count = totalPosts;
                            ViewBag.Posts = posts;
                            return View();
                        }
                    }
                    else
                    {
                        var CurrentUser = _context.Users.Include(u => u.DepartmentUsers).ThenInclude(ud => ud.Department).FirstOrDefault(u => u.Id == user.Id);
                        var DeparmentOfUser = CurrentUser.DepartmentUsers.Select(ud => ud.Department).FirstOrDefault();
                        if (DeparmentOfUser != null)
                        {
                            if (DeparmentOfUser.Name == department || hasRole || hasRole1)
                            {
                                var posts = _context.Posts.Where(p => p.Department.Name == department)
                                                          .Include(p => p.Files)
                                                          .Include(p => p.Department)
                                                          .Include(p=>p.Category)
                                                          .Include(p => p.User)
                                                          .OrderByDescending(p=>p.PublishedDate)
                                                          .Take(8)
                                                          .ToList();
                                ViewBag.Posts = posts;
                                int totalPosts = _context.Posts.Where(p => p.Department.Name == department).Include(p => p.Files)
                                           .Include(p => p.Department)
                                           .Include(p => p.User).Count();
                                ViewBag.Count = totalPosts;
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
                                var posts = _context.Posts.Where(p => p.Department.Name == department)
                                                          .Include(p => p.Files)
                                                          .Include(p => p.Department)
                                                          .Include(p => p.User)
                                                          .OrderByDescending(p => p.PublishedDate)
                                                          .Take(8)
                                                          .ToList();
                                ViewBag.Posts = posts;
                                int totalPosts = _context.Posts.Where(p => p.Department.Name == department).Include(p => p.Files)
                                           .Include(p => p.Department)
                                           .Include(p => p.User).Count();
                                ViewBag.Count = totalPosts;
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
        public async Task<IActionResult> Pagination(int page, string category)
        {
            var Page = page;
            var skip = (page - 1) * 8;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var Category = textInfo.ToTitleCase(category.ToLower());
            var departments = _context.Departments;
            if (Category == "All")
            {             
                var Posts = _context.Posts.Include(p => p.Files)
                            .Include (p => p.Category)
                            .Include(p => p.Department)
                            .Include(p => p.User).Select(p => new Post
                            {
                                Id = p.Id,
                                User = p.User,
                                Category=p.Category,
                                Comments = p.Comments,
                                Content = p.Content,
                                Department = p.Department,
                                Files = p.Files,
                                PublishedDate = DateTime.Now,
                                Status = p.Status,
                                Title = p.Title,
                            }).OrderByDescending(p => p.PublishedDate)
                            .Skip(skip)
                            .Take(8)
                            .ToList();
                return Json(new {posts=Posts,message="success"});
            }
            if (departments.Any(d => d.Name == Category))
            {
                var Posts = _context.Posts.Where(p => p.Department.Name == category)
                                          .Include(p => p.Files)
                                          .Include(p => p.Department)
                                          .Include(p => p.User)
                                          .Select(p => new Post
                                          {
                                            Id = p.Id,
                                            User = p.User,
                                            Category = p.Category,
                                            Comments = p.Comments,
                                            Content = p.Content,
                                            Department = p.Department,
                                            Files = p.Files,
                                            PublishedDate = DateTime.Now,
                                            Status = p.Status,
                                            Title = p.Title,
                                          })
                                          .OrderByDescending(p => p.PublishedDate)
                                          .Skip(skip).Take(8)
                                          .ToList();
                return Json(new { posts = Posts, message = "success" });
            }
            if(_context.Categories.Any(c=>c.Name == Category))
            {
                var Posts = _context.Posts.Where(p => p.Category.Name == category)
                                          .Include(p => p.Files)
                                          .Include(p => p.Department)
                                          .Include(p => p.User)
                                          .Select(p => new Post
                                          {
                                              Id = p.Id,
                                              User = p.User,
                                              Category = p.Category,
                                              Comments = p.Comments,
                                              Content = p.Content,
                                              Department = p.Department,
                                              Files = p.Files,
                                              PublishedDate = DateTime.Now,
                                              Status = p.Status,
                                              Title = p.Title,
                                          })
                                          .OrderByDescending(p => p.PublishedDate)
                                          .Skip(skip).Take(8)
                                          .ToList();
                return Json(new { posts = Posts, message = "success" });
            }

            return Json(new { message = "success" });
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
                        if (hasPermission )
                        {
                            var Department = _context.Users.Include(u => u.DepartmentUsers)
                                                            .ThenInclude(DU => DU.Department)
                                                            .Where(u => u.Id == user.Id).Select(u => u.DepartmentUsers
                                                            .Select(DU => DU.Department)).FirstOrDefault().FirstOrDefault();
                            post.Department = Department;
                        }
                        else
                        {
                            var Category = _context.Categories.Where(c=>c.Id==1).FirstOrDefault();
                            post.Category = Category;
                        }
                        
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
                        if (Department!=null)
                        {
                            post.Department = Department;
                        }
                        else
                        {
                            var Category = _context.Categories.Where(c => c.Id == 1).FirstOrDefault();
                            post.Category = Category;
                        }
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

        //search function
        [HttpPost]
        public async Task<IActionResult> Search(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Json(new {message="empty"}); // Trả về 404 Not Found khi không có text
            }

            var PostSearch = _context.Posts.Where(e => e.Title.Contains(text)).ToList();
            return Json(new { posts = PostSearch });
        }
    }
}
