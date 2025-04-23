namespace EcommerceBackend.Services.CloudinaryServices
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
