using CloudinaryDotNet.Actions;

namespace NewsSystem.Services.Clodinary
{
    public interface ICloudinaryServise
    {
        ImageUploadResult Upload(string filePath, string assetType);
        string Signature();
    }
}