using FluentValidation;
using Microsoft.AspNetCore.Http;
using Service.DTOs.ActorCharacter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Movie
{
    public class MovieUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public TimeSpan? Duration { get; set; }
        public IFormFile? UploadPoster { get; set; }

        public ICollection<int>? DirectorsId { get; set; }
        public ICollection<int>? GenresId { get; set; }
        public ICollection<ActorCharacterUpdateDto>? Actors { get; set; }
    }
    public class MovieUpdateDtoValidator : AbstractValidator<MovieUpdateDto>
    {
        public MovieUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(200)
                .WithMessage("Title must be less than 200 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(200)
                .WithMessage("Description must be less than 2000 characters.");
        }
    }
}
