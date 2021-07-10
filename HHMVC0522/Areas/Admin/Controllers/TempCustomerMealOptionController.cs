using BLL;
using DTO;
using System;
using System.Collections.Generic;
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
    }
}