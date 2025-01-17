﻿using System;

namespace Vodamep.Data.StatLp
{
    public class FinanceProvider : CodeProviderBase
    {
        private static volatile FinanceProvider instance;
        private static object syncRoot = new Object();

        public static FinanceProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new FinanceProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Data.StatLp.finance.csv";
    }
}
