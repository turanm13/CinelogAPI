using Domain.Common;
using Domain.Entities.Join;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }
        public ICollection<SeriesGenre> SeriesGenres { get; set; }
    }
}
