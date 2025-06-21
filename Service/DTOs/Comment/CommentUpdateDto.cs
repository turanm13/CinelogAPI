using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Comment
{
    public class CommentUpdateDto
    {
        public string Content { get; set; }
    }
    public class CommentUpdateDtoValidator : AbstractValidator<CommentUpdateDto>
    {
        public CommentUpdateDtoValidator()
        {
            RuleFor(x => x.Content)
                .MaximumLength(1000)
                .WithMessage("Comment must be less than 1000 character");
        }
    }
}
