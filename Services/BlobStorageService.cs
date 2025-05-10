using Azure.Storage.Blobs;

namespace EventEase.Services;
public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName = "event-ease";

    public BlobStorageService(
        BlobServiceClient blobServiceClient/*, 
        IConfiguration configuration*/
    )
    {
        _blobServiceClient = blobServiceClient;
        // _containerName = configuration["AzureBlobStorage:event-ease"];
    }

    public async Task<string> UploadImageAsync(IFormFile file, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(blobName);
        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        return blobClient.Uri.ToString();
    }

    public async Task DeleteImageAsync(string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }
}