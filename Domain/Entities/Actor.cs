using Domain.Common;
using Domain.Entities.Join;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Actor : BaseEntity
    {
        public string FullName { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<MovieActor> MovieActors { get; set; }
        public ICollection<SeriesActor> SeriesActors { get; set; }

    }
}
