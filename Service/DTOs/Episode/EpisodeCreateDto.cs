using FluentValidation;
using Service.DTOs.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Episode
{
    public class EpisodeCreateDto
    {
        public int SeriesId { get; set; }

        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }

        public string Title { get; set; }
        public DateOnly ReleaseDate { get; set; }

        public TimeSpan Duration { get; set; }
    }
    public class EpisodeCreateDtoValidator : AbstractValidator<EpisodeCreateDto>
    {
        public EpisodeCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required")
                .MaximumLength(200)
                .WithMessage("Title must be less than 200 characters.");
            RuleFor(x => x.ReleaseDate)
                .NotEmpty()
                .WithMessage("ReleaseDate is required");
            RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage("Duration is required");
            RuleFor(x => x.SeriesId)
                .NotEmpty()
                .WithMessage("SeriesId is required");
            RuleFor(x => x.SeasonNumber)
                .NotEmpty()
                .WithMessage("SeasonNumber is required")
                .GreaterThan(0)
                .WithMessage("Season number must be greater than 0");
            RuleFor(x => x.EpisodeNumber)
                .NotEmpty()
                .WithMessage("EpisodeNumber is required")
                .GreaterThan(0)
                .WithMessage("Episode number must be greater than 0");
        }
    }
}
