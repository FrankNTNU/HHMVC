using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Models;
using UI.ViewModels;

namespace UI.Controllers
{
    public class HHApi_DietLogsHistoryController : Controller
    {
        // GET: HHApi_DietLogsHistory
        DietLogBLL dlBLL = new DietLogBLL();
        MemberBLL mBLL = new MemberBLL();
        public PartialViewResult GetDateDietLogsByDateFlag(int memberId, int? flagAmount, DateTime? date)
        {
            string theDate=null;
            if (flagAmount != null) {
                theDate = DateTime.Now.AddDays((int)flagAmount).ToString(CDictionary.MMddyyyy);
            }
            if (date!= null)
            {
                theDate = ((DateTime)date).ToString(CDictionary.MMddyyyy);
            }
            DietLogsHistoryViewModel model = new DietLogsHistoryViewModel(memberId, theDate, true,true);
            PartialViewResult result = PartialView("_DietLogsHistoryPartial", model);
            

            return result;
        }

        public void DeleteDietLogByID(int dietLogId,bool IsCustomUploaded)
        {
            dlBLL.DeleteDietLog(dietLogId, IsCustomUploaded);
        }

        public void EditDietLogByID(DietLogEditDTO editDto)
        {
           dlBLL.EditDietLogByID(editDto);
        }

       
        public JsonResult GetHealthProfileByDate(int memberId, string date)
        {

            Member member = mBLL.GetMemberByMemberID(memberId);
            MemberForDietDTO mDto = new MemberForDietDTO(date)
            {
                MemberID = member.ID,
                Birthdate = member.Birthdate,
                ActivityLevelID = member.ActivityLevelID,
                Gender = member.Gender,
                Height = member.Height
            };
            MemberHealthProfile profile = new MemberHealthProfile(mDto, date,true);


            return Json(profile, JsonRequestBehavior.AllowGet);

        }


    }


    }