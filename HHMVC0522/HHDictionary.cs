using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI
{
    public class HHDictionary
    {
        public readonly static decimal HighActivitySuggestThreshold = 1m;

        public readonly static decimal MediumActivitySuggestThreshold = 0.9m;

        public readonly static decimal LowActivitySuggestThreshold = 0.85m;

        public readonly static int ProgramSuccessPoint = 500;

        public readonly static decimal EasyCoefficient = 0.95m;

        public readonly static decimal NormalCoeffient = 0.9m;

        public readonly static decimal HardCoeffient = 0.85m;
    }
}