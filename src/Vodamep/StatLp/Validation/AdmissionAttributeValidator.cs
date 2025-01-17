﻿
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{

    internal class AdmissionAttributeValidator : AbstractValidator<StatLpReport>
    {
        private static readonly DisplayNameResolver DisplayNameResolver = new DisplayNameResolver();

        public AdmissionAttributeValidator()
        {
            this.RuleFor(x => x).Custom((x, ctx) =>
            {
                if (x.Admissions.Any())
                {
                    foreach (Admission admission in x.Admissions)
                    {
                        // Alle Attribute für diese Aufnahme
                        List<Attribute> attributesForAddmission = x.Attributes.Where(att => att.PersonId == admission.PersonId &&
                                                                                            admission.AdmissionDateD == att.FromD &&
                                                                                            att.AttributeType != AttributeType.UndefinedAttribute).ToList();

                        if (admission.AdmissionDateD > System.DateTime.MinValue)
                        {

                            // Alle Attributtypen prüfen
                            foreach (AttributeType attributeType in ((AttributeType[])System.Enum.GetValues(typeof(AttributeType))).Where(at => at != AttributeType.UndefinedAttribute))
                            {
                                int attributeCount = attributesForAddmission.Count(a => a.AttributeType == attributeType);

                                if (attributeCount <= 0)
                                {
                                    ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.Admissions),
                                        Validationmessages.StatLpAdmissionAttributeMissing(
                                            x.GetPersonName(admission.PersonId),
                                            admission.AdmissionDateD?.ToShortDateString(),
                                            DisplayNameResolver.GetDisplayName(attributeType.ToString()))));
                                }
                                else if (attributeCount > 1)
                                {
                                    ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.Admissions),
                                        Validationmessages.StatLpAdmissionMultipleAttribute(
                                            x.GetPersonName(admission.PersonId),
                                            admission.AdmissionDateD?.ToShortDateString(),
                                            DisplayNameResolver.GetDisplayName(attributeType.ToString()))));

                                }
                            }
                        }

                    }
                }
            });
        }
    }
}
