using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using DTO.ViewModels;
using UI.ViewModels;
using UI.Models;

namespace UI.Controllers
{
    public class HHDietController : Controller
    {
        DietLogBLL dlBLL = new DietLogBLL();
        MealBLL mBLL = new MealBLL();
        MealTagBLL mtBLL = new MealTagBLL();
        TimeOfDayBLL todBLL = new TimeOfDayBLL();
        ProgramBLL pBLL = new ProgramBLL();
        MemberBLL mbBLL = new MemberBLL();
        // GET: HHDietLog



        //public ActionResult DeleteDietLogCartItem(int id, string addedTime)
        // {
        //     //MealOption theMeal = mBLL.GetMealByID(id);

        //         List<DietLogCartItemViewModel> list = Session[CDictionary.KEY_DIETLOGCART_ITEMS] as List<DietLogCartItemViewModel>;
        //     if (list != null)
        //     {
        //         for(int i= 0;i< list.Count; i++)
        //         {

        //             if (list[i].MealOptID == id && list[i].AddedTime == addedTime)
        //             {
        //                 list.RemoveAt(i);
        //                 break;
        //             }
        //         }

        //     }

        //     return RedirectToAction("Create");
        // }
        public ActionResult Create()
        {
            int memberID = 17; //todo  test
            if (Session["ID"] != null)
            {
                memberID = (int)Session["ID"];
            }

            Member member = mbBLL.GetMemberByMemberID(memberID);
            Program program = pBLL.GetCurrentProgram(memberID);

            MemberForDietDTO mDto = new MemberForDietDTO(DateTime.Now.ToString(CDictionary.MMddyyyy))
            {
                MemberID = member.ID,
                Birthdate = member.Birthdate,
                ActivityLevelID = member.ActivityLevelID,
                Gender = member.Gender,
                Height = member.Height,
                
            };
            if (program != null)
            {
                mDto.ActivityLevelID = program.ActivityLevelID;
                mDto.Program = new ProgramDTO()
                {
                    Name = program.Name,
                    StartDate = program.StartDate,
                    EndDate = program.EndDate,
                    TargetWeight = program.TargetWeight,
                    InitialWeight = program.InitialWeight,

                };
            }




            CreateDietLogViewModel model = new CreateDietLogViewModel(mDto);

            model.TempDietLogs = Session[CDictionary.KEY_DIETLOGCART_ITEMS] as MixedDietLogDTO[];
            //==============
            return View(model);
        }


        public ActionResult MealStyleList(int id)
        {
            int memberID = 17; //todo  test
            if (Session["ID"] != null)
            {
                memberID = (int)Session["ID"];
            }
            Program program = pBLL.GetCurrentProgram(memberID);

            MealStyleListViewModel model = new MealStyleListViewModel(memberID, id, program);
            return View(model);
        }


        public ActionResult test()
        {
            return View();
        }

        public ActionResult WaterLog()
        {
            return View();
        }


    }
}