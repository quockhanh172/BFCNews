using System.IO;

namespace BFCNews.Service
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment environment;
        private readonly ILogger<FileUploadService> _logger;
        public FileUploadService(IWebHostEnvironment env)
        {
            environment = env;
        }
        public Tuple<int, string> UploadFileAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("File is empty");
                }

                // Generate a unique file name (or use the original file name)


                // Determine the physical path to save the file
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "OfficeUploads");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var originalFileName = Path.GetFileName(file.FileName);
                var uniqueFileName = originalFileName;
                var filePath = Path.Combine(path, uniqueFileName);
                int count = 1;
                while (System.IO.File.Exists(filePath))
                {
                    // If a file with the same name exists, add a number to make it unique
                    uniqueFileName = Path.GetFileNameWithoutExtension(originalFileName) + "_" + count + Path.GetExtension(originalFileName);
                    filePath = Path.Combine(path, uniqueFileName);
                    count++;
                }
                var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                // Save the file to the server
                stream.Close();
                return new Tuple<int, string>(1, uniqueFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, "Error has occured");
            }
        }

        bool IFileUploadService.DeleteImage(string FileName)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "OfficeUploads\\", FileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
