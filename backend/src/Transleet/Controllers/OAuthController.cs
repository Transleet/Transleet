using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

using Orleans.Concurrency;
using Orleans.Providers.Streams.Common;

using Transleet.Models;

namespace Transleet.Controllers
{
    [Route("api/oauth")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOptionsMonitor<JwtOptions> _jwtBearerOptions;
        private readonly IOptionsMonitor<GithubOAuthOptions> _githubOAuthOptions;
        private readonly ILogger<OAuthController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IDataProtector _dataProtector;
        private readonly IHttpClientFactory _httpClientFactory;

        public OAuthController(
            IConfiguration configuration,
            IOptionsMonitor<GithubOAuthOptions> githubOAuthOptions,
            IOptionsMonitor<JwtOptions> jwtBearerOptions,
            ILogger<OAuthController> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IDataProtectionProvider dataProtectionProvider,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _githubOAuthOptions = githubOAuthOptions;
            _jwtBearerOptions = jwtBearerOptions;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _dataProtector = dataProtectionProvider.CreateProtector("Token");
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("github_login")]
        public IActionResult GithubLogin([FromQuery] string returnUrl)
        {
            var request = HttpContext.Request;
            var url =
                $"https://github.com/login/oauth/authorize?client_id={_githubOAuthOptions.CurrentValue.ClientId}&redirect_uri={request.Scheme}://{request.Host}/api/oauth/github_callback&scope=user&state={_dataProtector.Protect(returnUrl)}";
            return Redirect(url);
        }

        [HttpGet("github_callback")]
        public async Task<IActionResult> GithubCallback([FromQuery] string code, [FromQuery] string state)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var res = await httpClient.PostAsJsonAsync("https://github.com/login/oauth/access_token", new
            {
                client_id = _githubOAuthOptions.CurrentValue.ClientId,
                client_secret = _githubOAuthOptions.CurrentValue.ClientSecret,
                code
            });
            var body = await res.Content.ReadAsStringAsync();
            var accessToken = body.Split('&')[0].Split('=')[1];
            return Redirect(_dataProtector.Unprotect(state) + $"?state={_dataProtector.Protect(accessToken)}");
        }

        [HttpGet("github_callback1")]
        public async Task<IActionResult> GithubCallback1([FromQuery] string state)
        {
            var accessToken = _dataProtector.Unprotect(state);
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
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _jwtBearerOptions.CurrentValue.Issuer,
                Audience = _jwtBearerOptions.CurrentValue.Audience,
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.UserName!), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Email, user.Email!) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials =
                    new SigningCredentials(_jwtBearerOptions.CurrentValue.Key, SecurityAlgorithms.HmacSha256)
            };
            await _signInManager.SignInAsync(user, true);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { token = tokenHandler.WriteToken(token) });
        }
    }
}

