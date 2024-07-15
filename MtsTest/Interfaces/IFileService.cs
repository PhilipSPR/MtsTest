namespace MtsTest.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file);
        Task<byte[]> GetFileAsync(string fileName);
        Task<bool> DeleteFileAsync(string fileName);
    }
}
