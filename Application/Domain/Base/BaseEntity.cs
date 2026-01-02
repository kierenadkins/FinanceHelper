using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Domain.Base
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateClosed { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool Deleted { get; set; }
    }
}
