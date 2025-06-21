using Service.DTOs.Movie;
using Service.DTOs.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Watchlist
{
    public class WatchlistDto
    {
        public int Id { get; set; }
        public string CreatedDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public int? MovieId { get; set; }
        public MovieShortDto? Movie { get; set; }

        public int? SeriesId { get; set; }
        public SeriesShortDto? Series { get; set; }
    }
}
