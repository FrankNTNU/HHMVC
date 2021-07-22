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
            dtos = bll.GetOnlyMeals();
            
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
        public ActionResult CreateMealWithTags()
        {
            TagCategoryDetailBLL tagBll = new TagCategoryDetailBLL();
            MealDetailDTO mealDetailDTO = new MealDetailDTO();
            //mealDetailDTO.Tags = tagBll.GetTagForAddMeal();
            mealDetailDTO.DTags = tagBll.getDTags();
            return View(mealDetailDTO);
        }
        [HttpPost]
        public ActionResult CreateMealWithTags(MealDetailDTO mealDetailDTO,List<string> tags)
        {
            if (ModelState.IsValid)
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
                int mealID = bll.Add(mealDetailDTO);
                TagCategoryDetailBLL tagCategoryDetailBLL = new TagCategoryDetailBLL();
                tagCategoryDetailBLL.AddMealTags(mealID, tags);
                return RedirectToAction("List");
            }
            else
            {
                TagCategoryDetailBLL tagBll = new TagCategoryDetailBLL();
                //mealDetailDTO.Tags = tagBll.GetTagForAddMeal();
                mealDetailDTO.DTags = tagBll.getDTags();
                return View(mealDetailDTO);
            }

        }

        public ActionResult Delete(int id)
        {
            MealBLL bll = new MealBLL();
            bll.DeleteMeal(id);
            return RedirectToAction("List");
        }

        public ActionResult HiddenMeal(int ID)
        {
            MealBLL bll = new MealBLL();
            bll.HiddenMeal(ID);
            return RedirectToAction("List");
        }
        public ActionResult DisplayMeal(int ID)
        {
            MealBLL bll = new MealBLL();
            bll.DisplayMeal(ID);
            return RedirectToAction("List");
        }

        public ActionResult Update(int id)
        {
            MealBLL mealbll = new MealBLL();
            TagCategoryDetailBLL tagCategoryDetailBLL = new TagCategoryDetailBLL();
            MealDetailDTO dto = new MealDetailDTO();
            dto = mealbll.GetMeals(id);
            return View(dto);
        }
        [HttpPost]
        public ActionResult Update(MealDetailDTO dto)
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
            MealBLL bll = new MealBLL();
            
            string oldImagePass = bll.UpdateMeal(dto);
            string imageFullPath = Server.MapPath("~/Areas/Admin/Content/MealOptionImages/" + oldImagePass);
            if (System.IO.File.Exists(imageFullPath))
            {
                System.IO.File.Delete(imageFullPath);
            }
            return RedirectToAction("List");
        }
        public ActionResult UpdateWithTags(int id)
        {
            MealBLL mealbll = new MealBLL();
            MealDetailDTO dto = new MealDetailDTO();
            TagCategoryDetailBLL tagBll = new TagCategoryDetailBLL();
            //mealDetailDTO.Tags = tagBll.GetTagForAddMeal();
            dto = mealbll.GetMealWithTags(id);
            dto.DTags = tagBll.getDTags();
            return View(dto);
        }
        [HttpPost]
        public ActionResult UpdateWithTags(MealDetailDTO dto, string[] tags)
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
            MealBLL bll = new MealBLL();
            TagCategoryDetailBLL tagBll = new TagCategoryDetailBLL();
            tagBll.UpdateMealTags(dto.ID, tags);
            string oldImagePass = bll.UpdateMeal(dto);
            string imageFullPath = Server.MapPath("~/Areas/Admin/Content/MealOptionImages/" + oldImagePass);
            if (dto.MealOptionUpLoadImage != null)
            {
                if (System.IO.File.Exists(imageFullPath))
                {
                    System.IO.File.Delete(imageFullPath);
                }
            }
            
            return RedirectToAction("List");
        }


        public ActionResult ListTest()
        {

            MealBLL bll = new MealBLL();
            List<MealDetailDTO> dtos = new List<MealDetailDTO>();
            dtos = bll.GetOnlyMeals();

            return View(dtos);
        }

        public ActionResult PageTest(int page=1)//預設第一頁
        {
            int currentPage = page < 1 ? 1 : page;

            MealBLL bll = new MealBLL();
            List<MealDetailDTO> dtos = new List<MealDetailDTO>();
            dtos = bll.GetOnlyMeals();

            var pagedResukt = dtos.ToPagedList(currentPage, 10);//回傳客戶端請求的頁數及筆數


            return View(pagedResukt);
        }
        public bool checkMaelName(string userInput)
        {
            MealBLL bll = new MealBLL();
            bool check = bll.checkMealName(userInput);
            return check;
        }
    }
}