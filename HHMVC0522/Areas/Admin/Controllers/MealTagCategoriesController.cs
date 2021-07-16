using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DTO;

namespace UI.Areas.Admin.Controllers
{
    public class MealTagCategoriesController : Controller
    {
        // GET: Admin/MealTagCategories
        public ActionResult MealTagCategroiesList()
        {
            TagCategoryDetailBLL bll = new TagCategoryDetailBLL();
            List<TagCategoryDetailDTO> dtos = new List<TagCategoryDetailDTO>(); 
            dtos = bll.GetTags();
            
            return View(dtos);
        }
        public ActionResult AddMealTagCategory()
        {
            TagCategoryDetailDTO dto = new TagCategoryDetailDTO();
            return View(dto);
        }
        [HttpPost]
        public ActionResult AddMealTagCategory(TagCategoryDetailDTO tagCategoryDetail)
        {
            TagCategoryDetailBLL bll = new TagCategoryDetailBLL();
            if (tagCategoryDetail.UpLoadImage != null)
            {
                Bitmap image = new Bitmap(tagCategoryDetail.UpLoadImage.InputStream);
                Bitmap resizedImage = new Bitmap(image, 200, 200);
                string uniqueNumber = Guid.NewGuid().ToString();
                string fileName = uniqueNumber + tagCategoryDetail.UpLoadImage.FileName;
                resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/TagCategoriesImages/" + fileName));
                tagCategoryDetail.Image = fileName;
            }
            else
            {
                tagCategoryDetail.Image = "food.jpg";
            }
            bll.AddTags(tagCategoryDetail);
            return RedirectToAction("MealTagCategroiesList");
        }
        public ActionResult Delete(int ID)
        {
            TagCategoryDetailBLL bll = new TagCategoryDetailBLL();
            string deleteImagePass = bll.Delete(ID);

            string imageFullPath = Server.MapPath("~/Areas/Admin/Content/TagCategoriesImages/" + deleteImagePass);
            if (deleteImagePass != "food.jpg")
            {
                if (System.IO.File.Exists(imageFullPath))
                {
                    System.IO.File.Delete(imageFullPath);
                }
            }
            return RedirectToAction("MealTagCategroiesList");
        }
        public ActionResult UpadteTags(int ID)
        {
            TagCategoryDetailBLL bll = new TagCategoryDetailBLL();
            TagCategoryDetailDTO dto = new TagCategoryDetailDTO();
            dto = bll.GetTags(ID);
            return View(dto);
        }
        [HttpPost]
        public ActionResult UpadteTags(TagCategoryDetailDTO dto)
        {
            TagCategoryDetailBLL bll = new TagCategoryDetailBLL();
            if (dto.UpLoadImage != null)
            {
                Bitmap image = new Bitmap(dto.UpLoadImage.InputStream);
                Bitmap resizedImage = new Bitmap(image, 200, 200);
                string uniqueNumber = Guid.NewGuid().ToString();
                string fileName = uniqueNumber + dto.UpLoadImage.FileName;
                resizedImage.Save(Server.MapPath("~/Areas/Admin/Content/TagCategoriesImages/" + fileName));
                dto.Image = fileName;
            }
            string oldImagePass = bll.Update(dto);
            string imageFullPath = Server.MapPath("~/Areas/Admin/Content/TagCategoriesImages/" + oldImagePass);
            if (dto.UpLoadImage != null)
            {
                if (System.IO.File.Exists(imageFullPath))
                {
                    System.IO.File.Delete(imageFullPath);
                }
            }
            return RedirectToAction("MealTagCategroiesList");
        }
        public ActionResult GetMealsByTag(int ID)
        {
            TagCategoryDetailBLL bll = new TagCategoryDetailBLL();
            List<MealDetailDTO> mealDto = new List<MealDetailDTO>();
            mealDto = bll.GetMealsByTag(ID);
            
            return View(mealDto);
        }

       
        public ActionResult DeleteMeal(int id, int tagID)
        {
            TagCategoryDetailBLL bll = new TagCategoryDetailBLL();
            bll.DeleteMealInTag(id, tagID);

            return RedirectToAction("GetMealsByTag",new {ID=tagID });
        }
       
        //public ActionResult DeleteMeal(int id)
        //{
        //    int TagID = 0;
           
        //    TagCategoryDetailBLL bll = new TagCategoryDetailBLL();
        //    bll.DeleteMealInTag(id, TagID);
        //    return View();
        //}

        public ActionResult GetMealsByTagTest(int ID)
        {

            TagCategoryDetailBLL bll = new TagCategoryDetailBLL();
            List<MealDetailDTO> mealDto = new List<MealDetailDTO>();
            mealDto = bll.GetMealsByTag(ID);

            return View(mealDto);
        }
        public bool checkTagName(string userInput)
        {
            TagCategoryDetailBLL bll = new TagCategoryDetailBLL();
            bool check = bll.checkTagName(userInput);
            return check;
        }
    }
}