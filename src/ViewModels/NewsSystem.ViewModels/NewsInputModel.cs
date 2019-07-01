using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using NewsSystem.Common;
using NewsSystem.Data.Models;
using NewsSystem.Mappings;


namespace NewsSystem.ViewModels
{
    public class NewsInputModel : IMapTo<News>
    {

        [Required]
        public string Title { get; set; }

        [Required, FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Specify a Image files(jpg,jpeg,png)")]
        public IFormFile Image { get; set; }

        [Required]
        public string Content { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public NewsSignature Signature { get; set; }

    }
}
