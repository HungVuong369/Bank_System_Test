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
    public class BalanceControllerTest
    {
        private readonly BalanceController _balanceController;
        private readonly Mock<IBalanceService> _mockBalanceService = new();

        public BalanceControllerTest()
        {
            _balanceController = new BalanceController(_mockBalanceService.Object);
        }

        private void SetupMockBalanceService_GetBalance(ResponseDto expectedResponse)
        {
            _mockBalanceService.Setup(x => x.GetBalance(It.IsAny<string>(), It.IsAny<string>()))
                                 .Returns(expectedResponse);
        }

        private void SetupMockBalanceService_HoldAmount(ResponseDto expectedResponse)
        {
            _mockBalanceService.Setup(x => x.HoldAmount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()))
                                 .Returns(expectedResponse);
        }

        private void SetupMockBalanceService_UnholdAmount(ResponseDto expectedResponse)
        {
            _mockBalanceService.Setup(x => x.UnHoldAmount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()))
                                 .Returns(expectedResponse);
        }

        [Test]
        public void GetBalance_Returns_OkResult_When_Success()
        {
            var expectedResponse = new ResponseDto();

            SetupMockBalanceService_GetBalance(expectedResponse);

            var accountRequest = new AccountRequest
            {
                AccountNo = "2182415246",
                IdCard = "235236555233"
            };

            var result = _balanceController.GetBalance(accountRequest.AccountNo, accountRequest.IdCard);

            HelperNUnit.AssertIsOkResult(result, expectedResponse);
        }

        [Test]
        public void GetBalance_Returns_BadRequest_When_IdCardDoesNotBelongToAccount()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeIdCard_Does_Not_Belong_To_Account);

            SetupMockBalanceService_GetBalance(expectedResponse);

            var accountRequest = new AccountRequest
            {
                AccountNo = "2182415246",
                IdCard = "2352233"
            };

            var result = _balanceController.GetBalance(accountRequest.AccountNo, accountRequest.IdCard);

            HelperNUnit.AssertIsBadRequestResult(result, expectedResponse);
        }

        [Test]
        public void GetBalance_Returns_NotFound()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeNotFound);

            SetupMockBalanceService_GetBalance(expectedResponse);

            var accountRequest = new AccountRequest
            {
                AccountNo = "2182",
                IdCard = "2352233"
            };

            var result = _balanceController.GetBalance(accountRequest.AccountNo, accountRequest.IdCard);

            HelperNUnit.AssertIsNotFoundResult(result, expectedResponse);
        }

        [Test]
        public void GetBalance_Returns_BadRequest_When_DoesNotHaveBalance()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeDoesNotHaveBalance);

            SetupMockBalanceService_GetBalance(expectedResponse);

            var accountRequest = new AccountRequest
            {
                AccountNo = "2182415246",
                IdCard = "235236555233"
            };

            var result = _balanceController.GetBalance(accountRequest.AccountNo, accountRequest.IdCard);

            HelperNUnit.AssertIsBadRequestResult(result, expectedResponse);
        }

        [Test]
        public void HoldAmount_Returns_OkResult_When_Success()
        {
            var expectedResponse = new ResponseDto(new BalanceTotal
            {
                HoldAmount = 1000,
                UsableAmount = 1000
            });

            SetupMockBalanceService_HoldAmount(expectedResponse);

            var request = new HoldAmountRequest
            {
                AccountNo = "2182415246",
                IdCard = "235236555233",
                Amount = 1000,
                ApproveBy = "MinhNH",
                Description = "MinhNH Hold Amount 1000"
            };

            var result = _balanceController.HoldAmount(request);

            HelperNUnit.AssertIsOkResult(result, expectedResponse);
        }

        [Test]
        public void HoldAmount_Returns_NotFound()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeNotFound);

            SetupMockBalanceService_HoldAmount(expectedResponse);

            var request = new HoldAmountRequest
            {
                AccountNo = "123",
                IdCard = "123",
                Amount = 1000,
                ApproveBy = "MinhNH",
                Description = "MinhNH Hold Amount 1000"
            };

            var result = _balanceController.HoldAmount(request);

            HelperNUnit.AssertIsNotFoundResult(result, expectedResponse);
        }

        [Test]
        public void HoldAmount_Returns_BadRequest_When_DoesNotHaveBalance()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeDoesNotHaveBalance);

            SetupMockBalanceService_HoldAmount(expectedResponse);

            var request = new HoldAmountRequest
            {
                AccountNo = "2182415246",
                IdCard = "235236555233",
                Amount = 1000,
                ApproveBy = "MinhNH",
                Description = "MinhNH Hold Amount 1000"
            };

            var result = _balanceController.HoldAmount(request);

            HelperNUnit.AssertIsBadRequestResult(result, expectedResponse);
        }

        [Test]
        public void HoldAmount_Returns_BadRequest_When_HoldAmountExceedUsable()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeHoldAmountExceedUsable);

            SetupMockBalanceService_HoldAmount(expectedResponse);

            var request = new HoldAmountRequest
            {
                AccountNo = "2182415246",
                IdCard = "235236555233",
                Amount = 1000000000,
                ApproveBy = "MinhNH",
                Description = "MinhNH Hold Amount 1000"
            };

            var result = _balanceController.HoldAmount(request);

            HelperNUnit.AssertIsBadRequestResult(result, expectedResponse);
        }

        [Test]
        public void UnholdAmount_Returns_OkResult_When_Success()
        {
            var expectedResponse = new ResponseDto(new BalanceTotal
            {
                HoldAmount = 0,
                UsableAmount = 2000
            });

            SetupMockBalanceService_UnholdAmount(expectedResponse);

            var request = new UnholdAmount
            {
                AccountNo = "2182415246",
                IdCard = "235236555233",
                Amount = 1000,
                ApproveBy = "MinhNH",
                Description = "MinhNH Unhold Amount 1000"
            };

            var result = _balanceController.UnholdAmount(request);

            HelperNUnit.AssertIsOkResult(result, expectedResponse);
        }

        [Test]
        public void UnholdAmount_Returns_NotFound()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeNotFound);

            SetupMockBalanceService_UnholdAmount(expectedResponse);

            var request = new UnholdAmount
            {
                AccountNo = "1",
                IdCard = "3592",
                Amount = 1000,
                ApproveBy = "MinhNH",
                Description = "MinhNH Unhold Amount 1000"
            };

            var result = _balanceController.UnholdAmount(request);

            HelperNUnit.AssertIsNotFoundResult(result, expectedResponse);
        }

        [Test]
        public void UnholdAmount_Returns_BadRequest_When_DoesNotHaveBalance()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeDoesNotHaveBalance);

            SetupMockBalanceService_UnholdAmount(expectedResponse);

            var request = new UnholdAmount
            {
                AccountNo = "2182415246",
                IdCard = "235236555233",
                Amount = 1000,
                ApproveBy = "MinhNH",
                Description = "MinhNH Unhold Amount 1000"
            };

            var result = _balanceController.UnholdAmount(request);

            HelperNUnit.AssertIsBadRequestResult(result, expectedResponse);
        }

        [Test]
        public void UnholdAmount_Returns_BadRequest_When_UnholdAmountExceedHold()
        {
            var expectedResponse = HelperFunctions.Instance.GetErrorResponseByError(HelperFunctions.ErrorCodeUnholdAmountExceedUsable);

            SetupMockBalanceService_UnholdAmount(expectedResponse);

            var request = new UnholdAmount
            {
                AccountNo = "2182415246",
                IdCard = "235236555233",
                Amount = 100000000,
                ApproveBy = "MinhNH",
                Description = "MinhNH Unhold Amount 1000"
            };

            var result = _balanceController.UnholdAmount(request);

            HelperNUnit.AssertIsBadRequestResult(result, expectedResponse);
        }
    }
}
