using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MyAlbum.Tests
{
    public class Utilities
    {
        public static ControllerContext SetupCurrentUserForController(string expectedUserName, Mock<HttpContext> mockContext = null)
        {
            if (mockContext == null)
                mockContext = new Mock<HttpContext>();

            ClaimsPrincipal claimUser = new ClaimsPrincipal();
            ClaimsIdentity claimIdentity = new ClaimsIdentity();
            claimIdentity.AddClaim(new Claim(ClaimTypes.Name, expectedUserName));
            claimUser.AddIdentity(claimIdentity);
            mockContext.SetupGet(ctx => ctx.User).Returns(claimUser);
            ControllerContext controllerContext = new ControllerContext();
            controllerContext.HttpContext = mockContext.Object;
            return controllerContext;
        }
    }
}