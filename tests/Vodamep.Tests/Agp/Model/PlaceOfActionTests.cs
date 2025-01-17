﻿using System.Linq;
using Vodamep.Agp.Model;
using Vodamep.Data.Agp;
using Xunit;

namespace Vodamep.Tests.Agp.Model
{
    public class PlaceOfActionTests
    {
        [Fact]
        public void AsSorted_ReturnspectedResult()
        {
            var list1 = new[] {
                PlaceOfAction.BasePlace,
                PlaceOfAction.LkhRankweilPlace,
                PlaceOfAction.MedicalOrinationPlace,
                PlaceOfAction.OtherPlace,
                PlaceOfAction.ResidencePlace,
                PlaceOfAction.UndefinedPlace,
              }.Select(x => x.ToString());

            var values = PlaceOfActionProvider.Instance.Values.Select(x => x.Key);

            Assert.Equal(list1, values);
        }
    }
}