using NewsSystem.ViewModels.CustomAttributes;
using Microsoft.AspNetCore.Http;



namespace NewsSystem.ViewModels
{
    public class AddPhotoInputModel
    {
        [ValidateImage]
        public IFormFile Image { get; set; }

        public int NewsId { get; set; }

        public string NewsUrl { get; set; }
    }
}
