using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using NewsSystem.Common;
using NewsSystem.Data.Models;
using NewsSystem.Mappings;
using NewsSystem.ViewModels.CustomAttributes;

namespace NewsSystem.ViewModels
{
    public class NewsInputModel : IMapTo<News>
    {

        [Required]
        public string Title { get; set; }

        [ValidateImage]
        public IFormFile Image { get; set; }

        [Required]
        public string Content { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public NewsSignature Signature { get; set; }

        public NewsCategory Category { get; set; }

        public string ImageUrl { get; set; }

    }
}
