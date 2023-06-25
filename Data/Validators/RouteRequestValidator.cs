using DataAccess.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Validators
{
    public class RouteRequestValidator: AbstractValidator<RouteRequestDTO>
    {
        public RouteRequestValidator()
        {
            RuleFor(p => p.Origin)
                .NotEmpty()
                .WithMessage("Origin is required");

            RuleFor(p => p.Origin)
                .Length(3)
                .WithMessage("Origin must have a lenght of 3");

            RuleFor(p => p.Destination)
                .NotEmpty()
                .WithMessage("Destination is required");

            RuleFor(p => p.Destination).
                Length(3)
                .WithMessage("Destination must have a lenght of 3");

            RuleFor(x => x.Origin)
            .NotEqual(x => x.Destination)
            .WithMessage("Origin and Destination must be different.");
        }

    }
}
