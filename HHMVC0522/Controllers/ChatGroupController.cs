using DAL;
using DTO;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    [System.Web.Mvc.Authorize]
    public class ChatGroupController : Controller
    {
        HealthHelperEntities dbContext = new HealthHelperEntities();

        // GET: ChatGroup
        public ActionResult GroupChat()
        {

            return View(UserStatic.UserChatGroups);
        }

        //==============================================================
        //恩旗
        //NewGroup
        [HttpPost]
        public JsonResult NewGroup(string connId, string groupName, int weight1, int weight2, string preGroupId)
        {
            if (UserStatic.UserChatGroups.Keys.Contains(groupName))
            {
                return Json(new { Result = "已有同名的聊天群" });
            }
            else
            {
                // Add To DB Groups Table
                Group group = dbContext.Groups.Add(new Group
                {
                    GroupName = groupName,
                    StartWeight = weight1,
                    EndWeight = weight2,
                    IsAlive = true,
                    IsService = false
                });

                dbContext.SaveChanges();

                //Add
                var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                Context.Groups.Add(connId, group.ID.ToString());

                UserStatic.UserChatGroups.Add(group.ID.ToString(), new ChatGroupDTO
                {
                    GroupName = groupName,
                    WeightRange = new Tuple<int, int>(weight1, weight2),
                    GroupMembers = new List<UserDetail> { new UserDetail
                    {
                        ConnID = connId,
                        UserID = User.Identity.Name
                    }}
                });

                //Remove
                if (preGroupId != "")
                {
                    Context.Groups.Remove(connId, preGroupId);

                    var member = UserStatic.UserChatGroups[preGroupId].GroupMembers
                        .SingleOrDefault(gm => gm.ConnID == connId);

                    UserStatic.UserChatGroups[preGroupId].GroupMembers.Remove(member);

                    RemoveGroup(this.dbContext, preGroupId);
                }

                return Json(new
                {
                    Result = "Success",
                    GroupID = group.ID,
                    GroupName = groupName
                });

            }
        }

        //AddToGroup
        [HttpPost]
        public JsonResult AddToGroup(string connId, string groupId, string groupName, string preGroupId)
        {
            var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            //Add To Static Dictionary
            if (UserStatic.UserChatGroups.Keys.Contains(groupId))
            {
                //Add
                Context.Groups.Add(connId, groupId);

                UserStatic.UserChatGroups[groupId].GroupMembers.Add(new UserDetail
                {
                    ConnID = connId,
                    UserID = User.Identity.Name
                });

                //Remove
                if (preGroupId != "")
                {
                    Context.Groups.Remove(connId, preGroupId);

                    var member = UserStatic.UserChatGroups[preGroupId].GroupMembers
                        .SingleOrDefault(gm => gm.ConnID == connId);

                    UserStatic.UserChatGroups[preGroupId].GroupMembers.Remove(member);

                    RemoveGroup(this.dbContext, preGroupId);
                }

                return Json(new
                {
                    Result = "Success",
                    GroupID = groupId,
                    GroupName = groupName
                });
            }
            else
            {
                return Json(new { Result = "無此聊天群", GroupID = "", GroupName = "" });
            }

        }

        //GetGroups
        [HttpPost]
        public JsonResult GetGroups()
        {
            //var groupList = UserStatic.UserChatGroups.Select(cg => new 
            //{ 
            //    cg.Key,
            //    cg.Value.WeightRange.Item1,
            //    cg.Value.WeightRange.Item2
            //});

            var groupList = dbContext.Groups.Where(g => g.IsAlive).Select(g => new
            {
                g.ID,
                g.GroupName,
                g.StartWeight,
                g.EndWeight
            });

            return Json(groupList);
        }

        internal static void RemoveGroup(HealthHelperEntities dbContext, string groupId)
        {
            if (UserStatic.UserChatGroups[groupId].GroupMembers.Count == 0)
            {
                UserStatic.UserChatGroups.Remove(groupId);

                int gId = int.Parse(groupId);

                //Set This Group's IsAlive to false
                dbContext.Groups.SingleOrDefault(g => g.ID == gId).IsAlive = false;

                dbContext.SaveChanges();
            }
        }

        [HttpPost]
        public JsonResult GetGroupMembers(string groupId)
        {

            var gmList = UserStatic.UserChatGroups[groupId].GroupMembers.Select(gm => gm.UserID).ToList();

            var nameList = gmList.Select(gm =>
            {
                return dbContext.Members.SingleOrDefault(m => m.ID.ToString() == gm).UserName;
            }).Distinct();

            return Json(nameList);
        }

        [HttpPost]
        public ActionResult SendMessage(string connId, string groupId, string message)
        {

            var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            string UserId = UserStatic.UserChatGroups[groupId].GroupMembers
                .SingleOrDefault(gm => gm.ConnID == connId).UserID;

            string userName = dbContext.Members.SingleOrDefault(m => m.ID.ToString() == UserId).UserName;

            List<string> connIds = UserStatic.UserChatGroups[groupId].GroupMembers
                .Where(gm => gm.UserID == UserId).Select(gm => gm.ConnID).ToList(); ;

            Context.Clients.Group(groupId).receiveFromGroupMember(connIds, userName, message);

            //todo
            dbContext.GroupChats.Add(new GroupChat
            {
                GroupID = int.Parse(groupId),
                MemberID = int.Parse(UserId),
                Message = message,
                TimeStamp = DateTime.Now
            });

            dbContext.SaveChanges();

            return View();
        }

        [HttpPost]
        public JsonResult GetGroupId(string connId)
        {
            foreach (var groupId in UserStatic.UserChatGroups.Keys.ToList())
            {
                foreach (var member in UserStatic.UserChatGroups[groupId].GroupMembers.ToList())
                {
                    if (member.ConnID == connId)
                    {
                        return Json( new { GroupID = groupId, GroupName = UserStatic.UserChatGroups[groupId].GroupName });
                    }
                }
            }

            return Json(new { GroupID = "", GroupName = "" });
        }
    }
}