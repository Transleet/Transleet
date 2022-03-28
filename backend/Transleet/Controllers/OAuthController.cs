using System.Net.Http.Headers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation;
using Transleet.Models;

namespace Transleet.Controllers
{
    [Route("oauth")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OAuthController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly OpenIddictValidationService _validationService;
        private readonly IDataProtector _dataProtector;
        private readonly IHttpClientFactory _httpClientFactory;

        public OAuthController(
            IConfiguration configuration,
            ILogger<OAuthController> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            OpenIddictValidationService validationService,
            IDataProtectionProvider dataProtectionProvider,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _dataProtector = dataProtectionProvider.CreateProtector("Token");
            _httpClientFactory = httpClientFactory;
            _validationService = validationService;
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

        [HttpPost("github_callback3")]
        public async Task<IActionResult> GithubCallback3()
        {

            var request = HttpContext.GetOpenIddictServerRequest();
            if (request!.GrantType != "github")
            {
                return BadRequest();
            }
            var state = (string)request.GetParameter("state")!;
            var accessToken = _dataProtector.Unprotect(state!);
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
                user = new ApplicationUser()
                {
                    UserName = userInfo.Name,
                    Email = primaryEmail.Email,
                    EmailConfirmed = true,
                    GithubUserInfo = userInfo
                };
                await _userManager.CreateAsync(user);
            }
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }
}

