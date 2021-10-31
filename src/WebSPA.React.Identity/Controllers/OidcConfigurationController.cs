using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyAlbum.Services.Identity.API.Core.Models;

namespace MyAlbum.Services.Indentity.API.Controllers
{
    [ApiExplorerSettings(GroupName = "JWT")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage()]
    public class OidcConfigurationController : Controller
    {
        private readonly ILogger<OidcConfigurationController> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration config;

        public OidcConfigurationController(IClientRequestParametersProvider clientRequestParametersProvider,
            ILogger<OidcConfigurationController> _logger, IHttpContextAccessor _httpContextAccessor,
            SignInManager<ApplicationUser> _signInManager, [FromServices] IConfiguration _config)
        {
            ClientRequestParametersProvider = clientRequestParametersProvider;
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
            signInManager = _signInManager;
            config = _config;
        }

        public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("GetConfiguration")]
        public IActionResult GetConfiguration()
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("IssuerUri", config.GetValue<string>("IssuerUri")); 
            parameters.Add("AlbumApiUrl", config.GetValue<string>("AlbumApiUrl"));
            parameters.Add("CommentApiUrl", config.GetValue<string>("CommentApiUrl"));
            parameters.Add("PhotoApiUrl", config.GetValue<string>("PhotoApiUrl"));
            parameters.Add("GoogleApiKey", config.GetValue<string>("GoogleApiKey"));
            return Ok(parameters);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("_configuration/{clientId}")]
        public IActionResult GetClientRequestParameters([FromRoute] string clientId)
        {
            var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
            return Ok(parameters);
        }

        private string GetHostUrl(HttpRequest request)
        {
            return string.Format("{0}://{1}", request.Scheme, request.Host);
        }

        private string GenerateJSONWebToken(string userName, int expireMinutes = 20)
        {
            string filePath = config.GetSection("IdentityServer").GetSection("Key").GetValue<string>("FilePath");
            string filePassword = config.GetSection("IdentityServer").GetSection("Key").GetValue<string>("Password");
            X509Certificate2 signingCert = new X509Certificate2(filePath, filePassword);
            X509SecurityKey privateKey = new X509SecurityKey(signingCert);
            var now = DateTime.UtcNow;
            var tokenHandler = new JwtSecurityTokenHandler();
            var currentUrl = GetHostUrl(this.httpContextAccessor.HttpContext.Request);
            var newToken = new JwtSecurityToken(currentUrl, "MyAlbum.DeveloperAPI",
                new List<Claim>() {
                    new Claim(System.Security.Claims.ClaimTypes.Name, userName)
                }, now, now.AddMinutes(Convert.ToInt32(expireMinutes)),
                new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256Signature));

            string token = tokenHandler.WriteToken(newToken);
            return token;
        }

        [HttpPost("api/JWT/Generate")]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> GenerateToken([FromForm] string UserName, [FromForm] string Password)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                return Unauthorized();
                
            var result = await this.signInManager.PasswordSignInAsync(UserName, Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var tokenString = GenerateJSONWebToken(UserName);
                var response = Ok(new { token = tokenString });

                return response;
            }
            else
                return Unauthorized();
        }

        [HttpPost("api/JWT/Read")]
        [ProducesResponseType((int)System.Net.HttpStatusCode.OK)]
        public IActionResult ReadToken([FromForm] string Token)
        {
            if (string.IsNullOrEmpty(Token))
                return NoContent();

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = (JwtSecurityToken)tokenHandler.ReadToken(Token);

            return Ok(new { token = token }); ;
        }
    }
}
