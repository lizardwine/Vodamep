﻿using System.Linq;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class StayValidator : AbstractValidator<Stay>
    {
        public StayValidator(StatLpReport parentReport)
        {
            this.RuleFor(x => x.From).SetValidator(new TimestampWithOutTimeValidator());
            this.RuleFor(x => x.To).SetValidator(new TimestampWithOutTimeValidator());
            this.RuleFor(x => x.To.AsDate()).GreaterThanOrEqualTo(x => x.From.AsDate()).Unless(x => x.From == null || x.To == null).WithMessage(Validationmessages.FromMustBeBeforeTo);

            this.RuleFor(x => x.PersonId)
                .Must((stay, personId) =>
                {
                    return parentReport.Persons.Any(y => y.Id == personId);
                })
                .WithMessage(Validationmessages.PersonIsNotAvailable);

            this.RuleFor(x => x)
                .Must((stay) =>
                {
                    return parentReport.Admissions.Any(admission => stay.From == admission.Valid && stay.PersonId == admission.PersonId);
                })
                .WithMessage(x => Validationmessages.StatLpReportAdmissionMustExistAtStartOfStay(x.PersonId));

        }
    }
}
