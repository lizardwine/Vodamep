﻿using FluentValidation;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Vodamep.Data;
using Vodamep.Hkpv.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Hkpv.Validation
{
    internal class EmploymentValidator : AbstractValidator<Employment>
    {
        public EmploymentValidator(DateTime from, DateTime to, Staff s)
        {
            this.RuleFor(x => x.From).NotEmpty();
            this.RuleFor(x => x.To).NotEmpty();


            // Datum des Anstellungsverhältnisses muss im Meldungszeitraum liegen
            if (from != DateTime.MinValue)
            {
                this.RuleFor(x => x.FromD).GreaterThanOrEqualTo(from)
                    .Unless(x => x.FromD == null)
                    .WithMessage(x => Validationmessages.InvalidEmploymentFromToReportRange(s));
            }

            if (to != DateTime.MinValue)
            {
                this.RuleFor(x => x.ToD).LessThanOrEqualTo(to)
                    .WithMessage(x => Validationmessages.InvalidEmploymentFromToReportRange(s))
                    .Unless(x => x.ToD == null);
            }


            // Wert für Stunden pro Woche < 100
            this.RuleFor(x => x.HoursPerWeek).ExclusiveBetween(0, 100).WithMessage(Validationmessages.EmploymentHoursPerWeekMustBeBetween0And100);

        }
    }
}
