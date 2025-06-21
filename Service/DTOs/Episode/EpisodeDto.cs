using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Episode
{
    public class EpisodeDto
    {
        public int Id { get; set; }
        public string CreatedDate { get; set; }
        public string SeriesName { get; set; }

        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }

        public string Title { get; set; }
        public string? ReleaseDate { get; set; }

        public string? Duration { get; set; }
    }
}
