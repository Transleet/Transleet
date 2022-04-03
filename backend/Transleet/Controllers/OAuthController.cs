using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Transleet.Models;

namespace Transleet.Controllers
{
    [Route("oauth")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OAuthController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IDataProtector _dataProtector;
        private readonly IHttpClientFactory _httpClientFactory;

        public OAuthController(
            IConfiguration configuration,
            ILogger<OAuthController> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IDataProtectionProvider dataProtectionProvider,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _dataProtector = dataProtectionProvider.CreateProtector("Token");
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("github_login")]
        public IActionResult GithubLogin([FromQuery] string returnUrl)
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/oauth/github_callback1?returnUrl=" + returnUrl }, "Github");
        }

        [HttpGet("github_callback1")]
        public async Task<IActionResult> GithubCallback1([FromQuery] string returnUrl)
        {
            var result = await HttpContext.AuthenticateAsync("Github");
            var state = _dataProtector.Protect(result.Ticket!.Properties.GetTokenValue("access_token")!);
                return Redirect(returnUrl + "?state=" + state);
        }

        [HttpPost("github_callback2")]
        public async Task<IActionResult> GithubCallback3([FromQuery]string state)
        {
            var accessToken = _dataProtector.Unprotect(state);
            _logger.LogDebug(accessToken);
            using var httpClient = _httpClientFactory.CreateClient();
            using var userInfoRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
            userInfoRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            userInfoRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            userInfoRequest.Headers.UserAgent.Add(new ProductInfoHeaderValue("Transleet", "1.0.0"));
            using var userInfoResponse = await httpClient.SendAsync(userInfoRequest, HttpContext.RequestAborted);
            userInfoResponse.EnsureSuccessStatusCode();
            var userInfo = await userInfoResponse.Content.ReadFromJsonAsync<GithubUserInfo>(cancellationToken: HttpContext.RequestAborted);
            using var userEmailsRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user/emails");
            userEmailsRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            userEmailsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            userEmailsRequest.Headers.UserAgent.Add(new ProductInfoHeaderValue("Transleet", "1.0.0"));
            using var userEmailsResponse = await httpClient.SendAsync(userEmailsRequest, HttpContext.RequestAborted);
            userEmailsResponse.EnsureSuccessStatusCode();
            userInfo!.Emails =
                await userEmailsResponse.Content.ReadFromJsonAsync<List<GithubEmailInfo>>(
                    cancellationToken: HttpContext.RequestAborted);
            var primaryEmail = userInfo.Emails!.FirstOrDefault(_ => _.Primary = true);
            var user = await _userManager.FindByEmailAsync(primaryEmail!.Email);
            if (user is null)
            {
                user = new User()
                {
                    UserName = userInfo.Name,
                    Email = primaryEmail.Email,
                    EmailConfirmed = true,
                    GithubUserInfo = userInfo
                };
                await _userManager.CreateAsync(user);
            }
            await _signInManager.SignInAsync(user, true);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Authentication:JwtBearer:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _configuration["Authentication:JwtBearer:Issuer"],
                Audience = _configuration["Authentication:JwtBearer:Audience"],
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.UserName) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            await _signInManager.SignInAsync(user, true);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { token = tokenHandler.WriteToken(token) });
        }
    }
}

