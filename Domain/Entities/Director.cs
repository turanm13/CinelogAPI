using Domain.Common;
using Domain.Entities.Join;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Director: BaseEntity
    {
        public string FullName { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? Bio { get; set; }
        public string? PhotoUrl { get; set; }

        public ICollection<MovieDirector> MovieDirectors { get; set; }
        public ICollection<SeriesDirector> SeriesDirectors { get; set; }
    }
}
