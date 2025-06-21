using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Login
{
    public class LoginDto
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
        public class LoginDtoValidator : AbstractValidator<LoginDto>
        {
            public LoginDtoValidator()
            {
                RuleFor(m => m.UserNameOrEmail)
                    .NotNull().WithMessage("Username or email can not be empty!");
                RuleFor(m => m.Password)
                    .NotNull().NotEmpty().WithMessage("Password can not be empty!")
                    .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
            }
        }
    }
}
