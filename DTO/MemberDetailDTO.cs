using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MemberDetailDTO
    {
        public int ID { get; set; }
        public string TaiwanID { get; set; }
        public int Height { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public int StatusID { get; set; }
        public int ActivityLevelID { get; set; }
        public string ActivityLevel { get; set; }
        public bool Gender { get; set; }
        public string GenderString { get; set; }
        public bool IsAdmin { get; set; }
        public string Phone { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
