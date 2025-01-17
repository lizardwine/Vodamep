﻿using System.Linq;
using Vodamep.Data.Mkkp;
using Vodamep.Mkkp.Model;
using Xunit;

namespace Vodamep.Tests.Mkkp.Model
{
    public class DiagnosisgroupTests
    {
        [Fact]
        public void DiagnosisGroups_ReturnspectedResult()
        {

            var list1 = new[] {
                DiagnosisGroup.GeneticDisease,
                DiagnosisGroup.HeartDisease,
                DiagnosisGroup.MetabolicDisease,
                DiagnosisGroup.NeurologicalDisease,
                DiagnosisGroup.OncologicalDisease,
                DiagnosisGroup.PalliativeCare1,
                DiagnosisGroup.PalliativeCare2,
                DiagnosisGroup.PalliativeCare3,
                DiagnosisGroup.PalliativeCare4,
                DiagnosisGroup.Premature,
                DiagnosisGroup.SurgicalCare,
                DiagnosisGroup.UndefinedDiagnosisGroup,
            }.Select(x => x.ToString());

            var values = DiagnosisgroupProvider.Instance.Values.Select(x => x.Key);

            Assert.Equal(list1, values);
        }
    }
}