using CloudinaryDotNet.Actions;

namespace NewsSystem.Services.Clodinary
{
    public interface ICloudinaryService
    {
        ImageUploadResult Upload(ImageUploadParams upldParams);

    }
}