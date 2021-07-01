using BLL;
using DAL;
using DTO;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//===============================
//For QnA
using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker;
using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker.Models;
using System.Threading.Tasks;

namespace UI.Controllers
{
    [System.Web.Mvc.Authorize]
    [System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class FrontContactController : AsyncController
    {
        HealthHelperEntities dbContext = new HealthHelperEntities();
        Random rn = new Random();

        // GET: FrontContact
        public ActionResult Index()
        {
            return View();
        }
      
        public ActionResult Chat()
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home2");
            }
            return View();
        }

        //=======================================================================
        //恩旗
        [HttpPost]
        public async Task<ActionResult> Chat(string connId, string message)
        {
            HealthHelperEntities dbContext = new HealthHelperEntities();
            
            List<string> adminConnIds = UserStatic.ConnectedUsers.Select(cu => new
            {
                cu.ConnID,
                UserID = int.Parse(cu.UserID)
            }).Where(cu => dbContext.Members.SingleOrDefault(cu1 => cu1.ID == cu.UserID).IsAdmin)
                .Select(cu => cu.ConnID).ToList();

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            //todo AI認知服務
            if (adminConnIds.Count == 0)
            {
                string answer = await GetAnsFromKB(message);
                context.Clients.Client(connId).ReceiveFromService(answer);
            }
            else
            {
                context.Clients.Client(adminConnIds[rn.Next(0, adminConnIds.Count)])
                    .ReceiveFromCustomer(connId, Session["UserName"], Session["ImagePath"], message);
            }    

            return View();
        }

        //==============================================================
        //恩旗
        //GetAnswers From Knowledge Base

        private async Task<string> GetAnsFromKB(string message) {

            string kbId = "acd4f593-dc0c-43ef-b658-77546c18ef5f";
            var authoringKey = "b2ab1e830f1548c38bec03719a4bcabf";
            var authoringURL = "https://msit130myqnamaker.cognitiveservices.azure.com/";
            var queryingURL = "https://msit130myqnamaker.azurewebsites.net";

            //===============================================================
            //Get QueryKey
            var client = new QnAMakerClient(new ApiKeyServiceClientCredentials(authoringKey))
            { Endpoint = authoringURL };

            var endpointKeysObject = await client.EndpointKeys.GetKeysAsync();
            var primaryQueryEndpointKey = endpointKeysObject.PrimaryEndpointKey;

            //================================================================
            //New RuntimeClient
            var runtimeClient = new QnAMakerRuntimeClient(new EndpointKeyServiceClientCredentials(primaryQueryEndpointKey))
            { RuntimeEndpoint = queryingURL };

            //================================================================
            //Get Answer from KB
            var response = await runtimeClient.Runtime
                .GenerateAnswerAsync(kbId, new QueryDTO { Question = message });

            return response.Answers[0].Answer;
        }

        [HttpPost]
        public JsonResult AddToServiceGroup(string connId)
        {
            //If already Reconnect to old Group
            foreach (var groupId in UserStatic.ServiceGroups.Keys.ToList())
            {
                if (UserStatic.ServiceGroups[groupId].GroupName == User.Identity.Name)
                {
                   return Json(new 
                   { 
                       Result = "Reconnect to old group", 
                       GroupId = groupId
                   });
                }
            }

            // Add To DB Groups Table
            Group group = dbContext.Groups.Add(new Group
            {
                GroupName = User.Identity.Name,
                StartWeight = 0,
                EndWeight = 0,
                IsAlive = true,
                IsService = true
            });

            dbContext.SaveChanges();

            //Online Admins List
            List<Tuple<string, int>> admins = UserStatic.ConnectedUsers.Select(cu => new
            {
                cu.ConnID,
                UserID = int.Parse(cu.UserID)
            }).Where(cu => dbContext.Members.SingleOrDefault(cu1 => cu1.ID == cu.UserID).IsAdmin)
                .Select(cu => new Tuple<string, int>(cu.ConnID, cu.UserID)).ToList();

            Tuple<string, int> admin = null;
            string adminConnId = "";
            int adminId = 0;

            if (admins.Count != 0)
            {
                admin = admins[rn.Next(0, admins.Count)];
                adminConnId = admin.Item1;
                adminId = admin.Item2;
            }
             
            //Add to ServiceGroup
            var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            Context.Groups.Add(connId, group.ID.ToString());

            if (adminConnId != "")
            {
                Context.Groups.Add(adminConnId, group.ID.ToString());
            }
                
            UserStatic.ServiceGroups.Add(group.ID.ToString(), new ServiceGroupDTO
            {
                GroupName = User.Identity.Name,
                UserConnId = connId,
                AdminConnId = adminConnId,
                AdminId = adminId.ToString()
            });

            return Json(new { Result = "Success", GroupId = group.ID.ToString() });
        }

        [HttpPost]
        public async Task SendMessage(string connId, string groupId, string message)
        {

            var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            Member member = dbContext.Members.SingleOrDefault(m => m.ID.ToString() == User.Identity.Name);

            DateTime timeStamp = DateTime.Now;

            Context.Clients.Group(groupId)
                .receive(connId, member.UserName, message, timeStamp.ToString("M/d HH:mm"), member.Image, groupId);
            
            dbContext.GroupChats.Add(new GroupChat
            {
                GroupID = int.Parse(groupId),
                MemberID = int.Parse(User.Identity.Name),
                Message = message,
                TimeStamp = timeStamp
            });

            dbContext.SaveChanges();

            //Online Admins List
            var admins = UserStatic.ConnectedUsers.Select(cu => new
            {
                cu.ConnID,
                UserID = int.Parse(cu.UserID)
            }).Where(cu => dbContext.Members.SingleOrDefault(cu1 => cu1.ID == cu.UserID).IsAdmin)
                .Select(cu => cu).ToList();

            if (admins.Count == 0)
            {
                Task<string> answerTask = GetAnsFromKB(message);

                await Task.Run(() => ReplyByOnAMaker(answerTask, groupId, timeStamp));
            }

        }

        public JsonResult GetGroupMsgs(string groupId)
        {
            var msgList = dbContext.GroupChats.Where(gc => gc.GroupID.ToString() == groupId
                && gc.Group.IsService)
                .OrderBy(gc => gc.TimeStamp).ToList()
                .Select(gc =>
                {
                    string connId = "";
                    string userName = "";
                    string image = "";

                    if (gc.MemberID == null)
                    {
                        return new
                        {
                            connId = "",
                            userName = "客服人員",
                            message = gc.Message,
                            timeStamp = gc.TimeStamp.ToString("M/d HH:mm"),
                            image = "e9ec5c93-c442-4d6d-96d1-fc2fb8c570fcuser2.png"
                        };
                    }
                    else
                    {

                        Member member = dbContext.Members.SingleOrDefault(m => m.ID == gc.MemberID);

                        if (member.IsAdmin)
                        {
                            connId = UserStatic.ServiceGroups[groupId].AdminConnId;
                            userName = "客服人員";
                            image = "e9ec5c93-c442-4d6d-96d1-fc2fb8c570fcuser2.png";
                        }
                        else
                        {
                            connId = UserStatic.ServiceGroups[groupId].UserConnId;
                            userName = member.UserName;
                            image = member.Image;
                        }

                        return new
                        {
                            connId = connId,
                            userName = userName,
                            message = gc.Message,
                            timeStamp = gc.TimeStamp.ToString("M/d HH:mm"),
                            image = image
                        };
                    }
                });

            return Json(msgList);
        }

        [HttpPost]
        public JsonResult IsConnected()
        {
            foreach (string groupId in UserStatic.ServiceGroups.Keys.ToList())
            {
                if (UserStatic.ServiceGroups[groupId].GroupName == User.Identity.Name)
                {
                    return Json(new { IsConnected = true });
                }
            }
            return Json(new { IsConnected = false });
        }

        [NonAction]
        private void ReplyByOnAMaker(Task<string> answerTask, string groupId, DateTime timeStamp)
        {
            string answer = answerTask.Result;

            var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            Context.Clients.Group(groupId)
                    .receive("", "客服人員", answer, timeStamp.ToString("M/d HH:mm"), "e9ec5c93-c442-4d6d-96d1-fc2fb8c570fcuser2.png", groupId);

            dbContext.GroupChats.Add(new GroupChat
            {
                GroupID = int.Parse(groupId),
                MemberID = null,
                Message = answer,
                TimeStamp = timeStamp
            });

            dbContext.SaveChanges();
        }

        [HttpPost]
        public void SetNotReadCountSession(int notReadCount)
        {
            Session["NotReadCount"] = notReadCount;
        }
    }
}