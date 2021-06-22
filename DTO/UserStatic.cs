using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public static class UserStatic
    {
        public static List<UserDetail> ConnectedUsers = new List<UserDetail>();
    }
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }
    public class UserDetail
    {
        public string ConnID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }
    }
}
