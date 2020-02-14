namespace P01_BillsPaymentSystem.Data.Models
{
    public class BankAccount
    {
        public BankAccount()
        {
            this.Balance = 0;
        }

        public int BankAccountId { get; set; }

        private decimal Balance { get; set; }

        public string BankName { get; set; }

        public string SwiftCode { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public decimal GetBalance => this.Balance;

        public decimal Withdraw(decimal amount)
        {
            if (amount > this.Balance)
            {
                var balance = this.Balance;
                this.Balance = 0m;
                return amount - balance;
            }

            this.Balance -= amount;
            return 0m;
        }

        public void Deposit(decimal amount)
        {
            this.Balance += amount;
        }
    }
}
