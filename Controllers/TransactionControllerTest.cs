using BankSystem.Controllers;
using BankSystem.Dtos.Request;
using BankSystem.Dtos.Response;
using BankSystem.Repository;
using BankSystem.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_System_Test.Controllers
{
    public class TransactionControllerTest
    {
        private readonly TransactionController transactionController;
        private readonly Mock<ITransactionService> _mockTransactionService = new();

        public TransactionControllerTest()
        {
            transactionController = new TransactionController(_mockTransactionService.Object);
        }

        private void SetupMockTransactionService_DepositMoney(ResponseDto expectedResponse)
        {
            _mockTransactionService.Setup(x => x.DepositMoney(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string>()))
                                 .Returns(expectedResponse);
        }

        private void SetupMockTransactionService_DepositApproval(ResponseDto expectedResponse)
        {
            _mockTransactionService.Setup(x => x.DepositApproval(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                                 .Returns(expectedResponse);
        }

        private void SetupMockTransactionService_SellPayment(ResponseDto expectedResponse)
        {
            _mockTransactionService.Setup(x => x.SellPayment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string>()))
                                 .Returns(expectedResponse);
        }

        private void SetupMockTransactionService_BuyPayment(ResponseDto expectedResponse)
        {
            _mockTransactionService.Setup(x => x.BuyPayment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string>()))
                                 .Returns(expectedResponse);
        }

        [Test]
        public void DepositMoney_Returns_OkResult_When_Success()
        {
            var expectedResponse = new ResponseDto();

            SetupMockTransactionService_DepositMoney(expectedResponse);

            var accountRequest = new DepositRequest
            {
                AccountNo = "2182415246",
                Amount = 1000,
                Description = "Deposit 1000"
            };

            var result = transactionController.DepositMoney(accountRequest);

            HelperNUnit.AssertIsOkResult(result, expectedResponse);
        }

        [Test]
        public void DepositMoney_Returns_NotFound()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeNotFound);

            SetupMockTransactionService_DepositMoney(expectedResponse);

            var request = new DepositRequest
            {
                AccountNo = "215246",
                Amount = 1000,
                Description = "Deposit 1000"
            };

            var result = transactionController.DepositMoney(request);

            HelperNUnit.AssertIsNotFoundResult(result, expectedResponse);
        }

        [Test]
        public void DepositApproval_Returns_OkResult_When_Success()
        {
            var expectedResponse = new ResponseDto();

            SetupMockTransactionService_DepositApproval(expectedResponse);

            var request = new DepositApprovalRequest
            {
                ApproveBy = "TanNH",
                Status = 1,
                TransactionId = "dda8cfdc-b116-11ee-aea6-0242ac110002"
            };

            var result = transactionController.DepositApproval(request);

            HelperNUnit.AssertIsOkResult(result, expectedResponse);
        }

        [Test]
        public void DepositApproval_Returns_NotFound()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeNotFound);

            SetupMockTransactionService_DepositApproval(expectedResponse);

            var request = new DepositApprovalRequest
            {
                ApproveBy = "TanNH",
                Status = 1,
                TransactionId = "ddab116-11ee-aea6-0242ac110002"
            };

            var result = transactionController.DepositApproval(request);

            HelperNUnit.AssertIsNotFoundResult(result, expectedResponse);
        }

        [Test]
        public void SellPayment_Returns_OkResult_When_Success()
        {
            var expectedResponse = new ResponseDto();

            SetupMockTransactionService_SellPayment(expectedResponse);

            var request = new SellPaymentRequest
            {
                AccountNo = "123456789",
                IdCard = "1234567890",
                SecuritiesAccount = "2182415246",
                SecuritiesAccountIdCard = "235236555233",
                Amount = 1000,
                Descriptions = "123456789 sell payment 2182415246"
            };

            var result = transactionController.SellPayment(request);

            HelperNUnit.AssertIsOkResult(result, expectedResponse);
        }

        [Test]
        public void SellPayment_Returns_NotFound()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeNotFound);

            SetupMockTransactionService_SellPayment(expectedResponse);

            var request = new SellPaymentRequest
            {
                AccountNo = "string",
                IdCard = "string",
                SecuritiesAccount = "string",
                SecuritiesAccountIdCard = "string",
                Amount = 1000,
                Descriptions = "123456789 sell payment 2182415246"
            };

            var result = transactionController.SellPayment(request);

            HelperNUnit.AssertIsNotFoundResult(result, expectedResponse);
        }

        [Test]
        public void SellPayment_Returns_BadRequest_When_DoesNotHaveBalance()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeAccountNoUsed);

            SetupMockTransactionService_SellPayment(expectedResponse);

            var request = new SellPaymentRequest
            {
                AccountNo = "123456789",
                IdCard = "1234567890",
                SecuritiesAccount = "2182415246",
                SecuritiesAccountIdCard = "235236555233",
                Amount = 1000,
                Descriptions = "123456789 sell payment 2182415246"
            };

            var result = transactionController.SellPayment(request);

            HelperNUnit.AssertIsBadRequestResult(result, expectedResponse);
        }

        [Test]
        public void SellPayment_Returns_BadRequest_When_AmountTakenExceed()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeAmountTakenExceed);

            SetupMockTransactionService_SellPayment(expectedResponse);

            var request = new SellPaymentRequest
            {
                AccountNo = "123456789",
                IdCard = "1234567890",
                SecuritiesAccount = "2182415246",
                SecuritiesAccountIdCard = "235236555233",
                Amount = 10000000000,
                Descriptions = "123456789 sell payment 2182415246"
            };

            var result = transactionController.SellPayment(request);

            HelperNUnit.AssertIsBadRequestResult(result, expectedResponse);
        }

        [Test]
        public void BuyPayment_Returns_OkResult_When_Success()
        {
            var expectedResponse = new ResponseDto();

            SetupMockTransactionService_BuyPayment(expectedResponse);

            var request = new BuyPaymentRequest
            {
                AccountNo = "123456789",
                IdCard = "1234567890",
                SecuritiesAccount = "2182415246",
                SecuritiesAccountIdCard = "235236555233",
                Amount = 1000,
                Description = "123456789 buy payment 2182415246"
            };

            var result = transactionController.BuyPayment(request);

            HelperNUnit.AssertIsOkResult(result, expectedResponse);
        }

        [Test]
        public void BuyPayment_Returns_NotFound()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeNotFound);

            SetupMockTransactionService_BuyPayment(expectedResponse);

            var request = new BuyPaymentRequest
            {
                AccountNo = "string",
                IdCard = "string",
                SecuritiesAccount = "string",
                SecuritiesAccountIdCard = "string",
                Amount = 1000,
                Description = "123456789 buy payment 2182415246"
            };

            var result = transactionController.BuyPayment(request);

            HelperNUnit.AssertIsNotFoundResult(result, expectedResponse);
        }

        [Test]
        public void BuyPayment_Returns_BadRequest_When_DoesNotHaveBalance()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeAccountNoUsed);

            SetupMockTransactionService_BuyPayment(expectedResponse);

            var request = new BuyPaymentRequest
            {
                AccountNo = "123456789",
                IdCard = "1234567890",
                SecuritiesAccount = "2182415246",
                SecuritiesAccountIdCard = "235236555233",
                Amount = 1000,
                Description = "123456789 sell payment 2182415246"
            };

            var result = transactionController.BuyPayment(request);

            HelperNUnit.AssertIsBadRequestResult(result, expectedResponse);
        }

        [Test]
        public void BuyPayment_Returns_BadRequest_When_AmountTakenExceed()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeAmountTakenExceed);

            SetupMockTransactionService_BuyPayment(expectedResponse);

            var request = new BuyPaymentRequest
            {
                AccountNo = "123456789",
                IdCard = "1234567890",
                SecuritiesAccount = "2182415246",
                SecuritiesAccountIdCard = "235236555233",
                Amount = 10000000000,
                Description = "123456789 sell payment 2182415246"
            };

            var result = transactionController.BuyPayment(request);

            HelperNUnit.AssertIsBadRequestResult(result, expectedResponse);
        }
    }
}
