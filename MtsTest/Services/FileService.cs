using MtsTest.Interfaces;

namespace MtsTest.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger _logger;

        public FileService(IWebHostEnvironment environment, ILogger logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExtension = ".jpg"; 
            string filePath = Path.Combine(_environment.ContentRootPath, "Uploads", fileName + fileExtension);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                // Convert and save image
                using var image = System.Drawing.Image.FromStream(file.OpenReadStream());
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            return filePath;
        }

        public async Task<byte[]> GetFileAsync(string fileName)
        {
            string filePath = Path.Combine(_environment.ContentRootPath, "Uploads", fileName);
            if (File.Exists(filePath))
            {
                return await System.IO.File.ReadAllBytesAsync(filePath);
            }
            return null;
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            string filePath = Path.Combine(_environment.ContentRootPath, "Uploads", fileName);
            if (File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return true;
            }
            return false;
        }
    }
}
