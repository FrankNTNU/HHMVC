﻿using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace UI.Controllers
{
    public class Home2Controller : Controller
    {
        // GET: Home2
        LayoutBLL layoutBLL = new LayoutBLL();
        PostBLL postBLL = new PostBLL();
        UserBLL userBLL = new UserBLL();
        public ActionResult Index()
        {
            LayoutDTO layoutDTO = new LayoutDTO();
            layoutDTO = layoutBLL.GetPosts();

            return View(layoutDTO);
        }

        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            UserDTO dto = new UserDTO();
            return View(dto);
        }

        [HttpPost]
        public ActionResult Login(UserDTO model)
        {
            if (model.UserName != null && model.Password != null)
            {
                UserDTO user = userBLL.GetUserWithUsernameAndPassword(model);
                if (user.ID != 0)
                {
                    Session["ID"] = user.ID;
                    Session["Name"] = user.Name;
                    Session["ImagePath"] = user.ImagePath;
                    Session["UserName"] = user.UserName;

                    //====================================
                    //恩旗
                    //Use RedirectFromLoginPage to redirect
                    FormsAuthentication.RedirectFromLoginPage(user.ID.ToString(), false);
                    
                    //var i = User.Identity.Name;
                    //return RedirectToAction("Index", "Home2");
                    return null;
                    //====================================
                }
                else
                {
                    ViewBag.ProcessState = General.Messages.LoginError;
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult PostDetail(int ID)
        {
            layoutBLL = new LayoutBLL();
            LayoutDTO layoutDTO = new LayoutDTO();
            layoutDTO = layoutBLL.GetPostDetailPageItemWithID(ID);
            return View(layoutDTO);
        }

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult PostDetail(LayoutDTO model)
        {

            if (model.Comment.Name != null && model.Comment.Title != null && model.Comment.CommentContent != null)
            {
                if (postBLL.AddComment(model))
                {
                    ViewData["CommentState"] = "Success";
                    ModelState.Clear();
                }
                else
                {
                    ViewData["CommentState"] = "Error";
                    ViewBag.ProcessState = General.Messages.GeneralError;

                }
            }
           
            else
            {
                ViewData["CommentState"] = "Error";
                ViewBag.ProcessState = General.Messages.EmptyArea;

            }
            LayoutDTO layoutDTO = new LayoutDTO();
            layoutDTO = layoutBLL.GetPostDetailPageItemWithID(model.PostDetail.ID);
            return View(layoutDTO);
        }
        CommentBLL commentBLL = new CommentBLL();
        public ActionResult DeleteComment(int ID, int postID)
        {
            commentBLL.DeleteComment(ID);
            ViewData["CommentState"] = "Success";
            ModelState.Clear();
            return RedirectToAction("PostDetail/" + postID, "Home2");
        }

        //========================================================
        //恩旗
        //For dialog
        [HttpPost]
        public void SetDialogShownSession() {
            Session["dialogShown"] = true;

            if (Request.Cookies["PointShown"] == null)
            {
                HttpCookie cook = new HttpCookie("PointShown");
                cook.Value = "true";
                cook.Expires = DateTime.Now.AddDays(1).Date;
                Response.Cookies.Add(cook);
            }
        }

        //determine if last 7 days has no weight log
        [NonAction]
        public static void SetWeightLogSession(HttpContextBase context)
        {
            HealthHelperEntities dbContext = new HealthHelperEntities();
            DateTime today = DateTime.Now;
            DateTime d7b = DateTime.Now.Date.AddDays(-7);

            int MemberID = (int)context.Session["ID"];

            var q1 = dbContext.WeightLogs.Where(wgt => wgt.MemberID == MemberID
                && wgt.UpdatedDate <= today && wgt.UpdatedDate >= d7b);
            List<WeightLog> list = q1.ToList();
            if (list.Count == 0)
            {
                context.Session["NoWeightLog"] = true;
            }
            else
            {
                context.Session["NoWeightLog"] = false;
            }
        }

        //determine if there is any WorkoutPreferences
        [NonAction]
        public static void SetPreferencesSession(HttpContextBase context)
        {
            HealthHelperEntities dbContext = new HealthHelperEntities();

            int MemberID = (int)context.Session["ID"];

            var q1 = dbContext.WorkoutPreferences.Where(wp => wp.MemberID == MemberID);

            if (!q1.Any())
            {
                context.Session["NoPreferences"] = true;
            }
            else
            {
                context.Session["NoPreferences"] = false;
            }
        }

        //determine if user gets points in yesterday's settlement
        public static void SetPointsSession(HttpContextBase context)
        {
            HealthHelperEntities dbContext = new HealthHelperEntities();

            int MemberID = (int)context.Session["ID"];

            DateTime yesterday = DateTime.Now.Date.AddDays(-1).Date;

            var yesterdayPoints = dbContext.Points.Where(pts => pts.MemberID == MemberID
                && DbFunctions.TruncateTime(pts.GetPointsDateTime) == yesterday)
                .Select(pts => pts.GetPoints).DefaultIfEmpty(0).Sum();

            context.Session["GotPoints"] = yesterdayPoints;
            
        }

        //todo
        //determine if a program is frozen, which needs a resultWeight
        public static void SetProgramFrozenSession(HttpContextBase context)
        {
            HealthHelperEntities dbContext = new HealthHelperEntities();

            int MemberID = (int)context.Session["ID"];

            var program = dbContext.Programs.SingleOrDefault(prg => prg.MemberID == MemberID
                && prg.StatusID == 3);

            if (program != null)
            {
                context.Session["ProgramFrozen"] = 7 - (DateTime.Today - program.EndDate.Date).Days;
            }
            else
            {
                context.Session["ProgramFrozen"] = -1;
            }
        }

        [HttpPost]
        //Set ResultWeight and end the program
        public JsonResult SetResultWeight(int resultWeight) {

            HealthHelperEntities dbContext = new HealthHelperEntities();

            int MemberID = (int)Session["ID"];

            Program program = dbContext.Programs.SingleOrDefault(prg => prg.MemberID == MemberID
                && prg.StatusID == 3);

            Session["ProgramFrozen"] = false;

            decimal weightToLose = program.InitialWeight - program.TargetWeight;
            decimal actuallyLose = program.InitialWeight - resultWeight;

            int GetPoints = (int)(500 + 500 * actuallyLose / weightToLose);

            if (actuallyLose > 0)
            {
                dbContext.Points.Add(new Point
                {
                    MemberID = MemberID,
                    GetPoints = GetPoints,
                    GetPointsDateTime = DateTime.Now,
                    StatusID = 10,
                    ProgramID = program.ID
                });
            }
            else
            {
                dbContext.Points.Add(new Point
                {
                    MemberID = MemberID,
                    GetPoints = 500,
                    GetPointsDateTime = DateTime.Now,
                    StatusID = 10,
                    ProgramID = program.ID
                });
            }

            program.ResultWeight = resultWeight;
            program.StatusID = 5;

            dbContext.SaveChanges();

            return Json(new { Result = "Success", GetPoints = GetPoints });
            
        }
        //=========================================================
    }
}