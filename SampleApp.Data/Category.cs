using System.Collections.Generic;

namespace SampleApp.Data
{
    public class Category : BaseEntity
    {
        #region Ctor

        public Category()
        {
            Posts = new HashSet<Post>();
        }

        #endregion
        
        public string ParentId { get; set; }
        public Category Parent { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Post> Posts { get; }
    }
}