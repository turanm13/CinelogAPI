using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Join
{
    public class SeriesDirector
    {
        public int SeriesId { get; set; }
        public Series Series { get; set; }

        public int DirectorId { get; set; }
        public Director Director { get; set; }
    }
}
