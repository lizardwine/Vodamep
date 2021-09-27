﻿using FluentValidation;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using TechTalk.SpecFlow;
using Vodamep.Data.Dummy;
using Vodamep.Cm.Model;
using Vodamep.Cm.Validation;
using Xunit;

namespace Vodamep.Specs.StepDefinitions
{

    [Binding]
    public class CmValidationSteps
    {

        private CmReportValidationResult _result;

        public CmValidationSteps()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

            var loc = new DisplayNameResolver();
            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);

            this.Report = CmDataGenerator.Instance.CreateCmReport("", 2021, 2, 1, 1, false);
        }

        public CmReport Report { get; private set; }

        public CmReportValidationResult Result
        {
            get
            {
                if (_result == null)
                {
                    _result = (CmReportValidationResult)Report.Validate();
                }

                return _result;
            }
        }


        [Given(@"eine Meldung ist korrekt befüllt")]
        public void GivenAValidReport()
        {
            // nichts zu tun
        }

        [Given(@"der Id einer Person ist nicht eindeutig")]
        public void GivenPersonIdNotUnique()
        {
            var p0 = this.Report.Persons[0];

            var p = this.Report.AddDummyPerson();

            p.Id = p0.Id;
            p.Id = p0.Id;
        }

        [Given(@"für einen Klient gibt es mehrfache Leistungen")]
        public void GivenMultipleActivitiesForOneClient()
        {
            this.Report.AddDummyActivity();
        }

        [Given(@"die Eigenschaft '(\w*)' von '(\w*)' ist nicht gesetzt")]
        public void GivenThePropertyIsDefault(string name, string type)
        {
            if (type == nameof(CmReport))
                this.Report.SetDefault(name);
            else if (type == nameof(Person))
                this.Report.Persons[0].SetDefault(name);
            else if (type == nameof(Activity))
                foreach (var a in this.Report.Activities)
                    a.SetDefault(name);
            else
                throw new NotImplementedException();
        }

        [Given(@"die Eigenschaft '(\w*)' von '(\w*)' ist auf '(.*)' gesetzt")]
        public void GivenThePropertyIsSetTo(string name, string type, string value)
        {
            IMessage m;

            if (type == nameof(CmReport))
                m = this.Report;
            else if (type == nameof(Person))
                m = this.Report.Persons[0];
            else if (type == nameof(Activity))
                m = this.Report.Activities[0];
            else if (type == nameof(ClientActivity))
                m = this.Report.ClientActivities[0];
            else
                throw new NotImplementedException();

            m.SetValue(name, value);
        }

        [Given(@"die Datums-Eigenschaft '(\w*)' von '(\w*)' hat eine Uhrzeit gesetzt")]
        public void GivenThePropertyHasATime(string name, string type)
        {
            IMessage m;
            if (type == nameof(CmReport))
                m = this.Report;
            else if (type == nameof(Person))
                m = this.Report.Persons[0];
            else if (type == nameof(Activity))
                m = this.Report.Activities[0];
            else if (type == nameof(ClientActivity))
                m = this.Report.ClientActivities[0];
            else
                throw new NotImplementedException();

            var field = m.GetField(name);
            var ts = (field.Accessor.GetValue(m) as Timestamp) ?? this.Report.From;

            ts.Seconds = ts.Seconds + 60 * 60;
            field.Accessor.SetValue(m, ts);
        }

        [Given(@"die Liste von '(\w*)' ist leer")]
        public void GivenTheListPropertyIsEmpty(string type)
        {
            if (type == nameof(Person)) { 
                this.Report.Persons.Clear();

                //also delete depending data
                this.Report.ClientActivities.Clear();
            }
            else if (type == nameof(Activity))
                this.Report.Activities.Clear();
            else if (type == nameof(ClientActivity))
                this.Report.ClientActivities.Clear();
            else
                throw new NotImplementedException();
        }

        [Then(@"*enthält (das Validierungsergebnis )?keine Fehler")]
        public void ThenTheResultContainsNoErrors(string dummy)
        {
            Assert.True(this.Result.IsValid);
            Assert.Empty(this.Result.Errors.Where(x => x.Severity == Severity.Error));
        }

        [Then(@"*enthält (das Validierungsergebnis )?keine Warnungen")]
        public void ThenTheResultContainsNoWarnings(string dummy)
        {
            Assert.Empty(this.Result.Errors.Where(x => x.Severity == Severity.Warning));
        }

        [Then(@"*enthält (das Validierungsergebnis )?genau einen Fehler")]
        public void ThenTheResultContainsOneError(object test)
        {
            Assert.False(this.Result.IsValid);
            Assert.Single(this.Result.Errors.Where(x => x.Severity == Severity.Error).Select(x => x.ErrorMessage).Distinct());
        }

        [Then(@"die Fehlermeldung lautet: '(.*)'")]
        public void ThenTheResultContainsJust(string message)
        {
            Assert.Equal(message, this.Result.Errors.Select(x => x.ErrorMessage).Distinct().Single());
        }

        [Then(@"enthält das Validierungsergebnis den Fehler '(.*)'")]
        public void ThenTheResultContainsAnError(string message)
        {
            var pattern = new Regex(message, RegexOptions.IgnoreCase);

            Assert.NotEmpty(this.Result.Errors.Where(x => x.Severity == Severity.Error && pattern.IsMatch(x.ErrorMessage)));
        }
    }
}