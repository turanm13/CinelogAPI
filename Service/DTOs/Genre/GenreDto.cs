using Domain.Entities.Join;
using Service.DTOs.Movie;
using Service.DTOs.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Genre
{
    public class GenreDto
    {
        public int Id { get; set; }
        public string CreatedDate { get; set; }
        public string Name { get; set; }

        public IEnumerable<MovieShortDto>? Movies { get; set; }
        public IEnumerable<SeriesShortDto>? Series { get; set; }
    }
}
