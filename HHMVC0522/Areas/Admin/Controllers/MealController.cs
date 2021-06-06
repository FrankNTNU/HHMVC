using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DTO;
using PagedList;

namespace UI.Areas.Admin.Controllers
{
    public class MealController : Controller
    {
        // GET: Admin/Meal
        public ActionResult List()
        {

            MealBLL bll = new MealBLL();
            List<MealDetailDTO> dtos = new List<MealDetailDTO>();
            dtos = bll.GetMeals();
            
            return View(dtos);
        }
        public ActionResult CreateMeal()
        {
            MealDetailDTO mealDetailDTO = new MealDetailDTO();
            return View(mealDetailDTO);
        }
        [HttpPost]
        public ActionResult CreateMeal(MealDetailDTO mealDetailDTO)
        {
            if (mealDetailDTO.MealOptionUpLoadImage != null)
            {
                Bitmap image = new Bitmap(mealDetailDTO.MealOptionUpLoadImage.InputStream);
                Bitmap resizedImage = new Bitmap(image, 250, 250);
                string uniqueNumber = Guid.NewGuid().ToString();
                string fileName = uniqueNumber + mealDetailDTO.MealOptionUpLoadImage.FileName;
                resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/MealOptionImages/" + fileName));
                mealDetailDTO.MealOptionImage = fileName;
            }
            else
            {
                mealDetailDTO.MealOptionImage = "mealDefault.jpg";
            }
            MealBLL bll = new MealBLL();
            bll.Add(mealDetailDTO);
            return RedirectToAction("List");

        }
        public ActionResult Delete(int id)
        {
            MealBLL bll = new MealBLL();
            bll.DeleteMeal(id);
            return RedirectToAction("List");
        }

        public ActionResult Update(int id)
        {
            MealBLL bll = new MealBLL();
            MealDetailDTO dto = new MealDetailDTO();
            dto = bll.GetMeals(id);
            return View(dto);
        }
        [HttpPost]
        public ActionResult Update(MealDetailDTO dto)
        {
            if (dto.MealOptionUpLoadImage != null)
            {
                Bitmap image = new Bitmap(dto.MealOptionUpLoadImage.InputStream);
                Bitmap resizedImage = new Bitmap(image, 200, 500);
                string uniqueNumber = Guid.NewGuid().ToString();
                string fileName = uniqueNumber + dto.MealOptionUpLoadImage.FileName;
                resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/MealOptionImages/" + fileName));
                dto.MealOptionImage = fileName;
            }
            MealBLL bll = new MealBLL();
            string oldImagePass = bll.UpdateMeal(dto);
            string imageFullPath = Server.MapPath("~/Areas/Admin/Content/MealOptionImages/" + oldImagePass);
            if (System.IO.File.Exists(imageFullPath))
            {
                System.IO.File.Delete(imageFullPath);
            }
            return RedirectToAction("List");
        }
    }
}