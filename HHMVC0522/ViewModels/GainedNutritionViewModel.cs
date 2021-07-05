using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ViewModels
{
    public class GainedNutritionViewModel
    {
        DietLogBLL dlBLL = new DietLogBLL();
        private int _memberId;
        private string _date;
        private int[] _gainedNutritionsPercents;
        private int[] _suggestedNutritionsPercents;
        double _gainedCalTheDate;
        public GainedNutritionViewModel(int memberId, string date, double gainedCalTheDate)
        {

            _memberId = memberId;
            _date = date;
            _gainedCalTheDate = gainedCalTheDate;
            _gainedNutritionsPercents = new int[3];
            _suggestedNutritionsPercents = new int[3];
        }
        double AllDayGainedCal { get { return _gainedCalTheDate; } }

        double GainedProtein { get { return dlBLL.GetGainedProtein(_memberId, _date); } }   //g
        double GainedCarbs { get { return dlBLL.GetGainedCarbs(_memberId, _date); } }    //g
        double GainedFat { get { return dlBLL.GetGainedFat(_memberId, _date); } }    //g
         //int GainedNa { get { return (int)dlBLL.GetGainedNa(_memberId, _date); } }
         //int GainedSugar { get { return (int)dlBLL.GetGainedSugar(_memberId, _date); } }



        //在營養學上，最主要的熱量單位是大卡（千卡，kcal）；一公克的醣類與蛋白質能提供4大卡的熱量，脂肪為9大卡；
        //酒精也有熱量，每克的酒精則能提供7大卡熱量；至於營養素中的維生素、礦物質、纖維和水則不會提供我們身體熱量
        //https://www.hpa.gov.tw/Pages/Detail.aspx?nodeid=544&pid=726

        int GainedProteinPercent { get { return GainedProtein==0? 0:(int)Math.Round((4*GainedProtein /AllDayGainedCal)*100); } } //4KCal*g/AlldayKCal
        int GainedCarbsPercent { get { return GainedCarbs == 0 ? 0 : (int)Math.Round((4 * GainedCarbs / AllDayGainedCal)*100); } }  //4KCal*g/AlldayKCal
        int GainedFatPercent { get { return GainedFat == 0 ? 0 : (int)Math.Round((9 * GainedFat / AllDayGainedCal)*100); } } //9KCal*g/AlldayKCal


      

        public int[] GainedNutritionsPercents
        {
            get
            {
                _gainedNutritionsPercents[0] = GainedProteinPercent;
                _gainedNutritionsPercents[1] = GainedCarbsPercent;
                _gainedNutritionsPercents[2] = GainedFatPercent;
                
                return _gainedNutritionsPercents;
            }
        }


        public int[][] SuggestedNutritionsPercents
        {
            get
            {
                
                int[] rangeMin = { 40, 20, 20 };
                int[] rangeMax = { 50, 30, 40 };
                int[][] suggestedRange = { rangeMin, rangeMax };
                return suggestedRange;
            }
        }





    }
}