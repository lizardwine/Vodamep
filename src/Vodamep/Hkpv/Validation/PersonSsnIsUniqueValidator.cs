﻿using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Hkpv.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Hkpv.Validation
{
    internal class PersonSsnIsUniqueValidator : AbstractValidator<HkpvReport>
    {
        public PersonSsnIsUniqueValidator()
        {
            RuleFor(x => x.Persons)
                .Custom((list, ctx) =>
                {
                    var duplicates = list.Where(x => !string.IsNullOrEmpty(x.Ssn))
                        .GroupBy(x => x.Ssn)
                        .Where(x => x.Count() > 1);

                    foreach (var entry in duplicates)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(HkpvReport.Persons), Validationmessages.SsnNotUnique(entry)));
                    }
                });
        }

    }
}
