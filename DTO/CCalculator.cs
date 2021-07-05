using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public static class CCalculator
    {
        public static int GainedCalPercentage(double cal, int TDEE)
        {
            return (int)Math.Round(cal / TDEE * 100);
        }
    }
}
