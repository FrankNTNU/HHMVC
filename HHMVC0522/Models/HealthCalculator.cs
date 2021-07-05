using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.ViewModels;

namespace UI.Models
{
    static public class HealthCalculator
    {
        static public decimal TDEE(MemberForDietDTO member, int age, decimal weight)
        {
            decimal PAL = 0;

            switch (member.ActivityLevelID)
            {
                case 6:
                    PAL = 1.2m;
                    break;
                case 1:
                    PAL = 1.4m;
                    break;
                case 2:
                    PAL = 1.6m;
                    break;
                case 3:
                    PAL = 1.8m;
                    break;
            }
            decimal TDEE ;
            if (member.Gender)
            {
                TDEE = (10 * weight + 6.25m * (decimal)member.Height + 5 * (decimal)age - 5) * PAL;
            }
            else
            {
                TDEE = (10 * weight + 6.25m * (decimal)member.Height + 5 * (decimal)age - 161) * PAL;
            }

            return TDEE;
        }


        static public decimal GetProgramMaxCal(MemberForDietDTO member, ProgramDTO programDto, int age)
        {
            int days = (programDto.EndDate - programDto.StartDate).Days+1;



            double CalsToBurn = (programDto.InitialWeight-programDto.TargetWeight ) *7700;

            return Math.Round(TDEE(member, age, (decimal)programDto.InitialWeight) - (decimal)(CalsToBurn / days));

        }
    }
}