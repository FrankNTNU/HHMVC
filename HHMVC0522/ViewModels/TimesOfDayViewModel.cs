using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace UI.ViewModels
{
    public class TimesOfDayViewModel
    {
        private TimesOfDay _TimesOfDay;
        public TimesOfDayViewModel(TimesOfDay tod)
        {

            _TimesOfDay = tod;

        }

        public int ID { get { return _TimesOfDay.ID; } }

        public string Name { get { return _TimesOfDay.Name; } }
    }
    }