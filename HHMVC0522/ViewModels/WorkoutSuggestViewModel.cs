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

        public List<String> SuggestionByPreferences { get; set; } = new List<string>();

        public List<String> SuggestionByAge { get; set; } = new List<string>();

        public List<String> SuggestionByLog { get; set; } = new List<string>();

        public List<decimal> IngestReport { get; set; } = new List<decimal> { -1m, -1m, -1m, -1m, -1m };

        public List<decimal> ConsumeReport { get; set; } = new List<decimal> { -1m, -1m, -1m, -1m, -1m };

        public List<string> TimeOfDay { get; set; }
    }
}