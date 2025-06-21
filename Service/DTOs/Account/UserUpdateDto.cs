using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Account
{
    public class UserUpdateDto
    {
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public int? Age { get; set; }
        public string? Email { get; set; }
    }

    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(m => m.FullName)
                .Matches(@"^[a-zA-ZəöüğışçƏÖÜĞİŞÇ\s]+$").WithMessage("Full name can only contain letters and spaces.");
            RuleFor(m => m.Email)
                .EmailAddress().WithMessage("Email address format is wrong!");
            RuleFor(m => m.UserName)
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .Matches(@"^[a-zA-Z0-9_.-]+$").WithMessage("Username can only contain letters, numbers, underscores, dots or hyphens.");
            RuleFor(m => m.Age)
                .InclusiveBetween(12, 100).WithMessage("Age must be between 12 and 100 ");
        }
    }
}
