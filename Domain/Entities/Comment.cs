using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int? MovieId { get; set; }
        public Movie Movie { get; set; }

        public int? SeriesId { get; set; }
        public Series Series { get; set; }

        public string Content { get; set; }
    }
}
