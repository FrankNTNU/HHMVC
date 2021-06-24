using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ViewModels
{
    public class WorkoutSuggestViewModel
    {
        //public List<string> Suggestion { get; set; } = new List<string>();

        public string Warning { get; set; }

        public string ActivityLevel { get; set; }

        public List<Workout> SuggestionByPreferences { get; set; } = new List<Workout>();

        public List<Workout> SuggestionByAge { get; set; } = new List<Workout>();

        public List<Workout> SuggestionByLog { get; set; } = new List<Workout>();

        public List<decimal> IngestReport { get; set; } = new List<decimal> { -1m, -1m, -1m, -1m, -1m };

        public List<string> TimeOfDay { get; set; }
    }
}