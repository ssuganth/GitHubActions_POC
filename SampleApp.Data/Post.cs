using SampleApp.Data.Enums;

namespace SampleApp.Data
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public PostStatus PostStatus { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }
    }
}