using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using BugTracker.Data.Model;
using FluentValidation;
using MediatR;

namespace BugTracker.Core.Validation
{
    internal class BugStatusValidator:AbstractValidator<string>
    {
        public BugStatusValidator()
        {
            RuleFor(x => x).Must(x => BugStatuses.GetAllAvailableStatuses().Contains(x))
                .WithMessage(x =>
                    $"Статус '{x}' не поддерживается, доступные значения: {string.Join(", ", BugStatuses.GetAllAvailableStatuses())}");
        }
    }
}
