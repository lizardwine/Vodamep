﻿using System.Collections.Generic;
using Vodamep.Data.StatLp;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp.Validation
{
    internal class DisplayNameResolver
    {
        private readonly IDictionary<string, string> _dict = new SortedDictionary<string, string>();

        public DisplayNameResolver()
        {
            Init();
        }

        private void Init()
        {

            _dict.Add(nameof(StatLpReport.To), "Bis");
            _dict.Add(nameof(StatLpReport.ToD), "Bis");

            _dict.Add(nameof(StatLpReport.From), "Von");
            _dict.Add(nameof(StatLpReport.FromD), "Von");

            _dict.Add(nameof(StatLpReport.SourceSystemId), "Quellsystem ID");

            _dict.Add(nameof(StatLpReport.Institution), "Einrichtung");

            _dict.Add(nameof(Person.FamilyName), "Familienname");
            _dict.Add(nameof(Person.GivenName), "Vorname");
            _dict.Add(nameof(Person.Birthday), "Geburtsdatum");
            _dict.Add(nameof(Person.BirthdayD), "Geburtsdatum");

            _dict.Add(nameof(Admission), "Die Aufnahme");
            _dict.Add(nameof(Admission.AdmissionDate), "Aufnahmedatum");
            _dict.Add(nameof(Admission.AdmissionDateD), "Aufnahmedatum");
            _dict.Add(nameof(Admission.OriginAdmissionDate), "Ursprüngliches Aufnahmedatum");
            _dict.Add(nameof(Admission.Gender), "Geschlecht");
            _dict.Add(nameof(Admission.Country), "Land");
            _dict.Add(nameof(Admission.HousingTypeBeforeAdmission), "Wohnsituation vor der Aufnahme");
            _dict.Add(nameof(Admission.MainAttendanceRelation), "Verwandtschaftsverhältnis Hauptbetreuungspers.");
            _dict.Add(nameof(Admission.MainAttendanceCloseness), "Räumliche Nähe Hauptbetreuungsperson");
            _dict.Add(nameof(Admission.HousingReason), "Wohnraumsituations- und Ausstattungsgründe");
            _dict.Add(nameof(Admission.OtherHousingType), "Sonstige Lebens-/Betreuungssituation");
            _dict.Add(nameof(Admission.PersonalChanges), "Veränderungen persönliche Situation");
            _dict.Add(nameof(Admission.PersonalChangeOther), "Veränderungen persönliche Situation");
            _dict.Add(nameof(Admission.SocialChanges), "Veränderungen nicht bewältigt, weil");
            _dict.Add(nameof(Admission.SocialChangeOther), "Veränderungen nicht bewältigt, weil");
            _dict.Add(nameof(Admission.HousingReasonOther), "Wohnraumsituations- und Ausstattungsgründe");

            _dict.Add(nameof(Stay), "Ein Aufenthalt");
            
            _dict.Add(nameof(Leaving), "Die Entlassung");
            _dict.Add(nameof(Leaving.LeavingDateD), "Abgangsdatum");
            _dict.Add(nameof(Leaving.LeavingReason), "Abgangsart");
            _dict.Add(nameof(Leaving.DischargeLocationOther), "Sonstige Lebens-/Betreuungssituation");
            _dict.Add(nameof(Leaving.DischargeReasonOther), "Entlassung Grund");

            //enums
            _dict.Add(nameof(AttributeType.AdmissionType), "Aufnahmeart");
            _dict.Add(nameof(AttributeType.CareAllowance), "Pflegestufe");
            _dict.Add(nameof(AttributeType.CareAllowanceArge), "Pflegestufe Arge");
            _dict.Add(nameof(AttributeType.Finance), "Finanzierung");

            _dict.Add(nameof(Finance.SelfFi), "Selbst/Angehörige 100 %");
            _dict.Add(nameof(Finance.SocialAssistanceFi), "Mindestsicherung");
            _dict.Add(nameof(Finance.SocialAssistanceClaimFi), "Mindestsicherungsantrag in Bearbeitung");

            foreach (var keyValuePair in FinanceProvider.Instance.Values)
            {
                _dict.Add(keyValuePair.Key, keyValuePair.Value);
            }

            foreach (var keyValuePair in CareAllowanceProvider.Instance.Values)
            {
                _dict.Add(keyValuePair.Key, keyValuePair.Value);
            }

            foreach (var keyValuePair in CareAllowanceArgeProvider.Instance.Values)
            {
                _dict.Add(keyValuePair.Key, keyValuePair.Value);
            }

            foreach (var keyValuePair in AdmissionTypeProvider.Instance.Values)
            {
                _dict.Add(keyValuePair.Key, keyValuePair.Value);
            }

            //todo folgende dict.add löschen und mit dem Provider machen

            //_dict.Add(nameof(CareAllowance.L1), "Pflegestufe 1");
            //_dict.Add(nameof(CareAllowance.L2), "Pflegestufe 2");
            //_dict.Add(nameof(CareAllowance.L3), "Pflegestufe 3");
            //_dict.Add(nameof(CareAllowance.L4), "Pflegestufe 4");
            //_dict.Add(nameof(CareAllowance.L5), "Pflegestufe 5");
            //_dict.Add(nameof(CareAllowance.L6), "Pflegestufe 6");
            //_dict.Add(nameof(CareAllowance.L7), "Pflegestufe 7");

            _dict.Add(nameof(CareAllowanceArge.L0Ar), "Stufe 1");
            _dict.Add(nameof(CareAllowanceArge.L1Ar), "Stufe 2");
            _dict.Add(nameof(CareAllowanceArge.L2Ar), "Stufe 3");
            _dict.Add(nameof(CareAllowanceArge.L3Ar), "Stufe 4");
            _dict.Add(nameof(CareAllowanceArge.L4Ar), "Stufe 5");
            _dict.Add(nameof(CareAllowanceArge.L5Ar), "Stufe 6");
            _dict.Add(nameof(CareAllowanceArge.L6Ar), "Stufe 7");
            _dict.Add(nameof(CareAllowanceArge.L7Ar), "Stufe 8");

            _dict.Add(nameof(AdmissionType.ContinuousAt), "Daueraufnahme");
            _dict.Add(nameof(AdmissionType.HolidayAt), "Urlaub von der Pflege");
            _dict.Add(nameof(AdmissionType.TransitionalAt), "Übergangspflege");
            _dict.Add(nameof(AdmissionType.Covid19RespiteAt), "COVID-19 Entlastungspflege");
            _dict.Add(nameof(AdmissionType.GeriatricRemobilizationAt), "Geriatrische Remobilisation");
            _dict.Add(nameof(AdmissionType.CareTransitionAt), "Überleitungspflege");
            _dict.Add(nameof(AdmissionType.TrialAt), "Probe");
            _dict.Add(nameof(AdmissionType.CrisisInterventionAt), "Krisenintervention");
        }

        public string GetDisplayName(string name)
        {

            if (name == "der Aufnahme")
            {
            }

            if (!string.IsNullOrEmpty(name) && _dict.TryGetValue(name, out string value))
                return value;

            if (!string.IsNullOrWhiteSpace(name) && name != "Id" && name != "Name" && name != "Persons" && name != "Admissions"
                && name != "Valid" && name != "PersonId" && name != "Attributes" && name != "Leavings" && name != "Stays" &&
                name != "DeathLocation" && name != "DischargeLocation" && name != "DischargeReason" && name != "Pflegestufe 2")
            {

            }


            return name;
        }

    }
}
