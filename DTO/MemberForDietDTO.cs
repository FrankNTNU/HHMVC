using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MemberForDietDTO
    {
        private string _date;
        public MemberForDietDTO(string date) {
            _date = date;
        }
        public int MemberID { get; set;  }
        public DateTime Birthdate { get; set; }

        public int ActivityLevelID { get; set; }

        public bool Gender { get; set; }

        public double Height { get; set; }

        public ProgramDTO Program { get; set; }

        public string date { get { return _date; } }
        public int Age
        {
            get
            {
                return (new DateTime(1, 1, 1) + (DateTime.ParseExact(_date, CDictionary.MMddyyyy, CultureInfo.InvariantCulture)
                - Birthdate)).Year - 1;
            }
        }


    }
}
