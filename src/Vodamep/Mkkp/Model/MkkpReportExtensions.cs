﻿using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Vodamep.Data.Dummy;
using Vodamep.Mkkp.Validation;
using Vodamep.ReportBase;

namespace Vodamep.Mkkp.Model
{
    public static class MkkpReportExtensions
    {
        public static MkkpReport AddPerson(this MkkpReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static MkkpReport AddPersons(this MkkpReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));

        private static MkkpReport InvokeAndReturn(this MkkpReport m, Action<MkkpReport> action)
        {
            action(m);
            return m;
        }

        public static Task<SendResult> Send(this MkkpReport report, Uri address, string username, string password) => new ReportSendClient(address).Send(report, username, password);

        public static MkkpReportValidationResult Validate(this MkkpReport report) => (MkkpReportValidationResult)new MkkpReportValidator().Validate(report);

        public static string ValidateToText(this MkkpReport report, bool ignoreWarnings) => new MkkpReportValidationResultFormatter(ResultFormatterTemplate.Text, ignoreWarnings).Format(report, Validate(report));

        public static IEnumerable<string> ValidateToEnumerable(this MkkpReport report, bool ignoreWarnings) => new MkkpReportValidationResultListFormatter(ResultFormatterTemplate.Text, ignoreWarnings).Format(report, Validate(report));

        public static MkkpReport AsSorted(this MkkpReport report)
        {
            var result = new MkkpReport()
            {
                Institution = report.Institution,
                From = report.From,
                To = report.To
            };

            result.Activities.AddRange(report.Activities.AsSorted());

            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));
            result.Staffs.AddRange(report.Staffs.OrderBy(x => x.Id));
            result.TravelTimes.AddRange(report.TravelTimes.OrderBy(x => x.Id));

            return result;
        }
    }
}