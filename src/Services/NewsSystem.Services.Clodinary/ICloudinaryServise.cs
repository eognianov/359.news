using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsSystem.Services.Clodinary
{
    public interface ICloudinaryServise
    {
        ImageUploadResult Upload(string filePath, string assetType);
        string Signature();
    }
}