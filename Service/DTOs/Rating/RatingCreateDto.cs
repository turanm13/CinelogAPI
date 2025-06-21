using FluentValidation;
using Service.DTOs.Comment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Rating
{
    public class RatingCreateDto
    {
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }

        [Range(1, 10, ErrorMessage = "Score must be between 1 and 10.")]
        public int Score { get; set; }
    }
    public class RatingCreateDtoValidator : AbstractValidator<RatingCreateDto>
    {
        public RatingCreateDtoValidator()
        {
            RuleFor(x => x.Score)
            .InclusiveBetween(1, 10)
            .WithMessage("Score must be between 1 and 10.");
        }
    }
}
