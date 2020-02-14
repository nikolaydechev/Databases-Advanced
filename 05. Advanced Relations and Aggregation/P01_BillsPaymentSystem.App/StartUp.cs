namespace P01_BillsPaymentSystem.App
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using P01_BillsPaymentSystem.Data;
    using P01_BillsPaymentSystem.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            //using (var dbContext = new BillsPaymentSystemContext())
            //{
            //    Seed(dbContext);
            //}

            Console.Write("User Id: ");
            var userId = int.Parse(Console.ReadLine());
            Console.Write("Amount of money: ");
            var amountOfMoney = decimal.Parse(Console.ReadLine());

            using (var dbContext = new BillsPaymentSystemContext())
            {
                //Console.WriteLine(UserDetails(userId, dbContext));

                PayBills(userId, amountOfMoney, dbContext);

                //Console.WriteLine($"--------------------");
                //Console.WriteLine(UserDetails(userId, dbContext));
            }
        }

        private static string UserDetails(int userId, BillsPaymentSystemContext dbContext)
        {
            var user = dbContext.Users
                    .Where(e => e.UserId == userId)
                    .Select(e => new
                    {
                        UserName = e.FirstName + " " + e.LastName,
                        BankAccounts = e.PaymentMethods.Where(x => x.Type == PaymentMethodType.BankAccount).Select(p => p.BankAccount).ToList(),
                        CreditCards = e.PaymentMethods.Where(x => x.Type == PaymentMethodType.CreditCard).Select(c => c.CreditCard).ToList()
                    }).FirstOrDefault();

            if (user == null)
            {
                return $"User with id {userId} not found!";
            }

            var sb = new StringBuilder();
            sb.AppendLine($"User: {user.UserName}");

            if (user.BankAccounts.Any())
            {
                sb.AppendLine($"Bank Accounts:");
                foreach (var bankAccount in user.BankAccounts)
                {
                    sb.AppendLine($"-- ID: {bankAccount.BankAccountId}");
                    sb.AppendLine($"--- Balance: {bankAccount.GetBalance:F2}");
                    sb.AppendLine($"--- Bank: {bankAccount.BankName}");
                    sb.AppendLine($"--- SWIFT: {bankAccount.SwiftCode}");
                }
            }

            if (user.CreditCards.Any())
            {
                sb.AppendLine($"Credit Cards:");
                foreach (var creditCard in user.CreditCards)
                {
                    sb.AppendLine($"-- ID: {creditCard.CreditCardId}");
                    sb.AppendLine($"--- Limit: {creditCard.GetLimit:F2}");
                    sb.AppendLine($"--- MoneyOwed: {creditCard.GetMoneyOwed:F2}");
                    sb.AppendLine($"--- Limit Left: {creditCard.LimitLeft:F2}");
                    sb.AppendLine($"--- Expiration Date: {creditCard.ExpirationDate.ToString("yyyy/MM", CultureInfo.InvariantCulture)}");
                }
            }

            return sb.ToString().Trim();
        }

        private static void PayBills(int userId, decimal amount, BillsPaymentSystemContext dbContext)
        {
            var user = dbContext.Users
                .Where(x => x.UserId == userId)
                .Include(x => x.PaymentMethods)
                    .ThenInclude(x => x.BankAccount)
                .Include(x => x.PaymentMethods)
                    .ThenInclude(y => y.CreditCard)
                .FirstOrDefault();
            
            var userBankAccounts = user.PaymentMethods
                .Where(t => t.Type == PaymentMethodType.BankAccount)
                .Select(b => b.BankAccount).ToList();

            var userCreditCards = user.PaymentMethods
                .Where(t => t.Type == PaymentMethodType.CreditCard)
                .Select(c => c.CreditCard).ToList();
            

            decimal userBalance = userBankAccounts.Sum(x => x.GetBalance) + userCreditCards.Sum(x => x.LimitLeft);

            if (userBalance < amount)
            {
                Console.WriteLine($"Insufficient funds!");
                return;
            }

            decimal amountToWithdraw = amount;

            foreach (var userBankAccount in userBankAccounts.OrderBy(x => x.BankAccountId))
            {
                var restOfTheAmount = userBankAccount.Withdraw(amountToWithdraw);
                amountToWithdraw = restOfTheAmount;

                if (restOfTheAmount == 0m)
                {
                    dbContext.SaveChanges();
                    return;
                }
            }

            foreach (var userCreditCard in userCreditCards.OrderBy(x => x.CreditCardId))
            {
                var restOfTheAmount = userCreditCard.Withdraw(amountToWithdraw);
                amountToWithdraw = restOfTheAmount;

                if (restOfTheAmount == 0m)
                {
                    dbContext.SaveChanges();
                    return;
                }
            }
        }

        private static void Seed(BillsPaymentSystemContext db)
        {
            using (db)
            {
                var user = new User()
                {
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                    Email = "ivan@email.com",
                    Password = "ivan101"
                };

                var creditCards = new CreditCard[]
                {
                    new CreditCard(1500m, 10m, DateTime.ParseExact("20.05.2020", "dd.MM.yyyy", null)),
                    
                    new CreditCard(800m, 300m, DateTime.ParseExact("20.05.2020", "dd.MM.yyyy", null))
                };

                var bankAccount = new BankAccount()
                {
                    BankName = "Nordea Bank",
                    SwiftCode = "NORDEADKK"
                };
                bankAccount.Deposit(2000m);

                var paymentMethods = new PaymentMethod[]
                {
                    new PaymentMethod()
                    {
                        User = user,
                        CreditCard = creditCards[0],
                        Type = PaymentMethodType.CreditCard
                    },

                    new PaymentMethod()
                    {
                        User = user,
                        CreditCard = creditCards[1],
                        Type = PaymentMethodType.CreditCard
                    },

                    new PaymentMethod()
                    {
                        User = user,
                        BankAccount = bankAccount,
                        Type = PaymentMethodType.BankAccount
                    },
                };

                db.Users.Add(user);
                db.CreditCards.AddRange(creditCards);
                db.BankAccounts.Add(bankAccount);
                db.PaymentMethods.AddRange(paymentMethods);

                db.SaveChanges();
            }
        }
    }
}
