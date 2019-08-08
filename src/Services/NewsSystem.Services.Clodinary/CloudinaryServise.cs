using System;
using System.Collections.Generic;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;

namespace NewsSystem.Services.Clodinary
{
    public class CloudinaryServise : ICloudinaryServise
        {
            private readonly IConfiguration configuration;

            private readonly Cloudinary cloudinary =
                new Cloudinary(new Account("news0722", "791228753937375", "uK1KSeRDXZitVebMQc5Yjvlq1W8"));


            public CloudinaryServise()
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

            public ImageUploadResult Upload(string pathToFile, string assetType ="mainNews" )
            {
                try
                {
                    return cloudinary.Upload(UploadParams(pathToFile, assetType));

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
        }


}