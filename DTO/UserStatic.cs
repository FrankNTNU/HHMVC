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

        //==============================================
        //恩旗
        //UserChatGroup
        public static Dictionary<string, ChatGroupDTO> UserChatGroups = new Dictionary<string, ChatGroupDTO>();
        //CustomerServiceGroup
        public static Dictionary<string, ServiceGroupDTO> ServiceGroups = new Dictionary<string, ServiceGroupDTO>();
        //ChatGroupNotRead
        public static Dictionary<string, Dictionary<string, int>> ChatGroupNotRead = new Dictionary<string, Dictionary<string, int>>();
        //==============================================
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
