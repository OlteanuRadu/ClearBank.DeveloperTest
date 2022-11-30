using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validation;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class AccountValidatorTests
    {
        [Fact]
        public void CanProcessPaymentTest_When_PaymentScheme_Is_Bacs_And_Account_Is_Null_Then_Return_ValidationResult()
        {
            //Arrange
            var accountValidator = new AccountValidator();

            //Act
            var result = accountValidator
                            .CanProcessPayment(
                                new MakePaymentRequest
                                {
                                    PaymentScheme = PaymentScheme.Bacs
                                }, null);

            //Assert
            Assert.True(!result.IsSuccess);
        }

        [Theory]
        [InlineData(PaymentScheme.Bacs, AllowedPaymentSchemes.FasterPayments, false)]
        [InlineData(PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs, true)]
        public void CanProcessPaymentTest_When_PaymentScheme_Is_Bacs_And_Account_AllowedScheme_Is_Provided_Then_Return_ValidationResult(
            PaymentScheme paymentScheme,
            AllowedPaymentSchemes allowedPaymentSchemes,
            bool validationResult)
        {
            //Arrange
            var accountValidator = new AccountValidator();

            //Act
            var result = accountValidator
                            .CanProcessPayment(
                                new MakePaymentRequest
                                { 
                                    PaymentScheme = paymentScheme
                                },
                                new Account
                                { 
                                    AllowedPaymentSchemes = allowedPaymentSchemes
                                });

            //Assert
            Assert.True(result.IsSuccess == validationResult);
        }

        [Theory]
        [InlineData(PaymentScheme.FasterPayments, 100.1, AllowedPaymentSchemes.Bacs, 101, false)]
        [InlineData(PaymentScheme.FasterPayments, 100.1, AllowedPaymentSchemes.FasterPayments, 100, false)]
        [InlineData(PaymentScheme.FasterPayments, 100.1, AllowedPaymentSchemes.Bacs, 100, false)]
        [InlineData(PaymentScheme.FasterPayments, 100.1, AllowedPaymentSchemes.FasterPayments, 101, true)]
        public void CanProcessPaymentTest_When_PaymentScheme_Is_FasterPayments_And_Account_AllowedScheme_And_Amount_Is_Provided_Then_Return_ValidationResult(
            PaymentScheme paymentScheme,
            decimal requestAmount,
            AllowedPaymentSchemes allowedPaymentSchemes,
            decimal accountAmount,
            bool validationResult)
        {
            //Arrange
            var accountValidator = new AccountValidator();

            //Act
            var result = accountValidator
                            .CanProcessPayment(
                                new MakePaymentRequest
                                {
                                    PaymentScheme = paymentScheme,
                                    Amount = requestAmount
                                },
                                new Account
                                {
                                    AllowedPaymentSchemes = allowedPaymentSchemes,
                                    Balance = accountAmount
                                });

            //Assert
            Assert.True(result.IsSuccess == validationResult);
        }

        [Theory]
        [InlineData(PaymentScheme.Chaps, AllowedPaymentSchemes.Bacs, AccountStatus.Live, false)]
        [InlineData(PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps, AccountStatus.InboundPaymentsOnly, false)]
        [InlineData(PaymentScheme.Chaps, AllowedPaymentSchemes.Bacs, AccountStatus.InboundPaymentsOnly, false)]
        [InlineData(PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps, AccountStatus.Live, true)]

        public void CanProcessPaymentTest_When_PaymentScheme_Is_Chaps_And_Account_AllowedScheme_And_Status_Then_Return_ValidationResult(
            PaymentScheme paymentScheme,
            AllowedPaymentSchemes allowedPaymentSchemes,
            AccountStatus accountStatus,
            bool validationResult)
        {
            //Arrange
            var accountValidator = new AccountValidator();

            //Act
            var result = accountValidator
                            .CanProcessPayment(
                                new MakePaymentRequest
                                {
                                    PaymentScheme = paymentScheme,
                                },
                                new Account
                                {
                                    AllowedPaymentSchemes = allowedPaymentSchemes,
                                    Status = accountStatus
                                });

            //Assert
            Assert.True(result.IsSuccess == validationResult);
        }
    }
}
