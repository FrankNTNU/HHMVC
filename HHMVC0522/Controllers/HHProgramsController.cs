using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.ViewModels;

namespace UI.Controllers
{
    public class HHProgramsController : Controller
    {
        // GET: HHPrograms

        ProgramBLL pBLL = new ProgramBLL();
      
        public ActionResult RegisterProgram()
        {

            int memberID = 17; 
            if (Session["ID"] != null)
            {
                memberID = (int)Session["ID"];
            }
            ProgramPageViewModel model = new ProgramPageViewModel(memberID);  //used?


            return View(model);

        }
       
        


    }
}