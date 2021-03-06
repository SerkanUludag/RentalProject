using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constants.Messages
{
    public static class Messages
    {
        public static string Added = "Added";
        public static string Deleted = "Deleted";
        public static string Updated = "Updated";
        public static string Error = "Error";
        public static string Listed = "Listed";
        public static string Success = "Success";
        public static string MoreThanFiveImageError = "Cars cannot have more than 5 images";
        public static string UserRegistered = "User is registered";
        public static string UserNotFound = "User not found";
        public static string PasswordError = "Wrong password";
        public static string SuccessfulLogin = "Successfully logged in";
        public static string UserAlreadyExists = "User already exists";
        public static string AccessTokenCreated = "token created";
        public static string AuthorizationDenied = "Authorization denied";
    }
}
