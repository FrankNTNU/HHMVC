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
using System.Net.Mail;

namespace UI.Controllers
{
    [System.Web.Mvc.Authorize]
    [System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class FrontContactController : AsyncController
    {
        HealthHelperEntities dbContext = new HealthHelperEntities();
        

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

        //==============================================================
        //恩旗
        //GetAnswers From Knowledge Base

        private async Task<QnASearchResult> GetAnsFromKB(string message, string qnaId) {

            string kbId = "37836933-78da-43c1-a7b4-638fc07e773a";
            var authoringKey = "7b80c902130142feb525e207863c6925";
            var authoringURL = "https://msit130myqnamakerservice.cognitiveservices.azure.com/";
            var queryingURL = "https://msit130myappserviceforqnamaker.azurewebsites.net";

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
            if (message != null)
            {
                var response = await runtimeClient.Runtime
                    .GenerateAnswerAsync(kbId, new QueryDTO { Question = message });

                return response.Answers[0];
            }
            else
            {
                var response = await runtimeClient.Runtime
                    .GenerateAnswerAsync(kbId, new QueryDTO { QnaId = qnaId });

                return response.Answers[0];
            }

            
        }

        //private async Task<QnASearchResult> GetAnsByQnaId(string qnaId)
        //{

        //    string kbId = "37836933-78da-43c1-a7b4-638fc07e773a";
        //    var authoringKey = "7b80c902130142feb525e207863c6925";
        //    var authoringURL = "https://msit130myqnamakerservice.cognitiveservices.azure.com/";
        //    var queryingURL = "https://msit130myappserviceforqnamaker.azurewebsites.net";

        //    //===============================================================
        //    //Get QueryKey
        //    var client = new QnAMakerClient(new ApiKeyServiceClientCredentials(authoringKey))
        //    { Endpoint = authoringURL };

        //    var endpointKeysObject = await client.EndpointKeys.GetKeysAsync();
        //    var primaryQueryEndpointKey = endpointKeysObject.PrimaryEndpointKey;

        //    //================================================================
        //    //New RuntimeClient
        //    var runtimeClient = new QnAMakerRuntimeClient(new EndpointKeyServiceClientCredentials(primaryQueryEndpointKey))
        //    { RuntimeEndpoint = queryingURL };

        //    //================================================================
        //    //Get Answer from KB
        //    var response = await runtimeClient.Runtime
        //        .GenerateAnswerAsync(kbId, new QueryDTO { QnaId = qnaId });

        //    return response.Answers[0];
        //}

        [HttpPost]
        public JsonResult AddToServiceGroup(string connId)
        {
            //If already Reconnect to old Group
            foreach (var groupId in UserStatic.ServiceGroups.Keys.ToList())
            {
                if (UserStatic.ServiceGroups[groupId].GroupName == User.Identity.Name)
                {
                    int notReadCount = 0;

                    if (UserStatic.ServiceNotRead.ContainsKey(User.Identity.Name))
                    {
                        UserStatic.ServiceNotRead[User.Identity.Name].TryGetValue(groupId, out notReadCount);
                    }

                    return Json(new 
                    { 
                        Result = "Reconnect to old group", 
                        GroupId = groupId,
                        NotReadCount = notReadCount
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
            var admins = UserStatic.ConnectedUsers
                .Where(cu => cu.Role == "admin" && cu.UserID != User.Identity.Name)
                .Select(cu =>
                {
                    int serviceCount = 0;

                    foreach (var groupId in UserStatic.ServiceGroups.Keys.ToList())
                    {
                        if (UserStatic.ServiceGroups[groupId].AdminId == cu.UserID)
                        {
                            serviceCount++;
                        }
                    }
                    return new { cu, serviceCount };
                });


            string adminConnId = "";
            string adminId = "";

            if (admins.Any())
            {
                var admin = admins.Aggregate((minServeAdmin, nextAdmin)
                    => minServeAdmin.serviceCount <= nextAdmin.serviceCount ? minServeAdmin : nextAdmin);

                adminConnId = admin.cu.ConnID;
                adminId = admin.cu.UserID;
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
                AdminId = adminId
            });

            return Json(new { Result = "Success", GroupId = group.ID.ToString(), NotReadCount = 0 });
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
            var admins = UserStatic.ConnectedUsers
                .Where(cu => cu.Role == "admin" && cu.UserID != User.Identity.Name)
                .Select(cu => cu).ToList();

            if (admins.Count == 0)
            {
                //QnA Maker replys
                Task<QnASearchResult> answerTask = GetAnsFromKB(message, null);
                await Task.Run(() => ReplyByOnAMaker(answerTask, groupId, timeStamp));
            }

        }

        public JsonResult GetGroupMsgs(string groupId)
        {
            var msgList = dbContext.GroupChats.Where(gc => gc.GroupID.ToString() == groupId
                    && gc.Group.IsAlive && gc.Group.IsService)
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
                        UserDetail admin = UserStatic.ConnectedUsers
                            .SingleOrDefault(cu => cu.ConnID == UserStatic.ServiceGroups[groupId].AdminConnId
                                && cu.UserID == gc.MemberID.ToString());

                        UserDetail customer = UserStatic.ConnectedUsers
                            .SingleOrDefault(cu => cu.ConnID == UserStatic.ServiceGroups[groupId].UserConnId
                                && cu.UserID == gc.MemberID.ToString());

                        if (admin != null || customer == null)
                        {
                            connId = UserStatic.ServiceGroups[groupId].AdminConnId;
                            userName = "客服人員";
                            image = "e9ec5c93-c442-4d6d-96d1-fc2fb8c570fcuser2.png";
                        }
                        else
                        {
                            Member member = dbContext.Members.SingleOrDefault(m => m.ID == gc.MemberID);
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
        private void ReplyByOnAMaker(Task<QnASearchResult> answerTask, string groupId, DateTime timeStamp)
        {
            int notReadCount = IncreaseNotReadCount(groupId);

            QnASearchResult answer = answerTask.Result;

            var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            Context.Clients.Group(groupId)
                    .receive("", "客服人員", answer.Answer, timeStamp.ToString("M/d HH:mm")
                        , "e9ec5c93-c442-4d6d-96d1-fc2fb8c570fcuser2.png", groupId
                        , notReadCount);

            dbContext.GroupChats.Add(new GroupChat
            {
                GroupID = int.Parse(groupId),
                MemberID = null,
                Message = answer.Answer,
                TimeStamp = timeStamp
            });

            dbContext.SaveChanges();

            ShowPrompt(answer, groupId);

            //======================================================================
            //send email to admin

            //var groupMsgs = dbContext.GroupChats.Where(gc => gc.GroupID.ToString() == groupId).ToList();

            //int QnAMakerReplyCount = groupMsgs.Where(gm => gm.MemberID == null).Count();

            //if (QnAMakerReplyCount % 3 == 0)
            //{
            //    string to = "enchi.dong@hotmail.com.tw";
            //    string from = "brandon.dong0207@gmail.com";
            //    string subject = "有會員需要您的服務";
            //    string body = "管理員您好，有會員需要您的服務，對話如下：\n";

            //    foreach (var msg in groupMsgs)
            //    {
            //        body += "\t" + msg + "\n";
            //    }

            //    MailMessage emailMessage = new MailMessage(from, to, subject, body);

            //    SmtpClient client = new SmtpClient();
            //    client.EnableSsl = true;
            //    client.Send(emailMessage);
            //}

        }

        //when user click prompt
        [HttpPost]
        public async Task SelectPrompt(string connId, string groupId, string qnaId) {
            
            Task<QnASearchResult> promptTask = GetAnsFromKB(null, qnaId);

            Member member = dbContext.Members.SingleOrDefault(m => m.ID.ToString() == User.Identity.Name);

            DateTime timeStamp = DateTime.Now;

            var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            QnASearchResult promptQna = await promptTask;

            Context.Clients.Group(groupId)
                .receive(connId, member.UserName, promptQna.Questions[0], timeStamp.ToString("M/d HH:mm"), member.Image, groupId);

            dbContext.GroupChats.Add(new GroupChat
            {
                GroupID = int.Parse(groupId),
                MemberID = int.Parse(User.Identity.Name),
                Message = promptTask.Result.Questions[0],
                TimeStamp = timeStamp
            });

            int notReadCount = IncreaseNotReadCount(groupId);

            Context.Clients.Group(groupId)
                    .receive("", "客服人員", promptQna.Answer, timeStamp.ToString("M/d HH:mm")
                        , "e9ec5c93-c442-4d6d-96d1-fc2fb8c570fcuser2.png", groupId
                        , notReadCount);

            dbContext.GroupChats.Add(new GroupChat
            {
                GroupID = int.Parse(groupId),
                MemberID = null,
                Message = promptQna.Answer,
                TimeStamp = timeStamp
            });

            dbContext.SaveChanges();

            ShowPrompt(promptQna, groupId);
        }

        [HttpPost]
        public void ResetNotReadCount(string groupId)
        {
            if (UserStatic.ServiceNotRead.ContainsKey(User.Identity.Name)
                && UserStatic.ServiceNotRead[User.Identity.Name].ContainsKey(groupId))
            {
                UserStatic.ServiceNotRead[User.Identity.Name][groupId] = 0;
            }
            else if (UserStatic.ServiceNotRead.ContainsKey(User.Identity.Name)
                && !UserStatic.ServiceNotRead[User.Identity.Name].ContainsKey(groupId))
            {
                UserStatic.ServiceNotRead[User.Identity.Name].Add(groupId, 0);
            }
            else
            {
                UserStatic.ServiceNotRead.Add(User.Identity.Name, new Dictionary<string, int>());
                UserStatic.ServiceNotRead[User.Identity.Name].Add(groupId, 0);
            }
        }

        private int IncreaseNotReadCount(string groupId) {
            //===================================================
            //ServiceNotRead
            string user = UserStatic.ServiceGroups[groupId].GroupName;

            if (UserStatic.ServiceNotRead.ContainsKey(user)
                && UserStatic.ServiceNotRead[user].ContainsKey(groupId))
            {
                UserStatic.ServiceNotRead[user][groupId]++;
            }
            else if (UserStatic.ServiceNotRead.ContainsKey(user)
                && !UserStatic.ServiceNotRead[user].ContainsKey(groupId))
            {
                UserStatic.ServiceNotRead[user].Add(groupId, 1);
            }
            else
            {
                UserStatic.ServiceNotRead.Add(user, new Dictionary<string, int>());
                UserStatic.ServiceNotRead[user].Add(groupId, 1);
            }

            return UserStatic.ServiceNotRead[user][groupId];
            //==================================================================
        }

        private void ShowPrompt(QnASearchResult answer, string groupId)
        {
            var Context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            if (answer.Context != null)
            {
                foreach (var prompt in answer.Context.Prompts)
                {
                    Context.Clients.Group(groupId)
                        .receivePrompt(prompt.DisplayText, prompt.QnaId);
                }
            }
        }
    }
}