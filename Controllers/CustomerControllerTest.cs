using BankSystem.Controllers;
using BankSystem.Data;
using BankSystem.Data.Repositories;
using BankSystem.Dtos.Request;
using BankSystem.Dtos.Response;
using BankSystem.Models;
using BankSystem.Repository;
using BankSystem.Service;
using BankSystem.Utilities;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Bank_System_Test.Controllers
{
    [TestFixture]
    public class CustomerControllerTest
    {
        private readonly CustomerController _CustomerController;
        private readonly Mock<ICustomerService> _mockCustomerService = new();

        public CustomerControllerTest()
        {
            _CustomerController = new CustomerController(_mockCustomerService.Object);
        }

        private OpenCustomerRequest GetSampleOpenCustomerRequest(string accountNo = "1234678", string idCard = "12346780")
        {
            return new OpenCustomerRequest
            {
                AccountNo = accountNo,
                IdCard = idCard,
                Name = "John Doe 1",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "123 Main St",
                PhoneNumber = "123321",
                CardPlace = "Someplace",
                TypeId = 1,
                UserId = 1,
            };
        }

        private void SetupMockCustomerService_GetAccount(ResponseDto expectedResponse)
        {
            _mockCustomerService.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<string>()))
                                 .Returns(expectedResponse);
        }

        private void SetupMockCustomerService_OpenCustomer(ResponseDto expectedResponse)
        {
            _mockCustomerService.Setup(x => x.OpenCustomer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<byte>(), It.IsAny<long>()))
                                 .Returns(expectedResponse);
        }

        [Test]
        public void GetAccount_Returns_OkResult_When_Success()
        {
            var expectedResponse = new ResponseDto();
            
            SetupMockCustomerService_GetAccount(expectedResponse);
            
            var accountRequest = new AccountRequest
            {
                AccountNo = "2182415246",
                IdCard = "235236555233"
            };

            var result = _CustomerController.GetAccount(accountRequest.AccountNo, accountRequest.IdCard);

            HelperNUnit.AssertIsOkResult(result, expectedResponse);
        }

        [Test]
        public void GetAccount_Returns_NotFound_When_Account_Not_Found()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeNotFound);
            SetupMockCustomerService_GetAccount(expectedResponse);

            var accountRequest = new AccountRequest
            {
                AccountNo = "2182415246",
                IdCard = "23523655523"
            };

            var result = _CustomerController.GetAccount(accountRequest.AccountNo, accountRequest.IdCard);

            HelperNUnit.AssertIsNotFoundResult(result, expectedResponse);
        }

        [Test]
        public void GetAccount_Returns_NotFound_When_IdCard_Does_Not_Belong_To_Account()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeIdCard_Does_Not_Belong_To_Account);
            SetupMockCustomerService_GetAccount(expectedResponse);

            var accountRequest = new AccountRequest
            {
                AccountNo = "218215246",
                IdCard = "23523655523"
            };

            var result = _CustomerController.GetAccount(accountRequest.AccountNo, accountRequest.IdCard);

            HelperNUnit.AssertIsBadRequestResult(result, expectedResponse);
        }

        [Test]
        public void OpenCustomer_Success_ReturnsOkResult()
        {
            var expectedResponse = new ResponseDto();

            SetupMockCustomerService_OpenCustomer(expectedResponse);

            var openCustomerRequest = GetSampleOpenCustomerRequest();

            var result = _CustomerController.OpenCustomer(openCustomerRequest);

            HelperNUnit.AssertIsOkResult(result, expectedResponse);
        }

        [Test]
        public void OpenCustomer_AccountNoIsUsed_ReturnsBadRequest()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeAccountNoUsed);
            
            SetupMockCustomerService_OpenCustomer(expectedResponse);

            var openCustomerRequest = GetSampleOpenCustomerRequest(accountNo: "2182415246");

            var result = _CustomerController.OpenCustomer(openCustomerRequest);

            HelperNUnit.AssertIsBadRequestResult(result, expectedResponse);
        }

        [Test]
        public void OpenCustomer_IdCardIsUsed_ReturnsBadRequest()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeIdCardUsed);

            SetupMockCustomerService_OpenCustomer(expectedResponse);

            var openCustomerRequest = GetSampleOpenCustomerRequest(idCard: "085600225426");

            var result = _CustomerController.OpenCustomer(openCustomerRequest);

            HelperNUnit.AssertIsBadRequestResult(result, expectedResponse);
        }
    }
}