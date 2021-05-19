﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;
using Vodamep.Data;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class AdmissionValidator : AbstractValidator<Admission>
    {
        private DisplayNameResolver displayNameResolver = new DisplayNameResolver();

        public AdmissionValidator(StatLpReport parentReport)
        {

            this.RuleFor(x => x.Gender).NotEmpty();
            this.RuleFor(x => x.Gender).NotEmpty();

            this.RuleFor(x => x.Country).NotEmpty();

            this.RuleFor(x => x.Country)
                .Must((person, country) => CountryCodeProvider.Instance.IsValid(country))
                .Unless(x => string.IsNullOrEmpty(x.Country))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.Country)))
                .WithMessage(x => Validationmessages.ReportBaseInvalidValue(x.PersonId));


            this.RuleFor(x => x.Valid).SetValidator(new TimestampWithOutTimeValidator());

            this.RuleFor(x => x.PersonId)
                .Must((admission, personId) =>
                {
                    return parentReport.Persons.Any(y => y.Id == personId);
                })
                .WithMessage(Validationmessages.PersonIsNotAvailable);

            this.RuleFor(x => x.HousingTypeBeforeAdmission).NotEmpty();
            this.RuleFor(x => x.MainAttendanceRelation).NotEmpty();
            this.RuleFor(x => x.MainAttendanceCloseness).NotEmpty();
            this.RuleFor(x => x.HousingReason).NotEmpty();

            //ungültige werte
            var regex0 = new Regex(@"^[-,.a-zA-ZäöüÄÖÜß\(\) ][-,.a-zA-ZäöüÄÖÜß\(\) ]*[-,.a-zA-ZäöüÄÖÜß\(\) ]$");
            this.RuleFor(x => x.OtherHousingType).Matches(regex0).Unless(x => string.IsNullOrEmpty(x.OtherHousingType))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.OtherHousingType)))
                .WithMessage(x => Validationmessages.InvalidValueAdmission(parentReport.FromD.ToShortDateString(), x.PersonId));
            
            this.RuleFor(x => x.PersonalChangeOther).Matches(regex0).Unless(x => string.IsNullOrEmpty(x.PersonalChangeOther))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.PersonalChangeOther)))
                .WithMessage(x => Validationmessages.InvalidValueAdmission(parentReport.FromD.ToShortDateString(), x.PersonId));

            this.RuleFor(x => x.SocialChangeOther).Matches(regex0).Unless(x => string.IsNullOrEmpty(x.SocialChangeOther))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.SocialChangeOther)))
                .WithMessage(x => Validationmessages.InvalidValueAdmission(parentReport.FromD.ToShortDateString(), x.PersonId));
            
            this.RuleFor(x => x.HousingReasonOther).Matches(regex0).Unless(x => string.IsNullOrEmpty(x.HousingReasonOther))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.HousingReasonOther)))
                .WithMessage(x => Validationmessages.InvalidValueAdmission(parentReport.FromD.ToShortDateString(), x.PersonId));

            this.RuleFor(x => x.OtherHousingType).MaximumLength(100)
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.OtherHousingType)))
                .WithMessage(x => Validationmessages.TextTooLongAdmission(parentReport.FromD.ToShortDateString(), x.PersonId));

            this.RuleFor(x => x.PersonalChangeOther).MaximumLength(100)
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.PersonalChangeOther)))
                .WithMessage(x => Validationmessages.TextTooLongAdmission(parentReport.FromD.ToShortDateString(), x.PersonId));

            this.RuleFor(x => x.SocialChangeOther).MaximumLength(100)
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.SocialChangeOther)))
                .WithMessage(x => Validationmessages.TextTooLongAdmission(parentReport.FromD.ToShortDateString(), x.PersonId));

            this.RuleFor(x => x.HousingReasonOther).MaximumLength(100)
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.HousingReasonOther)))
                .WithMessage(x => Validationmessages.TextTooLongAdmission(parentReport.FromD.ToShortDateString(), x.PersonId));
            
            this.RuleFor(x => new { x.LastPostcode, x.LastCity }).Must(x => Postcode_CityProvider.Instance.IsValid($"{x.LastPostcode} {x.LastCity}")).WithMessage(x => Validationmessages.WrongPostCodeAdmission(parentReport.FromD.ToShortDateString(), x.PersonId));

            this.RuleFor(x => x.PersonalChanges).NotEmpty().Unless(x => !string.IsNullOrEmpty(x.PersonalChangeOther))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.PersonalChangeOther)))
                .WithMessage(Validationmessages.ItemNotValid);

            this.RuleFor(x => x.PersonalChanges)
                .Must((admission, ctx) => !admission.PersonalChanges.Any(x => x == PersonalChange.UndefinedPc))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.PersonalChanges)))
                .WithMessage(Validationmessages.ItemNotValid);

            this.RuleFor(x => x.PersonalChanges)
                .Must((admission, ctx) => ContainsDoubledValues(admission.PersonalChanges))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.PersonalChanges)))
                .WithMessage(Validationmessages.NoDoubledValuesAreAllowed);

            this.RuleFor(x => x.SocialChanges).NotEmpty().Unless(x => !string.IsNullOrEmpty(x.SocialChangeOther))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.SocialChanges)))
                .WithMessage(Validationmessages.ItemNotValid);

            this.RuleFor(x => x.SocialChanges)
                .Must((admission, ctx) => !admission.SocialChanges.Any(x => x == SocialChange.UndefinedSc))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.SocialChanges)))
                .WithMessage(Validationmessages.ItemNotValid);

            this.RuleFor(x => x.SocialChanges)
                .Must((admission, ctx) => ContainsDoubledValues(admission.SocialChanges))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.SocialChanges)))
                .WithMessage(Validationmessages.NoDoubledValuesAreAllowed);

            this.RuleFor(x => x.HousingTypeBeforeAdmission)
                .Must((admission, ctx) => !(admission.HousingTypeBeforeAdmission == AdmissionLocation.OtherAl &&
                                            string.IsNullOrEmpty(admission.OtherHousingType)))
                .WithMessage(Validationmessages.TextAreaEnterAValue);

            this.RuleFor(x => x.HousingReason)
                .Must((admission, ctx) => !(admission.HousingReason == HousingReason.OtherHr &&
                                            string.IsNullOrEmpty(admission.HousingReasonOther)))
                .WithMessage(Validationmessages.TextAreaEnterAValue);


        }

        private bool ContainsDoubledValues<T>(IEnumerable<T> values)
        {
            var doubledQuery = values.GroupBy(x => x)
                .Where(x => x.Count() > 1)
                .Select(group => group.Key);

            return !doubledQuery.Any();
        }
    }


}
