using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Episode : BaseEntity
    {
        public int SeriesId { get; set; }
        public Series Series { get; set; }

        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }

        public string Title { get; set; }
        public DateOnly ReleaseDate { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
