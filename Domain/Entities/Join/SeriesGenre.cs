using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Join
{
    public class SeriesGenre
    {
        public int SeriesId { get; set; }
        public Series Series { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
