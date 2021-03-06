﻿using Ether.Models;
using FluentValidation;
using System;

namespace Ether.Validators
{
    public class ReportViewModelValidator: AbstractValidator<ReportViewModel>
    {
        public ReportViewModelValidator()
        {
            RuleFor(r => r.Profiles)
                .NotEmpty();
            RuleFor(r => r.Report).NotNull();
            RuleFor(r => r.StartDate)
                .NotNull()
                .LessThan(p => p.EndDate)
                .GreaterThan(p => DateTime.Now.AddMonths(-6)).WithMessage("Start date shouldn't be more than 6 month in the past");
            RuleFor(r => r.EndDate)
                .NotNull()
                .GreaterThan(p => p.StartDate)
                .LessThan(p => DateTime.Now.AddDays(1)).WithMessage("End date shouldn't be more than 1 day in the future"); ;
        }
    }
}
