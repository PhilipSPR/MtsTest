using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MtsTest.Interfaces;
using MtsTest.Models;

namespace MtsTest.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly IHttpBinService _httpBinService;
        private readonly ILogger<ImageController> _logger;

        public ImageController(ApplicationDbContext context, IFileService fileService, IHttpBinService httpBinService, ILogger<ImageController> logger)
        {
            _context = context;
            _fileService = fileService;
            _httpBinService = httpBinService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                return BadRequest("Файл ней найден");
            }

            try
            {
                var filePath = await _fileService.SaveFileAsync(file);
                var imageBase64 = Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync(filePath));

                var response = await _httpBinService.SendImageAsync(imageBase64);

                var imageFile = new ImageFile
                {
                    FileName = Path.GetFileName(filePath),
                    FilePath = filePath,
                    PostResponse = await response.Content.ReadAsStringAsync(),
                    HttpStatusCode = (int)response.StatusCode
                };

                _context.ImageFiles.Add(imageFile);
                await _context.SaveChangesAsync();

                return Ok(new { fileName = imageFile.FileName, result = "Успешно", error = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = "Не успешно", error = ex.Message });
            }
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetImage(string fileName)
        {
            var imageFile = await _context.ImageFiles.FirstOrDefaultAsync(f => f.FileName == fileName);
            if (imageFile == null)
            {
                return NotFound("Файл не найден");
            }

            var bytes = await _fileService.GetFileAsync(imageFile.FileName);
            return File(bytes, "image/jpeg", imageFile.FileName);
        }

        [HttpDelete("{fileName}")]
        public async Task<IActionResult> DeleteImage(string fileName)
        {
            var imageFile = await _context.ImageFiles.FirstOrDefaultAsync(f => f.FileName == fileName);
            if (imageFile == null)
            {
                return NotFound("Файл не найден");
            }

            try
            {
                var success = await _fileService.DeleteFileAsync(imageFile.FileName);
                if (!success)
                {
                    return StatusCode(500, new { result = "Не успешно", error = "Не удалось удалить изображение" });
                }

                _context.ImageFiles.Remove(imageFile);
                await _context.SaveChangesAsync();

                return Ok(new { result = "Успешно", error = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = "Failure", error = ex.Message });
            }
        }
    }

}
