namespace NET_test.Services
{
    public interface IFileUploadService
    {
        Task<bool> ProcessCSVFileAsync(IFormFile file);
    }
}
