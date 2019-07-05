using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using NewsSystem.Data;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace NewsSystem.Services.Clodinary
{
    public class CloudinaryServise : ICloudinaryServise
    {
        private readonly IConfiguration configuration;
        private readonly Cloudinary cloudinary =
            new Cloudinary(new Account("my_cloud_name", "my_api_key", "my_api_secret"));

        public CloudinaryServise(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Transformation DefaultOptins { get; private set; }

        public string UrlString(string url, bool returnAsHtmlTag = false)
        {
            this.cloudinary.Api.UrlImgUp.Transform(this.DefaultOptins);
            return "";
        }
    }

}

