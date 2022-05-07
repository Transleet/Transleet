using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using Transleet.AspNetCore.Identity.MongoDbCore.Models;
using Transleet.MongoDbGenericRepository.Models;

namespace Transleet.Models;

public class User : MongoIdentityUser
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
