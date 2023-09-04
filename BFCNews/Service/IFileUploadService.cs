namespace BFCNews.Service
{
    public interface IFileUploadService
    {
        Tuple<int, string> UploadFileAsync(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);
    }
}
