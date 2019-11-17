namespace NewsSystem.ViewModels
{
    using NewsSystem.Data.Models;
    using NewsSystem.Mappings;

    public class SourceViewModel : IMapFrom<Source>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public int NewsCount { get; set; }
    }
}
