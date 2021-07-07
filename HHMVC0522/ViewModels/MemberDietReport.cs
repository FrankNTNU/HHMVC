using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.Models;

namespace UI.ViewModels
{
    public class MemberDietReport
    {
        public List<DietLogViewModel> DietLogs { get; set; }
        public MemberHealthProfile DietProfile { get; set; }

    }
}