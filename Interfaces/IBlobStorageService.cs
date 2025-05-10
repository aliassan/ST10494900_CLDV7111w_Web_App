namespace EventEase.Services;

public interface IBlobStorageService
{
    Task<string> UploadImageAsync(IFormFile file, string blobName);
    Task DeleteImageAsync(string blobName);
}