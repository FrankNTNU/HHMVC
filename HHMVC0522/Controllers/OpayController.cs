using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
//using OpayCredit.Models;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using ECPay.Payment.Integration;
using HttpMethod = ECPay.Payment.Integration.HttpMethod;
using DAL;

namespace UI.Controllers
{
    public class OpayController : Controller
    {
        // GET: Opay
        public ActionResult Index()
        {
            return View();
        }
        //發出訂單
        public  ActionResult Payment()
        {
            HttpClient client = new HttpClient();
            OpayCreateCreditOrder creditOrder = new OpayCreateCreditOrder();
            var contast = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
                /*"http://localhost/HealthHelper/FrontGift/GiftList";*/
            creditOrder.MerchantID = "2000132";
            creditOrder.MerchantTradeNo = "Hh" + DateTime.Now.ToString("yyyyMMddHHmmssff");
            creditOrder.MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            creditOrder.PaymentType = "aio";
            creditOrder.TotalAmount = 99;
            creditOrder.TradeDesc = "建立信用卡測試訂單";
            creditOrder.ItemName = "會員升級";
            creditOrder.ReturnURL = "https://healthhelper666.azurewebsites.net/Opay/PaidNotice";
            creditOrder.ChoosePayment = "Credit";
            creditOrder.ClientBackURL = contast;
            creditOrder.EncryptType = 1;
            
            //訂單資訊存入DB
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Order order = new Order();
                if (Session["ID"] != null)
                {
                    order.MemberID = (int)Session["ID"];
                    order.OrderNumber = creditOrder.MerchantTradeNo;
                    order.OrderTime = creditOrder.MerchantTradeDate;
                    order.OrderStatus = false;
                    order.PaidTime = "None";
                    db.Orders.Add(order);
                    db.SaveChanges();
                }
            }           
                return View("Payment",creditOrder);

                //轉Json
                // var creditOrderJson = JsonConvert.SerializeObject(creditOrder);
                // var content = new StringContent(creditOrderJson, UnicodeEncoding.UTF8, "application/json");
                // // content打包好在object中
                // // 呼叫API
                // var response = await client.PostAsync("https://payment-stage.opay.tw/Cashier/AioCheckOut/V5", content);
                // // 回傳
                // var responseString = await response.Content.ReadAsStringAsync();
                // // 存到DB

                //System.Diagnostics.Debug.WriteLine(responseString);
        }
        //儲值可加200點
        public readonly int newPremiumPoints = 1000;
        //14為儲值的代碼
        public readonly int newPremiumStatusID = 14;
        //完成付款後，呼叫PaidNotice
        //[HttpPost]
        public string PaidNotice(PaidNotice p)
        {
            string returnCode= "1|OK";
            try
            {
                using (HealthHelperEntities db = new HealthHelperEntities())
                {
                    Order order = db.Orders.FirstOrDefault(x => x.OrderNumber == p.MerchantTradeNo);
                    if (order != null)
                    {
                        int memberID = order.MemberID;
                        order.PaidTime =p.PaymentDate;
                        order.OrderStatus = true;                       
                        Member member = db.Members.FirstOrDefault(x => x.ID == memberID);
                        member.Points += newPremiumPoints;
                        member.IsVIP = true;
                        Point point = new Point();
                        point.MemberID = memberID;
                        point.GetPoints = newPremiumPoints;
                        point.GetPointsDateTime = DateTime.Now;
                        point.StatusID = newPremiumStatusID;
                        db.Points.Add(point);
                        db.SaveChanges();
                    }
                    
                    //    count+=1;
                }
            }
            catch(Exception ex)
            {
                returnCode = ex.Message;
            }
            //int count = 0;

            //歐付寶需要回傳"1|OK"來代表完成付款
            //if (count > 0)
            return returnCode;
            //else
            //    return "0|Error";
        }
    }             
}