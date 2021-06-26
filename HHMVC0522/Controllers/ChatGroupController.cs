using DTO;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class ChatGroupController : Controller
    {
        // GET: ChatGroup
        public ActionResult GroupChat()
        {

            return View(UserStatic.UserChatGroups);
        }

        //==============================================================
        //恩旗
        //NewGroup
        [HttpPost]
        public JsonResult NewGroup(string connId, string groupName, int weight1, int weight2)
        {
            if (UserStatic.UserChatGroups.Keys.Contains(groupName))
            {
                return Json(new { Result = "已有同名的聊天群" });
            }
            else
            {
                var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                Context.Groups.Add(connId, groupName);

                UserStatic.UserChatGroups.Add(groupName, new ChatGroupDTO
                {
                    WeightRange = new Tuple<int, int>(weight1, weight2),
                    GroupMembers = new List<UserDetail> { new UserDetail
                    {
                        ConnID = connId,
                        UserID = User.Identity.Name
                    }}
                });

                return Json(new { Result = "Success" });
            }

        }

        //AddToGroup
        [HttpPost]
        public JsonResult AddToGroup(string connId, string groupName)
        {
            var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            Context.Groups.Add(connId, groupName);

            //Add To Static Dictionary
            if (UserStatic.UserChatGroups.Keys.Contains(groupName))
            {
                UserStatic.UserChatGroups[groupName].GroupMembers.Add(new UserDetail
                {
                    ConnID = connId,
                    UserID = User.Identity.Name
                });

                return Json(new { Result = "Success" });
            }
            else
            {
                return Json(new { Result = "無此聊天群" });
            }


        }

        //GetGroups
        [HttpPost]
        public JsonResult GetGroups()
        {
            var groupList = UserStatic.UserChatGroups.Select(cg => new 
            { 
                cg.Key,
                cg.Value.WeightRange.Item1,
                cg.Value.WeightRange.Item2
            });

            return Json(groupList);
        }
    }
}