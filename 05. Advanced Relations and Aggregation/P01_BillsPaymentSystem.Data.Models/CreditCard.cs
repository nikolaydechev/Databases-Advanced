namespace P01_BillsPaymentSystem.Data.Models
{
    using System;

    public class CreditCard
    {
        public CreditCard()
        {
            this.MoneyOwed = 0;
            this.Limit = 0;
        }

        public CreditCard(decimal limit, decimal moneyOwed, DateTime expirationDate)
        {
            this.Limit = limit;
            this.MoneyOwed = moneyOwed;
            this.ExpirationDate = expirationDate;
        }

        public int CreditCardId { get; set; }

        private decimal Limit { get; set; }

        private decimal MoneyOwed { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public decimal GetLimit => this.Limit;

        public decimal GetMoneyOwed => this.MoneyOwed;

        public decimal LimitLeft => this.Limit - this.MoneyOwed;

        public decimal Withdraw(decimal amount)
        {
            if (this.LimitLeft - amount >= 0)
            {
                this.MoneyOwed += amount;
                return 0m;
            }

            var limitLeft = this.LimitLeft;
            this.MoneyOwed = this.Limit;
            return amount - limitLeft;
        }

        public void Deposit(decimal limit)
        {
            this.Limit += limit;
        }
    }
}
