﻿using DAL;
using DTO;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            int MemberID = (int)Session["ID"];

            decimal initialWeight = dbContext.Programs.SingleOrDefault(p => p.MemberID == MemberID
                && DbFunctions.TruncateTime(p.StartDate) <= DateTime.Today
                && DbFunctions.TruncateTime(p.EndDate) >= DateTime.Today
                && p.StatusID == 1).InitialWeight;

            if (weight1 < initialWeight - 5 || weight2 > initialWeight + 5)
            {
                return Json(new { Result = "體重區間必須介於挑戰計劃初始體重的±5kg" });
            }

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
            int MemberID = (int)Session["ID"];

            decimal initialWeight = dbContext.Programs.SingleOrDefault(p => p.MemberID == MemberID
                && DbFunctions.TruncateTime(p.StartDate) <= DateTime.Today
                && DbFunctions.TruncateTime(p.EndDate) >= DateTime.Today
                && p.StatusID == 1).InitialWeight;

            decimal startWeight = UserStatic.UserChatGroups[groupId].WeightRange.Item1;
            decimal endWeight = UserStatic.UserChatGroups[groupId].WeightRange.Item2;

            if (initialWeight < startWeight || initialWeight > endWeight)
            {
                return Json(new
                {
                    Result = "您的體重不在此群組的體重區間",
                    GroupID = "",
                    GroupName = ""
                });
            }

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
            //Sync Groups table with UserStatic.UserChatGroups
            //dbContext.Groups.Where(g => !UserStatic.UserChatGroups.ContainsKey(g.ID.ToString())).ToList()
            //    .ForEach(g =>
            //    {
            //        g.IsAlive = false;
            //    });
            //dbContext.SaveChanges();


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
            try
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
            catch (Exception ex)
            {
                string filePath = @"C:\Users\enchi\Desktop\Error.txt";

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(DateTime.Now.ToString("M/d HH:mm") + " Message : " + ex.Message);
                }
            }
        }

        [HttpPost]
        public JsonResult GetGroupMembers(string groupId)
        {

            var gmList = UserStatic.UserChatGroups[groupId].GroupMembers.Select(gm => gm.UserID).ToList();

            var nameList = gmList.Select(gm =>
            {
                return dbContext.Members.SingleOrDefault(m => m.ID.ToString() == gm).UserName;
            }).Distinct().OrderBy(n => n);

            return Json(nameList);
        }

        [HttpPost]
        public void SendMessage(string connId, string groupId, string message)
        {

            var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            string UserId = UserStatic.UserChatGroups[groupId].GroupMembers
                .SingleOrDefault(gm => gm.ConnID == connId).UserID;

            Member member = dbContext.Members.SingleOrDefault(m => m.ID.ToString() == UserId);

            List<string> connIds = UserStatic.UserChatGroups[groupId].GroupMembers
                .Where(gm => gm.UserID == UserId).Select(gm => gm.ConnID).ToList();

            DateTime timeStamp = DateTime.Now;

            Context.Clients.Group(groupId)
                .receiveFromGroupMember(connIds, member.UserName, message, timeStamp.ToString("M/d HH:mm"), member.Image);

            //todo
            dbContext.GroupChats.Add(new GroupChat
            {
                GroupID = int.Parse(groupId),
                MemberID = int.Parse(UserId),
                Message = message,
                TimeStamp = timeStamp
            });

            dbContext.SaveChanges();

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

        public JsonResult GetGroupMsgs(string groupId)
        {
            int gId = int.Parse(groupId);

            var msgList = dbContext.GroupChats.Where(gc => gc.GroupID == gId).OrderBy(gc => gc.TimeStamp).ToList()
                .Select(gc => 
                {
                    List<string> connIds = UserStatic.UserChatGroups[groupId].GroupMembers
                        .Where(gm => gm.UserID == gc.MemberID.ToString())
                        .Select(gm => gm.ConnID).ToList();

                    Member member = dbContext.Members.SingleOrDefault(m => m.ID == gc.MemberID);

                    return new
                    {
                        connIds = connIds,
                        userName = member.UserName,
                        message = gc.Message,
                        timeStamp = gc.TimeStamp.ToString("M/d HH:mm"),
                        image = member.Image
                    };
                });

            return Json(msgList);
        }
    }
}