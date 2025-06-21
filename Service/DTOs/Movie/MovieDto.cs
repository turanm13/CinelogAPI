using Domain.Entities.Join;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.DTOs.ActorCharacter;
using Service.DTOs.Director;
using Service.DTOs.Genre;
using Service.DTOs.Comment;

namespace Service.DTOs.Movie
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string CreatedDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReleaseDate { get; set; }
        public string Duration { get; set; }
        public string PosterUrl { get; set; }

        public double AverageRating { get; set; }
        public ICollection<CommentShortDto> Comments { get; set; }

        public ICollection<DirectorShortDto>? Directors { get; set; }
        public ICollection<GenreShortDto>? Genres { get; set; }
        public ICollection<ActorCharacterDto>? Actors { get; set; }
    }
}
