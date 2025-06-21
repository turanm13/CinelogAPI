using FluentValidation;
using Microsoft.AspNetCore.Http;
using Service.DTOs.Actor;
using Service.DTOs.ActorCharacter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Movie
{
    public class MovieCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public TimeSpan Duration { get; set; }
        public IFormFile UploadPoster { get; set; }

        public ICollection<int> DirectorsId { get; set; }
        public ICollection<int> GenresId { get; set; }
        public ICollection<ActorCharacterCreateDto> Actors { get; set; }
    }
    public class MovieCreateDtoValidator : AbstractValidator<MovieCreateDto>
    {
        public MovieCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required")
                .MaximumLength(200)
                .WithMessage("Title must be less than 200 characters.");
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required")
                .MaximumLength(200)
                .WithMessage("Description must be less than 2000 characters.");
            RuleFor(x => x.ReleaseDate)
                .NotEmpty()
                .WithMessage("ReleaseDate is required");
            RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage("Duration is required");
            RuleFor(x => x.DirectorsId)
                .NotEmpty()
                .WithMessage("DirectorsId is required");
            RuleFor(x => x.Actors)
                .NotEmpty()
                .WithMessage("ActorsId is required");
            RuleForEach(x => x.Actors).ChildRules(actor =>
            {
                actor.RuleFor(a => a.ActorId).NotEmpty().WithMessage("ActorId is required");
                actor.RuleFor(a => a.CharacterName).NotEmpty().WithMessage("CharacterName is required");
            });
            RuleFor(x => x.GenresId)
                .NotEmpty()
                .WithMessage("GenresId is required");
        }
    }
}
