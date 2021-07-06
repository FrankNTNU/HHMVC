using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using DTO;

namespace UI.Controllers
{
    public class FrontPointController : Controller
    {
        // GET: FrontPoint
        public ActionResult Index()
        {
            return View();
        }
        public static int pageSize = 10;
        [HttpPost]
        public JsonResult GetLogs(int pageIndex, string sortName, string sortDirection, string timeRange)
        {
            int userID = (int)Session["ID"];
            HealthHelperEntities db = new HealthHelperEntities();
            DateTime timeConstrait = DateTime.Today.AddDays(-30);
            switch (timeRange)
            {
                case "today":
                    timeConstrait = DateTime.Today;
                    break;
                case "pastThreeDays":
                    timeConstrait = DateTime.Today.AddDays(-3);
                    break;
                case "pastWeek":
                    timeConstrait = DateTime.Today.AddDays(-7);
                    break;
                case "pastMonth":
                    timeConstrait = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    break;
            }
            PointLogDTO logDTO = new PointLogDTO
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                RecordCount = db.Points.Where(x => x.MemberID == userID && x.GetPointsDateTime >= timeConstrait).Count()
            };
           
            logDTO.PointLogs = new List<PointDTO>();
            int startIndex = (pageIndex - 1) * logDTO.PageSize;
            switch (sortName)
            {
                case "AddDate":
                case "":
                    if (sortDirection == "ASC")
                    {
                        List<Point> list = db.Points
                            .OrderBy(x => x.GetPointsDateTime)
                            .ThenByDescending(x => x.GetPoints)
                            .Where(x => x.MemberID == userID && x.GetPointsDateTime >= timeConstrait)
                            .Skip(startIndex)
                            .Take(logDTO.PageSize).ToList();
                        foreach (var item in list)
                        {
                            PointDTO dto = new PointDTO();
                            dto.MemberID = item.MemberID;
                            dto.MemberName = item.Member.Name;
                            dto.AddTime = item.GetPointsDateTime;
                            dto.Points = item.GetPoints;
                            dto.StatusID = item.StatusID;
                            dto.StatusName = item.Status.Name;
                            logDTO.PointLogs.Add(dto);
                        }
                    }
                    else
                    {
                        List<Point> list = db.Points
                           .OrderByDescending(x => x.GetPointsDateTime).ThenByDescending(x => x.GetPoints).Where(x => x.MemberID == userID && x.GetPointsDateTime >= timeConstrait)
                           .Skip(startIndex)
                           .Take(logDTO.PageSize).ToList();
                        foreach (var item in list)
                        {
                            PointDTO dto = new PointDTO();
                            dto.MemberID = item.MemberID;
                            dto.MemberName = item.Member.Name;
                            dto.AddTime = item.GetPointsDateTime;
                            dto.Points = item.GetPoints;
                            dto.StatusID = item.StatusID;
                            dto.StatusName = item.Status.Name;
                            logDTO.PointLogs.Add(dto);
                        }
                    }
                    break;
                case "Description":
                    if (sortDirection == "ASC")
                    {
                        List<Point> list = db.Points
                            .OrderBy(x => x.Status.Name).ThenByDescending(x => x.GetPoints).Where(x => x.MemberID == userID && x.GetPointsDateTime >= timeConstrait)
                            .Skip(startIndex)
                            .Take(logDTO.PageSize).ToList();
                        foreach (var item in list)
                        {
                            PointDTO dto = new PointDTO();
                            dto.MemberID = item.MemberID;
                            dto.MemberName = item.Member.Name;
                            dto.AddTime = item.GetPointsDateTime;
                            dto.Points = item.GetPoints;
                            dto.StatusID = item.StatusID;
                            dto.StatusName = item.Status.Name;
                            logDTO.PointLogs.Add(dto);
                        }
                    }
                    else
                    {
                        List<Point> list = db.Points
                           .OrderByDescending(x => x.Status.Name).ThenByDescending(x => x.GetPoints).Where(x => x.MemberID == userID && x.GetPointsDateTime >= timeConstrait)
                           .Skip(startIndex)
                           .Take(logDTO.PageSize).ToList();
                        foreach (var item in list)
                        {
                            PointDTO dto = new PointDTO();
                            dto.MemberID = item.MemberID;
                            dto.MemberName = item.Member.Name;
                            dto.AddTime = item.GetPointsDateTime;
                            dto.Points = item.GetPoints;
                            dto.StatusID = item.StatusID;
                            dto.StatusName = item.Status.Name;
                            logDTO.PointLogs.Add(dto);
                        }
                    }
                    break;
                case "Points":
                    if (sortDirection == "ASC")
                    {
                        List<Point> list = db.Points
                            .OrderBy(x => x.GetPoints).Where(x => x.MemberID == userID && x.GetPointsDateTime >= timeConstrait)
                            .Skip(startIndex)
                            .Take(logDTO.PageSize).ToList();
                        foreach (var item in list)
                        {
                            PointDTO dto = new PointDTO();
                            dto.MemberID = item.MemberID;
                            dto.MemberName = item.Member.Name;
                            dto.AddTime = item.GetPointsDateTime;
                            dto.Points = item.GetPoints;
                            dto.StatusID = item.StatusID;
                            dto.StatusName = item.Status.Name;
                            logDTO.PointLogs.Add(dto);
                        }
                    }
                    else
                    {
                        List<Point> list = db.Points
                           .OrderByDescending(x => x.GetPoints).Where(x => x.MemberID == userID && x.GetPointsDateTime >= timeConstrait)
                           .Skip(startIndex)
                           .Take(logDTO.PageSize).ToList();
                        foreach (var item in list)
                        {
                            PointDTO dto = new PointDTO();
                            dto.MemberID = item.MemberID;
                            dto.MemberName = item.Member.Name;
                            dto.AddTime = item.GetPointsDateTime;
                            dto.Points = item.GetPoints;
                            dto.StatusID = item.StatusID;
                            dto.StatusName = item.Status.Name;
                            logDTO.PointLogs.Add(dto);
                        }
                    }
                    break;
            }           
            return Json(logDTO);
        }
    }
}