using FluentValidation;
using Microsoft.AspNetCore.Http;
using Service.DTOs.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Director
{
    public class DirectorCreateDto
    {
        public string FullName { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? Bio { get; set; }
        public IFormFile? PhotoUrl { get; set; }
    }
    public class DirectorCreateDtoValidator : AbstractValidator<DirectorCreateDto>
    {
        public DirectorCreateDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("FullName is required")
                .MaximumLength(30)
                .WithMessage("FullName must be less than 30 characters.");
        }
    }
}
