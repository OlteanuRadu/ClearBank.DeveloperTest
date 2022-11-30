using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation
{
    public interface IAccountValidator
    {
        ValidationAccountResult CanProcessPayment(MakePaymentRequest request, Account account);
    }
}
