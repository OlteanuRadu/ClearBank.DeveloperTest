using System;
using ClearBank.DeveloperTest.Configs;
using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using ClearBank.DeveloperTest.Validation;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStoreProvider _accountDataStoreProvider;
        private readonly IAccountValidator _accountValidator;
        private readonly IOptions<PaymentServiceOptions> _paymentServiceOptions;
        private readonly ILogger<IPaymentService> _logger;

        public PaymentService(
            IAccountDataStoreProvider accountDataStoreProvider,
            IAccountValidator accountValidator,
            IOptions<PaymentServiceOptions> paymentServiceOptions,
            ILogger<IPaymentService> logger
            )
        {
            _accountDataStoreProvider = accountDataStoreProvider ?? throw new ArgumentNullException(nameof(accountDataStoreProvider));
            _accountValidator = accountValidator ?? throw new ArgumentNullException(nameof(accountValidator));
            _paymentServiceOptions = paymentServiceOptions ?? throw new ArgumentNullException(nameof(paymentServiceOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var accountDataStore = _accountDataStoreProvider
                .GetAccountDataStore(_paymentServiceOptions.Value.DataStoreType);

            var account = accountDataStore
                .GetAccount(request.DebtorAccountNumber);

            var accountValidationResult = _accountValidator.CanProcessPayment(request, account);

            _logger.LogTrace("Account Validation Result", new Dictionary<string, string>
            {
                {"Validation Result", $"{accountValidationResult.IsSuccess}"},
                {"Validation Error", accountValidationResult.ValidationError }
            });

            if (accountValidationResult.IsSuccess)
            {
                accountDataStore.UpdateAccount(account.WithdrawAmount(request.Amount));

                return new MakePaymentResult
                {
                    Success = true
                };
            }

            return new MakePaymentResult
            {
                Message = accountValidationResult.ValidationError
            };
        }
    }
}
