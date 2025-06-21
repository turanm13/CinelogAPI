using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Director
{
    public class DirectorUpdateDto
    {
        public string? FullName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Bio { get; set; }
        public IFormFile? PhotoUrl { get; set; }
    }
    public class DirectorUpdateDtoValidator : AbstractValidator<DirectorUpdateDto>
    {
        public DirectorUpdateDtoValidator()
        {
            RuleFor(x => x.FullName)
                .MaximumLength(30)
                .WithMessage("FullName must be less than 30 characters.");
        }
    }
}
