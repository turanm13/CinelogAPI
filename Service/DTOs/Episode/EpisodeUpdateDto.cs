using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Episode
{
    public class EpisodeUpdateDto
    {
        public int? SeriesId { get; set; }

        public int? SeasonNumber { get; set; }
        public int? EpisodeNumber { get; set; }

        public string? Title { get; set; }
        public DateOnly? ReleaseDate { get; set; }

        public TimeSpan? Duration { get; set; }
    }
    public class EpisodeUpdateDtoValidator : AbstractValidator<EpisodeUpdateDto>
    {
        public EpisodeUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(200)
                .WithMessage("Title must be less than 200 characters.");
            RuleFor(x => x.SeasonNumber)
                .GreaterThan(0)
                .WithMessage("Season number must be greater than 0");
            RuleFor(x => x.EpisodeNumber)
                .GreaterThan(0)
                .WithMessage("Episode number must be greater than 0");
        }
    }
}
