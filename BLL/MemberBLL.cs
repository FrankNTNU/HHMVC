using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MemberBLL
    {
        MemberDAO memberDAO = new MemberDAO();
        public MemberDTO GetMemberWithEmailAndPassword(MemberDTO model)
        {
            MemberDTO dto = new MemberDTO();
            dto = memberDAO.GetMemberWithEmailAndPassword(model);
            return dto;
        }


        public int RegisterUser(MemberDTO model)
        {
            Member user = new Member();
            user.UserName = model.UserName;
            user.Password = model.Password;
            user.Email = model.Email;
            user.Image = model.ImagePath;
            user.Name = model.Name;
            user.IsAdmin = model.IsAdmin;
            user.JoinDate = DateTime.Now;
            user.Birthdate = model.BirthDate;
            user.Gender = model.Gender;
            user.Height = model.Height;
            user.Phone = model.Phone;
            user.ActivityLevelID = model.ActivityLevelID;
            user.StatusID = 2;
            user.MealCount = model.MealCount;
            user.ActiveCode= Guid.NewGuid().ToString().Substring(0, 8);
            //user.WeightLogs = model.Weight;
            int newID=memberDAO.AddMember(user);
            WeightLog weightLog = new WeightLog();
            weightLog.Weight = model.Weight;
            weightLog.MemberID =newID;
            weightLog.UpdatedDate = DateTime.Now;
            WeightLogDAO weightLogDAO = new WeightLogDAO();
            weightLogDAO.AddWeightLog(weightLog);
            return newID;
        }


        public string DeleteUser(int ID)
        {
            string imagePath = memberDAO.DeleteMember(ID);
            return imagePath;
        }

        public MemberDTO GetMemberWithID(int ID)
        {
            return memberDAO.GetMemberWithID(ID);
        }

        public string EditMember(MemberDTO model)
        {
            string oldImagePath = MemberDAO.EditMember(model);
            return oldImagePath;
        }
        public int GetPoints(int userID)
        {
            UserDAO userDAO = new UserDAO();
            return userDAO.GetPoints(userID);
        }
        public string GetActiveCode(int userID)
        {
            return memberDAO.GetActiveCode(userID);
        }
        public bool IsEmailExist(string email)
        {
            return memberDAO.IsEmailExist(email);
        }

        public void AddGoogleID(string email, string googleID)
        {
            memberDAO.AddGoogleID(email, googleID);
        }

        public Member GetMemberByMemberID(int memberID)
        {
           return memberDAO.GetMemberByMemberID(memberID); 
        }
        public void GetNewPassword(string email, string password)
        {
            memberDAO.GetNewPassword(email, password);
            
        }

        public void UpdateActivityLevel(int memberID, int activityLevelId)
        {
            memberDAO.UpdateActivityLevel(memberID, activityLevelId);
        }
    }
}

