using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using NewsSystem.Common;
using NewsSystem.Data.Models;
using NewsSystem.Mappings;
using NewsSystem.ViewModels.CustomAttributes;

namespace NewsSystem.ViewModels
{
    public class NewsUpdataInputModel:IMapTo<News>, IMapFrom<News>
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public NewsSignature Signature { get; set; }

        [ValidateImage]
        public IFormFile Image { get; set; }
        
        public string ImageUrl { get; set; }

        public bool Published { get; set; }
    }
}
