#nullable enable
using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Orleans.CodeGeneration;
using Orleans.Serialization;

namespace Transleet.Models
{

    public class User : IdentityUser<Guid>
    {
        public GithubUserInfo? GithubUserInfo { get; set; }

    }
    public class GithubUserInfo
    {
        [JsonPropertyName("login")]
        public string? Login { get; set; }

        [JsonPropertyName("avatar_url")]
        public Uri? AvatarUrl { get; set; }

        [JsonPropertyName("gravatar_id")]
        public string? GravatarId { get; set; }

        [JsonPropertyName("url")]
        public Uri? Url { get; set; }

        [JsonPropertyName("html_url")]
        public Uri? HtmlUrl { get; set; }

        [JsonPropertyName("followers_url")]
        public Uri? FollowersUrl { get; set; }

        [JsonPropertyName("following_url")]
        public string? FollowingUrl { get; set; }

        [JsonPropertyName("gists_url")]
        public string? GistsUrl { get; set; }

        [JsonPropertyName("starred_url")]
        public string? StarredUrl { get; set; }

        [JsonPropertyName("subscriptions_url")]
        public Uri? SubscriptionsUrl { get; set; }

        [JsonPropertyName("organizations_url")]
        public Uri? OrganizationsUrl { get; set; }

        [JsonPropertyName("repos_url")]
        public Uri? ReposUrl { get; set; }

        [JsonPropertyName("events_url")]
        public string? EventsUrl { get; set; }

        [JsonPropertyName("received_events_url")]
        public Uri? ReceivedEventsUrl { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("site_admin")]
        public bool? SiteAdmin { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("company")]
        public object? Company { get; set; }

        [JsonPropertyName("blog")]
        public string? Blog { get; set; }

        [JsonPropertyName("location")]
        public object? Location { get; set; }

        [JsonIgnore]
        public List<GithubEmailInfo>? Emails { get; set; }

        [JsonPropertyName("hireable")]
        public bool? Hireable { get; set; }

        [JsonPropertyName("bio")]
        public object? Bio { get; set; }

        [JsonPropertyName("twitter_username")]
        public object? TwitterUsername { get; set; }

        [JsonPropertyName("public_repos")]
        public long PublicRepos { get; set; }

        [JsonPropertyName("public_gists")]
        public long PublicGists { get; set; }

        [JsonPropertyName("followers")]
        public long Followers { get; set; }

        [JsonPropertyName("following")]
        public long Following { get; set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonPropertyName("private_gists")]
        public long PrivateGists { get; set; }

        [JsonPropertyName("total_private_repos")]
        public long TotalPrivateRepos { get; set; }

        [JsonPropertyName("owned_private_repos")]
        public long OwnedPrivateRepos { get; set; }

        [JsonPropertyName("disk_usage")]
        public long DiskUsage { get; set; }

        [JsonPropertyName("collaborators")]
        public long Collaborators { get; set; }

        [JsonPropertyName("two_factor_authentication")]
        public bool? TwoFactorAuthentication { get; set; }
    }

    public class GithubEmailInfo
    {
        [JsonPropertyName("email")] public string? Email { get; set; }

        [JsonPropertyName("primary")] public bool Primary { get; set; }

        [JsonPropertyName("verified")] public bool Verified { get; set; }

        [JsonPropertyName("visibility")] public string? Visibility { get; set; }
    }

    [Serializer(typeof(User))]
    internal class UserSerializer
    {
        [CopierMethod]
        public static object DeepCopier(object original, ICopyContext context)
        {
            var input = (User)original;
            var result = new User();
            context.RecordCopy(original, result);
            result.Id = input.Id;
            result.UserName = input.UserName;
            result.Email = input.Email;
            result.EmailConfirmed = input.EmailConfirmed;
            result.NormalizedEmail = input.NormalizedEmail;
            result.NormalizedUserName = input.NormalizedUserName;
            result.ConcurrencyStamp = input.ConcurrencyStamp;
            result.PhoneNumber = input.PhoneNumber;
            result.PhoneNumberConfirmed = input.PhoneNumberConfirmed;
            result.TwoFactorEnabled = input.TwoFactorEnabled;
            result.LockoutEnd = input.LockoutEnd;
            result.LockoutEnabled = input.LockoutEnabled;
            result.PasswordHash = input.PasswordHash;
            result.SecurityStamp = input.SecurityStamp;
            result.AccessFailedCount = input.AccessFailedCount;
            result.GithubUserInfo = (GithubUserInfo)context.DeepCopyInner(input.GithubUserInfo);
            return result;
        }


        [SerializerMethod]
        public static void Serializer(
            object untypedInput, ISerializationContext context, Type expected)
        {
            var input = (User)untypedInput;
            SerializationManager.SerializeInner(input.Id, context);
            SerializationManager.SerializeInner(input.UserName, context);
            SerializationManager.SerializeInner(input.Email, context);
            SerializationManager.SerializeInner(input.EmailConfirmed, context);
            SerializationManager.SerializeInner(input.NormalizedEmail, context);
            SerializationManager.SerializeInner(input.NormalizedUserName, context);
            SerializationManager.SerializeInner(input.ConcurrencyStamp, context);
            SerializationManager.SerializeInner(input.PhoneNumber, context);
            SerializationManager.SerializeInner(input.PhoneNumberConfirmed, context);
            SerializationManager.SerializeInner(input.TwoFactorEnabled, context);
            SerializationManager.SerializeInner(input.LockoutEnd, context);
            SerializationManager.SerializeInner(input.LockoutEnabled, context);
            SerializationManager.SerializeInner(input.PasswordHash, context);
            SerializationManager.SerializeInner(input.SecurityStamp, context);
            SerializationManager.SerializeInner(input.AccessFailedCount, context);
            SerializationManager.SerializeInner(input.GithubUserInfo, context);
        }

        [DeserializerMethod]
        public static object Deserializer(
            Type expected, IDeserializationContext context)
        {
            var result = new User();
            context.RecordObject(result);
            result.Id = SerializationManager.DeserializeInner<Guid>(context);
            result.UserName = SerializationManager.DeserializeInner<string>(context);
            result.Email = SerializationManager.DeserializeInner<string>(context);
            result.EmailConfirmed = SerializationManager.DeserializeInner<bool>(context);
            result.NormalizedEmail = SerializationManager.DeserializeInner<string>(context);
            result.NormalizedUserName = SerializationManager.DeserializeInner<string>(context);
            result.ConcurrencyStamp = SerializationManager.DeserializeInner<string>(context);
            result.PhoneNumber = SerializationManager.DeserializeInner<string>(context);
            result.PhoneNumberConfirmed = SerializationManager.DeserializeInner<bool>(context);
            result.TwoFactorEnabled = SerializationManager.DeserializeInner<bool>(context);
            result.LockoutEnd = SerializationManager.DeserializeInner<DateTimeOffset>(context);
            result.LockoutEnabled = SerializationManager.DeserializeInner<bool>(context);
            result.PasswordHash = SerializationManager.DeserializeInner<string>(context);
            result.SecurityStamp = SerializationManager.DeserializeInner<string>(context);
            result.AccessFailedCount = SerializationManager.DeserializeInner<int>(context);
            result.GithubUserInfo = SerializationManager.DeserializeInner<GithubUserInfo>(context);
            return result;
        }
    }
}
