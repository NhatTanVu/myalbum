using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MyAlbum.Tests
{
    public class Utilities
    {
        public static ControllerContext SetupCurrentUserForController(string expectedUserName)
        {
            ClaimsPrincipal claimUser = new ClaimsPrincipal();
            ClaimsIdentity claimIdentity = new ClaimsIdentity();
            claimIdentity.AddClaim(new Claim(ClaimTypes.Name, expectedUserName));
            claimUser.AddIdentity(claimIdentity);
            var mockContext = new Mock<HttpContext>();
            mockContext.SetupGet(ctx => ctx.User).Returns(claimUser);
            ControllerContext controllerContext = new ControllerContext();
            controllerContext.HttpContext = mockContext.Object;
            return controllerContext;
        }
    }
}