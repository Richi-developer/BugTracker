using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Dto;
using FluentValidation;

namespace BugTracker.Core.Validation
{
    public class BugDtoValidator: AbstractValidator<BugDto>
    {
        public BugDtoValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Наименование бага должно быть заполнено");
        }
    }
}
