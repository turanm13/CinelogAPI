using Domain.Entities;
using Domain.Entities.Join;
using Service.DTOs.Movie;
using Service.DTOs.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Actor
{
    public class ActorDto
    {
        public int Id { get; set; }
        public string CreatedDate { get; set; }
        public string FullName { get; set; }
        public string BirthDate { get; set; }
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }

        public IEnumerable<MovieShortDto>? Movies { get; set; }
        public IEnumerable<SeriesShortDto>? Series { get; set; }
    }
}
