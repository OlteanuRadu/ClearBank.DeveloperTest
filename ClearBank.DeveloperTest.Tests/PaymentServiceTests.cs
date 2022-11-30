using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ClearBank.DeveloperTest.Configs;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        private readonly Mock<IAccountDataStoreProvider> _accountDataStoreProvider;
        private readonly Mock<IAccountValidator> _accountValidator;
        private readonly Mock<IOptions<PaymentServiceOptions>> _paymentServiceOptions;
        private readonly Mock<ILogger<IPaymentService>> _logger;
        private readonly Mock<Account> _account;
        public PaymentServiceTests()
        {
            _accountDataStoreProvider = new Mock<IAccountDataStoreProvider>();
            _accountValidator = new Mock<IAccountValidator>();
            _paymentServiceOptions = new Mock<IOptions<PaymentServiceOptions>>();
            _logger = new Mock<ILogger<IPaymentService>>();

            _paymentServiceOptions
                .Setup(opt => opt.Value)
                .Returns(new PaymentServiceOptions());

            _account = new Mock<Account>();
        }

        [Fact]
        public void PaymentServiceTests_When_AccountValidationFails_Then_Return_UnsuccessfullPaymentResult()
        {
            //Arrange

            _accountDataStoreProvider.Setup(accountDataStoreProvider =>
                                                accountDataStoreProvider
                                                    .GetAccountDataStore(It.IsAny<string>())
                                                    .GetAccount(It.IsAny<string>()))
                                     .Returns(_account.Object);

            _accountValidator.Setup(accountValidator => accountValidator.CanProcessPayment(It.IsAny<MakePaymentRequest>(), It.IsAny<Account>()))
                             .Returns(new ValidationAccountResult());

            var sut = new PaymentService(
                        _accountDataStoreProvider.Object,
                        _accountValidator.Object,
                        _paymentServiceOptions.Object,
                        _logger.Object
                        );

            //Act
            var result = sut.MakePayment(new MakePaymentRequest());

            //Assert
            Assert.True(!result.Success);
            _account.Verify(a => a.WithdrawAmount(It.IsAny<decimal>()), Times.Never());
        }

        [Fact]
        public void PaymentServiceTests_When_AccountValidationPasses_Then_Return_SuccessfullPaymentResult()
        {
            //Arrange

            _accountDataStoreProvider.Setup(accountDataStoreProvider =>
                                                accountDataStoreProvider
                                                    .GetAccountDataStore(It.IsAny<string>())
                                                    .GetAccount(It.IsAny<string>()))
                                     .Returns(_account.Object);

            _accountValidator.Setup(accountValidator => accountValidator.CanProcessPayment(It.IsAny<MakePaymentRequest>(), It.IsAny<Account>()))
                             .Returns(new ValidationAccountResult { IsSuccess = true});

            var sut = new PaymentService(
                        _accountDataStoreProvider.Object,
                        _accountValidator.Object,
                        _paymentServiceOptions.Object,
                        _logger.Object
                        );

            //Act
            var result = sut.MakePayment(new MakePaymentRequest());

            //Assert
            Assert.True(result.Success);
            _account.Verify(a => a.WithdrawAmount(It.IsAny<decimal>()), Times.Once());
        }
    }
}
