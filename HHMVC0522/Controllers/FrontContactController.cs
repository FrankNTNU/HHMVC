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
    public class FrontContactController : Controller
    {
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
            Random rn = new Random();

            List<string> adminConnId = UserStatic.ConnectedUsers.Select(cu => new
            {
                cu.ConnID,
                UserID = int.Parse(cu.UserID)
            }).Where(cu => dbContext.Members.SingleOrDefault(cu1 => cu1.ID == cu.UserID).IsAdmin)
                .Select(cu => cu.ConnID).ToList();

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            //todo AI認知服務
            if (adminConnId.Count == 0)
            {
                string answer = await GetAnsFromKB(message);
                context.Clients.Client(connId).ReceiveFromService(answer);
            }
            else
            {
                context.Clients.Client(adminConnId[rn.Next(0, adminConnId.Count)])
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

    }
}