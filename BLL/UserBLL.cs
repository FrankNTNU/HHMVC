﻿using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UserBLL
    {
        UserDAO userDAO = new UserDAO();
        public UserDTO GetUserWithUsernameAndPassword(UserDTO model)
        {
            UserDTO dto = new UserDTO();
            dto = userDAO.GetUserWithUsernameAndPassword(model);
            return dto;
        }
        public List<UserDTO> GetUsers()
        {
            return userDAO.GetUsers();
        }

        public void AddUser(UserDTO model)
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
            user.TaiwanID = model.TaiwanID;
            user.Gender = model.Gender;
            user.Height = model.Height;
            user.Phone = model.Phone;
            user.ActivityLevelID = model.ActivityLevelID;
            user.StatusID = model.StatusID;
            int ID = userDAO.AddUser(user);
        }
       

        public string DeleteUser(int ID)
        {
            string imagePath = userDAO.DeleteUser(ID);
            return imagePath;
        }

        public UserDTO GetUserWithID(int ID)
        {
            return userDAO.GetUserWithID(ID);
        }

        public string UpdateUser(UserDTO model)
        {
            string oldImagePath = UserDAO.UpdateUser(model);
            return oldImagePath;
        }
    }
}
