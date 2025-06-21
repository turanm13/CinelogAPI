using FluentValidation;
using Service.DTOs.Episode;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Comment
{
    public class CommentCreateDto
    {
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
        public string Content { get; set; }
    }
    public class CommentCreateDtoValidator : AbstractValidator<CommentCreateDto>
    {
        public CommentCreateDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("SeasonNumber is required")
                .MaximumLength(1000)
                .WithMessage("Comment must be less than 1000 character");
        }
    }
}
