using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Account
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(m => m.FullName)
                .NotNull().NotEmpty().WithMessage("Full name can not be empty!")
                .Matches(@"^[a-zA-ZəöüğışçƏÖÜĞİŞÇ\s]+$").WithMessage("Full name can only contain letters and spaces.");
            RuleFor(m=>m.UserName)
                .NotNull().NotEmpty().WithMessage("User name can not be empty!")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .Matches(@"^[a-zA-Z0-9_.-]+$").WithMessage("Username can only contain letters, numbers, underscores, dots or hyphens."); 
            RuleFor(m => m.Email)
                .NotNull().NotEmpty().WithMessage("Email can not be empty!")
                .EmailAddress().WithMessage("Email address format is wrong!");
            RuleFor(m => m.Password)
                .NotNull().NotEmpty().WithMessage("Password can not be empty!")
                .Length(6,12).WithMessage("Password must be between 6 and 12 characters");
            RuleFor(m => m.Age)
                .NotNull().NotEmpty().WithMessage("Age can not be empty!")
                .InclusiveBetween(12, 100).WithMessage("Age must be between 12 and 100 ");

        }
    }
}
