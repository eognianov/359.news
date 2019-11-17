using System;
using System.Collections.Generic;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;

namespace NewsSystem.Services.Clodinary
{
    public class CloudinaryService : ICloudinaryService
        {
            private readonly IConfiguration configuration;

            private readonly Cloudinary cloudinary;


            public CloudinaryService(IConfiguration configuration)
            {
                this.cloudinary = new Cloudinary(new Account(configuration["Cloudinary:Cloud"], configuration["Cloudinary:API Key"]
                    ,configuration["Cloudinary:API Secret"]));

            }



            private ImageUploadParams UploadParams(string pathToFile, string assetType)
            {
                if (pathToFile==null)
                {
                    return new ImageUploadParams();
                }
                var result = new ImageUploadParams()
                {
                    File = new FileDescription(pathToFile),
                    UploadPreset = assetType
                };
                return result;
            }

//        public string UrlString(string url, bool returnAsHtmlTag = false)
//        {
//            this.cloudinary.Api.ApiUrlImgUpV(url);
//            return "";
//        }

            public ImageUploadResult Upload(ImageUploadParams upldParams)
            {

                return cloudinary.Upload(upldParams);

            }

            public string imageTag(string url)
            {
               return cloudinary.Api.UrlImgUp
                    .Transform(
                        new CloudinaryDotNet.Transformation().Width(688).Height(279).Gravity("auto").Crop("fill"))
                    .BuildImageTag(url);
            }

        }


}