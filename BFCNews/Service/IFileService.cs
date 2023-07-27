using Microsoft.AspNetCore.Mvc;

namespace BFCNews.Controllers
{
    public interface IFileService
    {
        Tuple<int, string> SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);
    }
}
