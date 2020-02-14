namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.Linq;
    using TeamBuilder.App.Core.Commands.Contracts;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Data;
    using TeamBuilder.Models;
    using TeamBuilder.Models.Enums;

    public class RegisterUserCommand : ICommand
    {
        //•	RegisterUser <username> <password> <repeat-password> <firstName> <lastName> <age> <gender>

        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(7, inputArgs);

            string userName = inputArgs[0];
            string password = inputArgs[1];
            string repeatedPassword = inputArgs[2];
            string firstName = inputArgs[3];
            string lastName = inputArgs[4];

            if (AuthenticationManager.IsAuthenticated())    
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }

            //Validate given username.
            if (userName.Length < Constants.MinUsernameLength || userName.Length > Constants.MaxUsernameLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UsernameNotValid, userName));
            }

            //Validate password.
            if (!password.Any(char.IsDigit) || !password.Any(char.IsUpper) || password.Length < Constants.MinPasswordLength || password.Length > Constants.MaxPasswordLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.PasswordNotValid, password));
            }

            //Validate repeatedPassword.
            if (password != repeatedPassword)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.PasswordDoesNotMatch);
            }

            int age;
            bool isNumber = int.TryParse(inputArgs[5], out age);

            //Validate Age
            if (!isNumber || age <= 0)
            {
                throw new ArgumentException(Constants.ErrorMessages.AgeNotValid);
            }

            //Validate Gender
            Gender gender;
            bool isGenderValid = Enum.TryParse(inputArgs[6], out gender);

            if (!isGenderValid)
            {
                throw new ArgumentException(Constants.ErrorMessages.GenderNotValid);
            }

            //Is username taken validation
            if (CommandHelper.IsUserExisting(userName))
            {
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.UsernameIsTaken, userName));
            }

            this.RegisterUser(userName, password, firstName, lastName, age, gender);

            return $"User {userName} was registered successfully!";
        }

        private void RegisterUser(string userName, string password, string firstName, string lastName, int age, Gender gender)
        {
            using (var context = new TeamBuilderContext())
            {
                var user = new User
                {
                    Username = userName,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age,
                    Gender = gender
                };

                context.Users.Add(user);

                context.SaveChanges();
            }
        }
    }
}
