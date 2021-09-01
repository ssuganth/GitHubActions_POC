using System;

namespace SampleApp.Data
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}