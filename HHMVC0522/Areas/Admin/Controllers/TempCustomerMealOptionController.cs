using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class TempCustomerMealOptionController : Controller
    {
        // GET: Admin/TempCustomerMealOption
        public ActionResult TempCustomerMealOptionList()
        {
            return View();
        }
        public ActionResult TempCustomerMealOptionListTest()
        {
            TempCustomerMealOptionBLL bll = new TempCustomerMealOptionBLL();
            return View(bll.GetTempCustomerMealOptions());
        }
        public ActionResult TempCustomerMealOptionListTest2()
        {
            TempCustomerMealOptionBLL bll = new TempCustomerMealOptionBLL();
            return View(bll.GetTempCustomerMealOptions());
        }
        public ActionResult TempCustomerMealOptionListTest3()
        {
            TempCustomerMealOptionBLL bll = new TempCustomerMealOptionBLL();
            return View(bll.GetTempCustomerMealOptions());
        }
        public ActionResult auditNotPassed(int ID)
        {
            TempCustomerMealOptionBLL bll = new TempCustomerMealOptionBLL();
            bll.auditNotPassed(ID);
            return RedirectToAction("TempCustomerMealOptionListTest3");
        }
        public ActionResult ReViwe(int ID)
        {
            TempCustomerMealOptionBLL bll = new TempCustomerMealOptionBLL();
            bll.ReView(ID);
            return RedirectToAction("TempCustomerMealOptionListTest3");
        }

        public ActionResult audit(int ID)
        {
            TempCustomerMealOptionBLL bll = new TempCustomerMealOptionBLL();
            AuditPassDTO dto = new AuditPassDTO();
            dto = bll.GetTempCustomerMealOption(ID);
            return View(dto);
        }
        [HttpPost]
        public ActionResult audit(AuditPassDTO dto)
        {
            if (dto.MealOptionUpLoadImage != null)
            {
                Bitmap image = new Bitmap(dto.MealOptionUpLoadImage.InputStream);
                Bitmap resizedImage = new Bitmap(image, 250, 250);
                string uniqueNumber = Guid.NewGuid().ToString();
                string fileName = uniqueNumber + dto.MealOptionUpLoadImage.FileName;
                resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/MealOptionImages/" + fileName));
                dto.MealOptionImage = fileName;
            }
            else
            {
                dto.MealOptionImage = "mealDefault.jpg";
            }
            MealBLL mealBLL = new MealBLL();
            NutrientBLL nutrientBLL = new NutrientBLL();
            TempCustomerMealOptionBLL bll = new TempCustomerMealOptionBLL();
            bll.AddToDB(dto);

            return RedirectToAction("TempCustomerMealOptionListTest3");
        }
        public ActionResult TempCustomerMealOptionListNotProceeded()
        {
            TempCustomerMealOptionBLL bll = new TempCustomerMealOptionBLL();
            return View(bll.GetTempCustomerMealOptionsNotProceeded());
        }
        public ActionResult TempCustomerMealOptionListPassed()
        {
            TempCustomerMealOptionBLL bll = new TempCustomerMealOptionBLL();
            return View(bll.GetTempCustomerMealOptionsPassed());
        }
        public ActionResult TempCustomerMealOptionListFailed()
        {
            TempCustomerMealOptionBLL bll = new TempCustomerMealOptionBLL();
            return View(bll.GetTempCustomerMealOptionsFailed());
        }
        public bool checkMaelName(string userInput)
        {
            MealBLL bll = new MealBLL();
            bool check = bll.checkMealName(userInput);
            return check;
        }
    }
}