using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace EventEase.Services;
public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobStorageService(
        BlobServiceClient blobServiceClient,
        IConfiguration configuration)
    {
        _blobServiceClient = blobServiceClient;
        _containerName = configuration["AzureBlobStorage:ContainerName"] ?? "event-ease";
    }

    // Keep all existing methods exactly as they are
    public async Task<string> UploadImageAsync(IFormFile file, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(blobName);
        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        return /*blobClient.Uri.ToString()*/blobName;
    }

    public async Task DeleteImageAsync(string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }
}