using BLL;
using DAL;
using DTO;
using DTO.ViewModels;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Models;
using UI.ViewModels;

namespace UI.Controllers
{
    public class HHApi_DietLogController : Controller
    {


        // GET: HHApi_DietLog
        DietLogBLL dlBLL = new DietLogBLL();
        MealBLL mBLL = new MealBLL();
        TimeOfDayBLL todBLL = new TimeOfDayBLL();
        ProgramBLL pBLL = new ProgramBLL();
        CustomerMealBLL cmBLL = new CustomerMealBLL();
        MemberBLL mbBLL = new MemberBLL();
        private string _today = DateTime.Now.ToString(CDictionary.MMddyyyy);



        [HttpPost]
        public JsonResult GetMealNameByKeyword(string mealKeyword)
        
        {
           
            return Json(mBLL.GetMealsByKeyword(mealKeyword));
        }




        public JsonResult QueryByMealKeyword(string mealNameKeyword, int memberID)
        {

            var q = dlBLL.GetDietLogsByKeyword(mealNameKeyword, memberID);
            List<DietLogCartItemViewModel> list = new List<DietLogCartItemViewModel>();
            foreach (DietLog dl in q)
            {
                DietLogCartItemViewModel dlItemViewModel = new DietLogCartItemViewModel(dl.MealOption);
                dlItemViewModel.Date = dl.Date;
                
                dlItemViewModel.Portion = dl.Portion;
                dlItemViewModel.TimeOfDayID = dl.TimeOfDayID;
                dlItemViewModel.IsValid = dl.IsValid;
                list.Add(dlItemViewModel);
            }         
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTimesOfDay()
        {
            List<TimesOfDayViewModel> todVModels = new List<TimesOfDayViewModel>();
            foreach (TimesOfDay tod in todBLL.GetTimeOfDays())
            {
                TimesOfDayViewModel model = new TimesOfDayViewModel(tod);
                todVModels.Add(model);
            }
            return Json(todVModels, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetMealInfoByID(int id, int memberId)
        {
            var meal = mBLL.GetMealByID(id);
            MealOptionViewModel mealViewModel = new MealOptionViewModel(meal, memberId);

            return Json(mealViewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LikeMealToggle(int memberId, int mealId )
        {
            var isLiked = mBLL.ToggleIsLikedMeal(memberId, mealId);

            return Json(isLiked, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetLikedList(int memberId)
        {
            List<MealOptionViewModel> mealVModels = new List<MealOptionViewModel>();

            foreach (MealOption ml in mBLL.GetLikedMeals(memberId))
            {
                MealOptionViewModel model = new MealOptionViewModel(ml, memberId);
                mealVModels.Add(model);

            }

            return Json(mealVModels, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetDietLogsByDate(int memberId, string date)
        //{
        //    List<DietLogViewModel> dietLogVModels = new List<DietLogViewModel>();

        //    foreach (DietLog dl in dlBLL.GetDietLogsByDate(memberId, date))
        //    {
        //        DietLogViewModel model = new DietLogViewModel(dl);
        //        dietLogVModels.Add(model);
        //    }

        //    return Json(dietLogVModels, JsonRequestBehavior.AllowGet);
        //}

       

        public JsonResult GetDietLogsToday(int memberId)   // add fram info
        {

            Member member = mbBLL.GetMemberByMemberID(memberId);
            MemberForDietDTO mDto = new MemberForDietDTO(DateTime.Now.ToString(CDictionary.MMddyyyy))
            {
                MemberID = member.ID,
                Birthdate = member.Birthdate,
                ActivityLevelID = member.ActivityLevelID,
                Gender = member.Gender,
                Height = member.Height
            };


            List<DietLogViewModel> dietLogVModels = new List<DietLogViewModel>();
            foreach (DietLog dl in dlBLL.GetDietLogsToday(mDto.MemberID))
            {

                dietLogVModels.Add(new DietLogViewModel(dl));
            }
            foreach (TempCustomerMealOption dl in cmBLL.GetCustomerMealByDate(mDto.MemberID, _today))
            {               
                dietLogVModels.Add(new DietLogViewModel(dl));
            }
             dietLogVModels.OrderBy(vm => vm.TimesOfDayID).ThenBy(vm => vm.MealTotalGainedCal);

            MemberDietReport report = new MemberDietReport() { DietLogs = dietLogVModels, DietProfile = new MemberHealthProfile(mDto, _today, false) };

            return Json(report, JsonRequestBehavior.AllowGet);
        }

        //===================================

        //public JsonResult AddToSession(int mealID)
        //{
        //    MealOption theMeal = mBLL.GetMealByID(mealID);
        //    List<DietLogCartItemViewModel> list = Session[CDictionary.KEY_DIETLOGCART_ITEMS] as List<DietLogCartItemViewModel>;
        //    if (theMeal != null)
        //    {

        //        if (list == null)
        //        {
        //            list = new List<DietLogCartItemViewModel>();
        //        }
        //        DietLogCartItemViewModel item = new DietLogCartItemViewModel(theMeal);
        //        item.AddedTime = DateTime.Now.ToString(CDictionary.yyyyMMddHHmmss);
        //        item.Date = DateTime.Now.ToString(CDictionary.MMddyyyy);
        //        item.Portion = 2;  //todo
        //        item.TimeOfDayID = 1;  //todo
        //        list.Add(item);
        //        Session[CDictionary.KEY_DIETLOGCART_ITEMS] = list;
        //        list = Session[CDictionary.KEY_DIETLOGCART_ITEMS] as List<DietLogCartItemViewModel>;
        //    }
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}


        //public JsonResult GetTempDietLogSession()
        //{

            
        //    return Json(Session[CDictionary.KEY_DIETLOGCART_ITEMS], JsonRequestBehavior.AllowGet);

        //}

        public JsonResult UpdateSession(MixedDietLogDTO[] items)
        {

            Session[CDictionary.KEY_DIETLOGCART_ITEMS] = items;
            return Json(Session[CDictionary.KEY_DIETLOGCART_ITEMS], JsonRequestBehavior.AllowGet);

        }
        public JsonResult AddItemToSession(MixedDietLogDTO item)
        {
            MixedDietLogDTO[] oldArr = Session[CDictionary.KEY_DIETLOGCART_ITEMS] as MixedDietLogDTO[];
            if (oldArr==null)
            {
                MixedDietLogDTO[] arr = { item };
                Session[CDictionary.KEY_DIETLOGCART_ITEMS] = arr;
            }
            else 
            {
                MixedDietLogDTO[] newArr = new MixedDietLogDTO[oldArr.Length + 1];
                int i;
                for ( i = 0; i < newArr.Length-1; i++)
                {
                    newArr[i] = oldArr[i];
                }
                newArr[i] = item;
                Session[CDictionary.KEY_DIETLOGCART_ITEMS] = newArr;
            }
            return Json(Session[CDictionary.KEY_DIETLOGCART_ITEMS], JsonRequestBehavior.AllowGet);


        }




        public JsonResult GetSession()
        {
            if (Session[CDictionary.KEY_DIETLOGCART_ITEMS] == null)
            {
                Session[CDictionary.KEY_DIETLOGCART_ITEMS] = 0;
            }
            return Json(Session[CDictionary.KEY_DIETLOGCART_ITEMS], JsonRequestBehavior.AllowGet);
        }


        public JsonResult SentTempListToDB(MixedDietLogDTO[] mixedDietLogs)
        {
            foreach (MixedDietLogDTO item in mixedDietLogs)
            {
                if (item.MealOptionID > 0)
                {
                    DietLogDTO dto = new DietLogDTO(item);
                    dlBLL.Add(dto);
                }
                else 
                {   
                    TempCustomerMealDTO dto = new TempCustomerMealDTO(item);



                    cmBLL.Add(dto);
                }
        
            }
            return GetDietLogsToday(mixedDietLogs[0].MemberID);

        }

        //private MixedDietLogDTO uploadPhoto(MixedDietLogDTO p)
        //{
        //    string photoName = Guid.NewGuid().ToString() + ".jpg";
        //    p.ImagePath = @"Content\Images\CustomerMealUploadImages\" + photoName;
        //    p.photo.CopyTo(new FileStream(host.WebRootPath + @"\Images\" + photoName, FileMode.Create));
        //    return p;
        //}




        public JsonResult GetMealsByTDEE(MemberHealthProfile memberHProfile, int mealTagID) 
        {
            

            int GainedCal = (int)dlBLL.GetGainedCalByDate(DateTime.Now.ToString(CDictionary.MMddyyyy), memberHProfile.MemberID);

            int maxCal = memberHProfile.TDEE - GainedCal;

            List<MealOptionViewModel> list = new List<MealOptionViewModel>();
            if (maxCal > 0) {
                foreach (MealOption m in mBLL.GetTagMealsUnderMaximumCal(mealTagID, maxCal))
                {
                    MealOptionViewModel model = new MealOptionViewModel(m, memberHProfile.MemberID);
                    list.Add(model);
                }
            }          
            return Json(list, JsonRequestBehavior.AllowGet);


        }

        public JsonResult GetMealsByProOrLight(MemberHealthProfile memberHProfile, int mealTagID)
        {


           //todo
            int ProOrLightmaxCal = pBLL.HasActiveProgram(memberHProfile.MemberID) == true ? 1600/*pBLL.ProgramMaximumCal()*/ : (int)((double)memberHProfile.TDEE* CDictionary.LightMealCoefficient);   //todo
            int GainedCal = (int)dlBLL.GetGainedCalByDate(DateTime.Now.ToString(CDictionary.MMddyyyy), memberHProfile.MemberID);
            int maxCal = ProOrLightmaxCal - GainedCal;


            List<MealOptionViewModel> list = new List<MealOptionViewModel>();
            if (maxCal > 0)
            {

                foreach (MealOption m in mBLL.GetTagMealsUnderMaximumCal(mealTagID, maxCal))
                {
                    MealOptionViewModel model = new MealOptionViewModel(m, memberHProfile.MemberID);
                    list.Add(model);
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }



        public PartialViewResult GetMealsByTag(int mealTagID, int memberId)
        {
            List<MealOptionViewModel> list = new List<MealOptionViewModel>();

            foreach (MealOption m in mBLL.GetMealsByTagID(mealTagID))
            {
                MealOptionViewModel model = new MealOptionViewModel(m, memberId);
                list.Add( model);
            }
            return PartialView("_MealOptionViewModelPartial", list);
        }

        //public JsonResult GetMealIDsByTag(int mealTagID)
        //{
        //    List<int> list = new List<int>();

        //    foreach (MealOption m in mBLL.GetMealsByTagID(mealTagID))
        //    {
        //        list.Add(m.ID);

        //    }
        //    return Json(list, JsonRequestBehavior.AllowGet);

        //}

        //public PartialViewResult GetMealPartialView(int mealID)
        //{


        //    return PartialView("_MealOptionViewModelPartial", new MealOptionViewModel (mBLL.GetMealByID(mealID)));

        //}




    }
}