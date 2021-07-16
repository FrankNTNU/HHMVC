using DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MemberDAO : HHContext
    {
        public MemberDTO GetMemberWithEmailAndPassword(MemberDTO model)
        {
            MemberDTO dto = new MemberDTO();
            Member user = db.Members.FirstOrDefault(x => x.Email == model.Email &&
            x.Password == model.Password);
            if (user != null && user.ID != 0)
            {
                dto.ID = user.ID;
                dto.UserName = user.UserName;
                dto.Email = user.Email;
                dto.Name = user.Name;
                dto.ImagePath = user.Image;
                dto.IsAdmin = user.IsAdmin;
                dto.StatusID = user.StatusID;
                dto.Points = user.Points;
            }
            return dto;
        }

        public MemberDTO GetMemberWithID(int ID)
        {
            MemberDTO dto = new MemberDTO();
            Member user = db.Members.FirstOrDefault(x => x.ID == ID);
            if (user != null)
            {
                dto.ID = user.ID;
                dto.TaiwanID = user.TaiwanID;
                dto.UserName = user.UserName;
                dto.Height = user.Height;
                dto.Password = user.Password;
                dto.Email = user.Email;
                dto.Phone = user.Phone;
                dto.Gender = user.Gender;
                dto.IsAdmin = user.IsAdmin;
                dto.BirthDate = user.Birthdate;
                dto.JoinDate = user.JoinDate;
                dto.ImagePath = user.Image;
                dto.ActivityLevelID = user.ActivityLevelID;
                dto.StatusID = user.StatusID;
                dto.Name = user.Name;
            }
            return dto;
        }

        public static string EditMember(MemberDTO model)
        {
            try
            {
                Member user = db.Members.First(x => x.ID == model.ID);
                string oldImagePath = user.Image;
                user.Name = model.Name;
                user.UserName = model.UserName;
                if (model.ImagePath != null)
                {
                    user.Image = model.ImagePath;
                }
                user.Email = model.Email;
                user.Password = model.Password;
                user.Height = model.Height;
                user.ActivityLevelID = model.ActivityLevelID;
                user.Birthdate = model.BirthDate;
                user.Phone = model.Phone;
                user.Gender = model.Gender;
                user.MealCount = model.MealCount;

                db.SaveChanges();
                return oldImagePath;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void AddGoogleID(string email, string googleID)
        {
            Member member = db.Members.FirstOrDefault(x => x.Email == email);
            member.GoogleID = googleID;
            db.SaveChanges();
        }
        public void GetNewPassword(string email, string password)
        {
            Member member = db.Members.FirstOrDefault(x => x.Email == email);
            member.Password = password;
            db.SaveChanges();
        }

        public bool IsEmailExist(string email)
        {
            Member member = db.Members.FirstOrDefault(x => x.Email == email);
            if (member != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string DeleteMember(int ID)
        {
            try
            {
                Member user = db.Members.First(x => x.ID == ID);
                db.Members.Remove(user);
                db.SaveChanges();
                return user.Image;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int AddMember(Member user)
        {
            try
            {
                db.Members.Add(user);
                db.SaveChanges();
                return user.ID;
            }
            catch (DbEntityValidationException e)
            {

                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void DeductPoints(int userID, int points)
        {
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Member member = db.Members.FirstOrDefault(x => x.ID == userID);
                member.Points -= points;
                db.SaveChanges();
            }
        }
        public int GetPoints(int userID)
        {
            int points = 0;
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Member member = db.Members.FirstOrDefault(x => x.ID == userID);
                points = member.Points;
            }
            return points;
        }
        public string GetActiveCode(int userID)
        {
            string activeCode = "";
            using (HealthHelperEntities db = new HealthHelperEntities())
            {
                Member member = db.Members.FirstOrDefault(x => x.ID == userID);
                activeCode = member.ActiveCode;
            }
            return activeCode;
        }
        public Member GetMemberByMemberID(int memberID)
        {
            return db.Members.FirstOrDefault(m => m.ID == memberID);
        }

        public void UpdateActivityLevel(int memberID, int activityLevelId)
        {
            int x = db.Members.FirstOrDefault(m => m.ID == memberID).ActivityLevelID;
            db.Members.FirstOrDefault(m => m.ID == memberID).ActivityLevelID = activityLevelId;
            db.SaveChanges();
        }
    }
}

