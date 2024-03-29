using BankSystem.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bank_System_Test
{
    public static class HelperNUnit
    {
        public static void AssertIsOkResult(object result, object expectedResponse)
        {
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(HelperFunctions.HttpStatusCodeOK, okResult.StatusCode);
            Assert.AreEqual(expectedResponse, okResult.Value);
        }

        public static void AssertIsNotFoundResult(object result, object expectedResponse)
        {
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(HelperFunctions.HttpStatusCodeNotFound, notFoundResult.StatusCode);
            Assert.AreEqual(expectedResponse, notFoundResult.Value);
        }

        public static void AssertIsBadRequestResult(object result, object expectedResponse)
        {
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual(HelperFunctions.HttpStatusCodeBadRequest, badRequestResult.StatusCode);
            Assert.AreEqual(expectedResponse, badRequestResult.Value);
        }
    }
}
