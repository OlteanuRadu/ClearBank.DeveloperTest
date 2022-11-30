using System.Security.Principal;

namespace ClearBank.DeveloperTest.Types
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public AccountStatus Status { get; set; }
        public AllowedPaymentSchemes AllowedPaymentSchemes { get; set; }

        public virtual Account WithdrawAmount(decimal amount)
        {
            Balance -= amount;
            return this;
        }
    }
}
