using DataAccess.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Validators
{
    public class CurrencyValidator: AbstractValidator<CurrencyDTO>
    {
        public CurrencyValidator()
        {
            RuleFor(p => p.Currency).NotEmpty().WithMessage("Currency is required");
            RuleFor(p => p.Currency).Length(3).WithMessage("Currency must have a lenght of 3");
        }
    }
}
