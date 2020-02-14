namespace TeamBuilder.App.Core
{
    using System;
    using System.Reflection.Metadata;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Models;

    public class AuthenticationManager
    {
        private static User currentUser;

        public static void Login(User user)
        {
            currentUser = user; 
        }

        public static void Logout()
        {
            if (!IsAuthenticated())
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }

            currentUser = null;
        }

        public static void Authorize()
        {
            if (currentUser == null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }
        }

        public static bool IsAuthenticated()
        {
            if (currentUser == null)
            {
                return false;
            }

            return true;
        }

        public static User GetCurrentUser()
        {
            return currentUser;
        }
    }
}
