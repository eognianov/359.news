using NewsSystem.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsSystem.Data.Models
{
    public class Photo:BaseDeletableModel<string> 
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PublicId { get; set; }
        public int Version { get; set; }
        public string Signature { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Format { get; set; }
        public string ResourceType { get; set; }
        public int Bytes { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string SecureUrl { get; set; }
        public string Path { get; set; }

        public int? NewsId { get; set; }
        public virtual News News  { get; set; }

        public string UploaderId { get; set; }

        public virtual ApplicationUser Uploader { get; set; }
    }
}
