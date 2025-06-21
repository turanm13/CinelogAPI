using Domain.Common;
using Domain.Entities.Join;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Series : BaseEntity 
    {       
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterUrl { get; set; }
        public DateOnly ReleaseDate { get; set; }

        public ICollection<SeriesDirector> SeriesDirectors { get; set; }

        public ICollection<SeriesGenre> SeriesGenres { get; set; }
        public ICollection<SeriesActor> SeriesActors { get; set; }

        public ICollection<Episode> Episodes { get; set; }


        public ICollection<Watchlist> Watchlists { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
