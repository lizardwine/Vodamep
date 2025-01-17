﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using Vodamep.Tb.Model;

namespace Vodamep.Tb.Validation
{
    public abstract class TbReportValidationResultFormatterBase
    {
        protected readonly ResultFormatterTemplate _template;
        protected readonly bool _ignoreWarnings;

        protected TbReportValidationResultFormatterBase(ResultFormatterTemplate template, bool ignoreWarnings = false)
        {
            _template = template;
            _ignoreWarnings = ignoreWarnings;

            _strategies = new[]
            {
                new GetNameByPatternStrategy(GetIdPattern(nameof(TbReport.Persons)), GetNameOfPerson),
                new GetNameByPatternStrategy(GetIdPattern(nameof(TbReport.Activities)), GetNameOfActivity),

                new GetNameByPatternStrategy($"^{nameof(TbReport.To)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(TbReport.ToD)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(TbReport.From)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(TbReport.FromD)}$",(a,b) => string.Empty),
            };

        }

        protected string GetSeverityName(FluentValidation.Severity severity)
        {
            switch (severity)
            {
                case FluentValidation.Severity.Error:
                    return "Fehler";
                case FluentValidation.Severity.Warning:
                    return "Warnung";
                case FluentValidation.Severity.Info:
                    return "Information";
                default:
                    return severity.ToString();
            }
        }

        protected string GetInfo(TbReport report, string propertyName)
        {
            foreach (var strategy in _strategies)
            {
                var (Success, Info) = strategy.GetInfo(report, propertyName);

                if (Success) return Info;
            }

            return propertyName;
        }

        private static string GetIdPattern(string propertyName) => $@"{propertyName}\[(?<id>\d+)\]";

        private readonly GetNameByPatternStrategy[] _strategies;

        private string GetNameOfPerson(TbReport report, int index)
        {
            if (report.Persons.Count > index && index >= 0)
            {
                var e = report.Persons[index];
                return $"Person: {e.FamilyName} {e.GivenName}";
            }

            return string.Empty;
        }

        private string GetNameOfActivity(TbReport report, int index)
        {
            if (report.Activities.Count > index && index >= 0)
            {
                var e = report.Activities[index];
                return $"Aktivität {e.HoursPerMonth}{_template.Linefeed} {GetNameOfPersonById(report, e.PersonId)}{_template.Linefeed}";
            }

            return string.Empty;
        }

        private string GetNameOfPersonById(TbReport report, string id)
        {
            var e = report.Persons.FirstOrDefault(x => x.Id == id);

            if (e == null)
                return string.Empty;

            return $"{e.FamilyName} {e.GivenName}";
        }

        private class GetNameByPatternStrategy
        {
            private readonly Regex _pattern;
            private readonly Func<TbReport, int, string> _resolveInfo;

            public GetNameByPatternStrategy(string pattern, Func<TbReport, int, string> resolveInfo)
            {
                _pattern = new Regex(pattern);
                _resolveInfo = resolveInfo;
            }

            public (bool Success, string Info) GetInfo(TbReport report, string propertyName)
            {
                var m = this._pattern.Match(propertyName);

                if (m.Success)
                {
                    int.TryParse(m.Groups["id"].Value, out int id);

                    return (true, _resolveInfo(report, id));
                }
                return (false, string.Empty);
            }


        }
    }
}