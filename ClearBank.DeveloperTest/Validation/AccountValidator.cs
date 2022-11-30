using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation
{
    public class AccountValidator : IAccountValidator
    {
        public ValidationAccountResult CanProcessPayment(MakePaymentRequest request, Account account)
        {
            if (account == null)
            {
                return new ValidationAccountResult { ValidationError = "Account is null" };
            }

            return request.PaymentScheme switch
            {
                PaymentScheme.Bacs =>  account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs) ?
                                       new ValidationAccountResult { IsSuccess = true } :
                                       new ValidationAccountResult { ValidationError = $"Failed account validation for Payment Scheme:Bacs business rules" },

                PaymentScheme.FasterPayments => account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments)
                                                && account.Balance > request.Amount ?
                                                new ValidationAccountResult { IsSuccess = true } :
                                                new ValidationAccountResult { ValidationError = $"Failed account validation for Payment Scheme:FasterPayments business rules" },

                PaymentScheme.Chaps => account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps)
                                                && account.Status == AccountStatus.Live ?
                                                new ValidationAccountResult { IsSuccess = true } :
                                                new ValidationAccountResult { ValidationError = $"Failed account validation for Payment Scheme:Chaps business rules" },
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}
