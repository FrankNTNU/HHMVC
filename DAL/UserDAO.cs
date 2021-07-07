using DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserDAO : HHContext
    {
        public UserDTO GetUserWithUsernameAndPassword(UserDTO model)
        {
            UserDTO dto = new UserDTO();
            Member user = db.Members.FirstOrDefault(x => x.UserName == model.UserName &&
            x.Password == model.Password);
            if (user != null && user.ID != 0)
            {
                dto.ID = user.ID;
                dto.UserName = user.UserName;
                dto.Name = user.Name;
                dto.ImagePath = user.Image;
                dto.IsAdmin = user.IsAdmin;
                dto.StatusID = user.StatusID;
                dto.Points = user.Points;
            }
            return dto;
        }
        public UserDTO GetUserWithEmailAndPassword(UserDTO model)
        {
            UserDTO dto = new UserDTO();
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
        public UserDTO GetUserWithEmailAndPwdAndCode(UserDTO model)
        {
            UserDTO dto = new UserDTO();
            Member user = db.Members.FirstOrDefault(x => x.Email == model.Email &&
            x.Password == model.Password && x.ActiveCode==model.ActiveCode);
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
                dto.ActiveCode = user.ActiveCode;
            }
            return dto;
        }

        public UserDTO GetUserWithGoogleID(string googleID)
        {
            UserDTO dto = new UserDTO();
            Member user = db.Members.FirstOrDefault(x => x.GoogleID == googleID);
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
                dto.GoogleID = user.GoogleID;
            }
            return dto;
        }

        public UserDTO GetUserWithID(int ID)
        {
            UserDTO dto = new UserDTO();
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

        public void ActivateUser(int userID)
        {
            Member member = db.Members.First(x => x.ID == userID);
            member.StatusID = 1;
            db.SaveChanges();
        }

        public static string UpdateUser(UserDTO model)
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
                user.IsAdmin = model.IsAdmin;
                user.Height = model.Height;
                user.ActivityLevelID = model.ActivityLevelID;
                user.Birthdate = model.BirthDate;
                user.Phone = model.Phone;
                user.StatusID = model.StatusID;
                user.Gender = model.Gender;
                user.TaiwanID = model.TaiwanID;
                db.SaveChanges();
                return oldImagePath;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string DeleteUser(int ID)
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

        public List<UserDTO> GetUsers()
        {
            List<Member> list = db.Members.ToList();
            List<UserDTO> userList = new List<UserDTO>();
            foreach (var item in list)
            {
                UserDTO dto = new UserDTO();
                dto.ID = item.ID;
                dto.Name = item.Name;
                dto.UserName = item.UserName;
                dto.ImagePath = item.Image;
                userList.Add(dto);
            }
            return userList;
        }

        public int AddUser(Member user)
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
            using(HealthHelperEntities db = new HealthHelperEntities())
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
                Member member= db.Members.FirstOrDefault(x => x.ID == userID);
                points = member.Points;
            }
            return points;
        }
       
    }
}
