﻿using System;
using Vodamep.ReportBase;

namespace Vodamep.Agp.Model
{
    public partial class TravelTime : ITravelTime
    {
        public DateTime DateD { get => this.Date.AsDate(); set => this.Date = value.AsTimestamp(); }

    }
}