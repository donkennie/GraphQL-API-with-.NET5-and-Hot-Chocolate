using GraphQLAuth.Data;
using GraphQLAuth.InputType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GraphQLAuth.Logic
{
    public class AuthLogic : IAuthLogic
    {


        private readonly AuthContext _authContext;

        public AuthLogic(AuthContext authContext)
        {
            _authContext = authContext;
        }


        private string ResigstrationValidations(RegisterInputType registerInput)
        {
            if (string.IsNullOrEmpty(registerInput.EmailAddress))
            {
                return "Email can't be empty";
            }

            if (string.IsNullOrEmpty(registerInput.Password)
                || string.IsNullOrEmpty(registerInput.ConfirmPassword))
            {
                return "Password Or ConfirmPassword Can't be empty";
            }

            if (registerInput.Password != registerInput.ConfirmPassword)
            {
                return "Invalid confirm password";
            }

            string emailRules = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            if (!Regex.IsMatch(registerInput.EmailAddress, emailRules))
            {
                return "Not a valid email";
            }

            // atleast one lower case letter
            // atleast one upper case letter
            // atleast one special character
            // atleast one number
            // atleast 8 character length
            string passwordRules = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";
            if (!Regex.IsMatch(registerInput.Password, passwordRules))
            {
                return "Not a valid password";
            }

            return string.Empty;
        }

        private string PasswordHash(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);


            return Convert.ToBase64String(hashBytes);
        }


        public string Register(RegisterInputType registerInput)
        {
            string errorMessage = ResigstrationValidations(registerInput);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return errorMessage;

            }

            User newUser = new User
            {

                EmailAddress = registerInput.EmailAddress,
                FirstName = registerInput.FirstName,
                LastName = registerInput.LastName,
                Password = PasswordHash(registerInput.Password)

            };

            _authContext.User.Add(newUser);
            _authContext.SaveChanges();

            UserRoles newRoles = new UserRoles
            {
                Name = "admin",
                UserId = newUser.UserId
            };

            _authContext.UserRoles.Add(newRoles);
            _authContext.SaveChanges();

            return "Registration Successful";

        }


    }
}
