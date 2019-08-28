using System.ComponentModel.DataAnnotations;

namespace NewsSystem.ViewModels
{
    public class AddVideoInputModel
    {
        [Url]
        public string VideoUrl { get; set; }

        public int NewsId { get; set; }

        public string NewsUrl { get; set; }
    }
}
