using DataAccess.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Validators
{
    public class JourneyValidator: AbstractValidator<Journey>
    {
        public JourneyValidator()
        {
            RuleFor(p => p.Origin).NotEmpty().WithMessage("Origin is required");
            RuleFor(p => p.Origin).Length(3).WithMessage("Origin must have a lenght of 3");
            RuleFor(p => p.Destination).NotEmpty().WithMessage("Destination is required");
            RuleFor(p => p.Destination).Length(3).WithMessage("Destination must have a lenght of 3");
            RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(p => p.Flights).NotNull().WithMessage("must have at least one flight");
        }
    }
}
