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

            private readonly Cloudinary cloudinary =
                new Cloudinary(new Account("news0722", "791228753937375", "uK1KSeRDXZitVebMQc5Yjvlq1W8"));


            public CloudinaryService()
            {
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
        }


}