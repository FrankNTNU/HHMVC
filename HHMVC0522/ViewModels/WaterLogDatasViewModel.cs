using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ViewModels
{
    public class WaterLogDatasViewModel
    {
        WaterLogBLL wBLL = new WaterLogBLL();
        private int[] _weeklyWaterLogGaineds;
        private int[][] _weeklyWaterLogSuggestedRanges;

        public WaterLogDatasViewModel(int memberId, string date) {
            _weeklyWaterLogGaineds = wBLL.GetWeeklyWaterLogs(memberId, date);
            _weeklyWaterLogSuggestedRanges = wBLL.GetWeeklySuggestedWaterLogRanges(memberId, date);
        }
        public int[] WeeklyWaterLogGaineds { get { return _weeklyWaterLogGaineds; } }
        public int[][] WeeklyWaterLogSuggestedRanges { get { return _weeklyWaterLogSuggestedRanges; } }

    }
}