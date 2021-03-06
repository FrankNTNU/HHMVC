using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class General
    {
        public static class Messages
        {
            public static int AddSuccess = 1;
            public static int EmptyArea = 2;
            public static int UpdateSuccess = 3;
            public static int ImageMissing = 4;
            public static int ExtensionError = 5;
            public static int GeneralError = 6;
            public static int LoginError = 7;
            public static int WrongImageSize = 8;
            public static int NotAdmin = 9;
            public static int SameName = 10;
            public static int GoogleLoginError = 11;
            public static int LoginFirstError = 12;
        }
        public static class Gender
        {
            public static bool Male = true;
            public static bool Female = false;
        }
        public static class Category
        {
            public static int UserPost = 1;
            public static int News = 2;
            public static int Notice = 3;
            public static int Rule = 4;
        }
    }
}
