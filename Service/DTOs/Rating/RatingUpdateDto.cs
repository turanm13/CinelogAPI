using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Rating
{
    public class RatingUpdateDto
    {
        public int Score { get; set; }
    }
    public class RatingUpdateDtoValidator : AbstractValidator<RatingUpdateDto>
    {
        public RatingUpdateDtoValidator()
        {
            RuleFor(x => x.Score)
            .InclusiveBetween(1, 10)
            .WithMessage("Score must be between 1 and 10.");
        }
    }
}
