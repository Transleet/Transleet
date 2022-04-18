using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;
using Transleet.Models;

namespace Transleet.Grains
{
    public interface IUserGrain : IGrainWithGuidKey
    {
        Task AddClaims(IList<IdentityUserClaim<Guid>> claims);

        Task AddLogin(IdentityUserLogin<Guid> login);

        Task AddToken(IdentityUserToken<Guid> token);

        Task AddToRole(Guid roleId);

        [AlwaysInterleave]
        Task<bool> ContainsRole(Guid id);

        Task<IdentityResult> Create(User user);

        Task<IdentityResult> Delete();

        [AlwaysInterleave]
        Task<User> Get();

        Task<IList<IdentityUserClaim<Guid>>> GetClaims();

        [AlwaysInterleave]
        Task<IdentityUserLogin<Guid>> GetLogin(string loginProvider, string providerKey);

        [AlwaysInterleave]
        Task<IList<IdentityUserLogin<Guid>>> GetLogins();

        [AlwaysInterleave]
        Task<IList<string>> GetRoles();

        [AlwaysInterleave]
        Task<IdentityUserToken<Guid>> GetToken(string loginProvider, string name);

        Task RemoveClaims(IList<Claim> claims);

        Task RemoveLogin(string loginProvider, string providerKey);

        Task RemoveRole(Guid id, bool updateRoleGrain);

        Task RemoveToken(IdentityUserToken<Guid> token);

        Task ReplaceClaims(Claim claim, Claim newClaim);

        Task<IdentityResult> Update(User user);
    }

    public class UserGrain :
            Grain, IUserGrain
    {
        private readonly IPersistentState<UserGrainState> _data;
        private readonly IdentityErrorDescriber _errorDescriber = new();
        private readonly ILookupNormalizer _normalizer;

        public UserGrain(
            ILookupNormalizer normalizer,
            [PersistentState(nameof(UserGrainState), "Default")]
        IPersistentState<UserGrainState> data)
        {
            _data = data;
            _normalizer = normalizer;
        }

        private bool Exists => _data.State?.User != null;
        private static string GrainType => typeof(IUserGrain).FullName;
        private Guid GrainKey => this.GetPrimaryKey();

        public Task AddClaims(IList<IdentityUserClaim<Guid>> claims)
        {
            if (Exists && claims?.Count > 0)
            {
                var tasks = new List<Task>();
                foreach (var c in claims)
                {
                    // avoid duplicate claims
                    if (!_data.State.Claims.Any(x => x.ClaimType == c.ClaimType && x.ClaimValue == c.ClaimValue))
                    {
                        _data.State.Claims.Add(c);
                        tasks.Add(GrainFactory.GetGrain(c).AddUserId(GrainKey));
                    }
                }
                tasks.Add(_data.WriteStateAsync());
                return Task.WhenAll(tasks);
            }

            return Task.CompletedTask;
        }

        public async Task AddLogin(IdentityUserLogin<Guid> login)
        {
            if (Exists && login != null)
            {
                _data.State.Logins.Add(login);
                var lookup = GrainFactory.GetLookup<string>(GrainType, "Logins");
                await lookup.AddOrUpdateAsync(login.ProviderKey, GrainKey);
                await _data.WriteStateAsync();
            }
        }

        public Task AddToken(IdentityUserToken<Guid> token)
        {
            if (Exists && token != null)
            {
                _data.State.Tokens.Add(token);
                return _data.WriteStateAsync();
            }

            return Task.CompletedTask;
        }

        public Task AddToRole(Guid roleId)
        {
            if (Exists && _data.State.Roles.Add(roleId))
            {
                return Task.WhenAll(
                    GrainFactory.GetGrain<IRoleGrain<User, Role>>(roleId).AddUser(GrainKey),
                    _data.WriteStateAsync());
            }

            return Task.CompletedTask;
        }

        public Task<bool> ContainsRole(Guid id)
        {
            return Task.FromResult(Exists && _data.State.Roles.Contains(id));
        }

        public async Task<IdentityResult> Create(User user)
        {
            if (Exists)
                return IdentityResult.Failed(_errorDescriber.LoginAlreadyAssociated());
            if (string.IsNullOrEmpty(user.Email))
                return IdentityResult.Failed(_errorDescriber.InvalidEmail(user.Email));
            if (string.IsNullOrEmpty(user.UserName))
                return IdentityResult.Failed(_errorDescriber.InvalidUserName(user.UserName));

            // Make sure to set normalized username and email
            user.NormalizedEmail = _normalizer.NormalizeEmail(user.Email);
            user.NormalizedUserName = _normalizer.NormalizeName(user.UserName);

            var lookup = GrainFactory.GetLookup<string>( GrainType,"Emails");
            if (!await lookup.AddOrUpdateAsync(user.NormalizedEmail, GrainKey))
            {
                return IdentityResult.Failed(_errorDescriber.DuplicateEmail(user.Email));
            }

            var lookup1 = GrainFactory.GetLookup<string>(GrainType, "Usernames");
            if (!await lookup1.AddOrUpdateAsync(user.NormalizedUserName, GrainKey))
            {
                await lookup.DeleteIfMatchAsync(user.NormalizedEmail, GrainKey);
                return IdentityResult.Failed(_errorDescriber.DuplicateUserName(user.UserName));
            }

            _data.State.User = user;

            await _data.WriteStateAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> Delete()
        {
            if (_data.State.User == null)
                return IdentityResult.Failed(_errorDescriber.DefaultError());
            var lookup = GrainFactory.GetLookup<string>(GrainType, "Emails");
            var lookup1 = GrainFactory.GetLookup<string>(GrainType, "Usernames");
            await lookup.DeleteAsync(_data.State.User.NormalizedEmail);
            await lookup1.DeleteAsync(_data.State.User.NormalizedUserName);
            await Task.WhenAll(_data.State.Roles.Select(r => GrainFactory.GetGrain<IRoleGrain<User, Role>>(r).RemoveUser(GrainKey)));
            await _data.ClearStateAsync();

            return IdentityResult.Success;
        }

        public Task<User> Get()
        {
            return Task.FromResult(_data.State.User);
        }

        public Task<IList<IdentityUserClaim<Guid>>> GetClaims()
        {
            if (Exists)
            {
                return Task.FromResult<IList<IdentityUserClaim<Guid>>>(_data.State.Claims);
            }

            if (_data.State == null)
                throw new Exception();

            return Task.FromResult<IList<IdentityUserClaim<Guid>>>(null);
        }

        public Task<IdentityUserLogin<Guid>> GetLogin(string loginProvider, string providerKey)
        {
            if (Exists)
            {
                return Task.FromResult(_data.State.Logins.Find(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey));
            }
            return Task.FromResult<IdentityUserLogin<Guid>>(null);
        }

        public Task<IList<IdentityUserLogin<Guid>>> GetLogins()
        {
            if (Exists)
            {
                return Task.FromResult<IList<IdentityUserLogin<Guid>>>(_data.State.Logins);
            }
            return Task.FromResult<IList<IdentityUserLogin<Guid>>>(null);
        }

        public async Task<IList<string>> GetRoles()
        {
            if (Exists)
            {
                return (await Task.WhenAll(_data.State.Roles.Select(r => GrainFactory.GetGrain<IRoleGrain<User, Role>>(r).Get())))
                    .Select(r => r.Name)
                    .ToList();
            }

            return null;
        }

        public Task<IdentityUserToken<Guid>> GetToken(string loginProvider, string name)
        {
            if (Exists)
            {
                return Task.FromResult(_data.State.Tokens.Find(t => t.LoginProvider == loginProvider && t.Name == name));
            }

            return Task.FromResult<IdentityUserToken<Guid>>(null);
        }

        public Task RemoveClaims(IList<Claim> claims)
        {
            var writeRequired = false;
            var tasks = new List<Task>();
            foreach (var c in claims)
            {
                foreach (var m in _data.State.Claims.Where(uc => uc.ClaimValue == c.Value && uc.ClaimType == c.Type).ToList())
                {
                    writeRequired = true;
                    _data.State.Claims.Remove(m);
                    tasks.Add(GrainFactory.GetGrain(m).RemoveUserId(GrainKey));
                }
            }

            if (writeRequired)
                tasks.Add(_data.WriteStateAsync());

            return Task.WhenAll(tasks);
        }

        public async Task RemoveLogin(string loginProvider, string providerKey)
        {
            if (Exists)
            {
                var loginToRemove = _data.State.Logins.Find(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);
                if (loginToRemove != null)
                {
                    _data.State.Logins.Remove(loginToRemove);
                    var lookup = GrainFactory.GetLookup<string>(GrainType, "Logins");
                    await lookup.DeleteAsync(providerKey);
                    await _data.WriteStateAsync();
                }
            }
        }

        public async Task RemoveRole(Guid id, bool updateRoleGrain)
        {
            if (Exists && _data.State.Roles.Remove(id))
            {
                if (updateRoleGrain)
                {
                    await GrainFactory.GetGrain<IRoleGrain<User, Role>>(id).RemoveUser(GrainKey);
                }

                await _data.WriteStateAsync();
            }
        }

        public Task RemoveToken(IdentityUserToken<Guid> token)
        {
            if (Exists)
            {
                var tokensToRemove = _data.State.Tokens.Find(t => t.LoginProvider == token.LoginProvider && t.Name == token.Name);
                if (tokensToRemove != null)
                {
                    _data.State.Tokens.Remove(tokensToRemove);
                    return _data.WriteStateAsync();
                }
            }

            return Task.CompletedTask;
        }

        public Task ReplaceClaims(Claim claim, Claim newClaim)
        {
            var matchedClaims = _data.State.Claims
                .Where(uc => uc.UserId.Equals(GrainKey) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type);

            if (matchedClaims.Any())
            {
                var tasks = new List<Task>();
                foreach (var c in matchedClaims)
                {
                    tasks.Add(GrainFactory.GetGrain(c).RemoveUserId(GrainKey));
                    c.ClaimValue = newClaim.Value;
                    c.ClaimType = newClaim.Type;
                    tasks.Add(GrainFactory.GetGrain(c).AddUserId(GrainKey));
                }

                tasks.Add(_data.WriteStateAsync());

                return Task.WhenAll(tasks);
            }

            return Task.CompletedTask;
        }

        public async Task<IdentityResult> Update(User user)
        {
            if (_data.State.User == null)
                return IdentityResult.Failed(_errorDescriber.DefaultError());
            if (string.IsNullOrEmpty(user.Email))
                return IdentityResult.Failed(_errorDescriber.InvalidEmail(user.Email));
            if (string.IsNullOrEmpty(user.UserName))
                return IdentityResult.Failed(_errorDescriber.InvalidUserName(user.UserName));

            // Make sure to set normalized username and email
            var newEmail = _normalizer.NormalizeEmail(user.Email);
            var newUsername = _normalizer.NormalizeName(user.UserName);

            var lookup = GrainFactory.GetLookup<string>(GrainType, "Emails");
            if (newEmail != _data.State.User.NormalizedEmail && !await lookup.AddOrUpdateAsync(newEmail, GrainKey))
            {
                return IdentityResult.Failed(_errorDescriber.DuplicateEmail(newEmail));
            }

            var lookup1 = GrainFactory.GetLookup<string>(GrainType, "Usernames");
            if (newUsername != _data.State.User.NormalizedUserName && !await lookup1.AddOrUpdateAsync(newUsername, GrainKey))
            {
                // if email changed, then undo that change
                if (newEmail != _data.State.User.NormalizedEmail)
                {
                    await lookup.DeleteIfMatchAsync(user.NormalizedEmail, GrainKey);
                }

                return IdentityResult.Failed(_errorDescriber.DuplicateUserName(user.UserName));
            }

            // Remove old values
            if (newEmail != _data.State.User.NormalizedEmail)
            {
                await lookup.DeleteIfMatchAsync(_data.State.User.NormalizedEmail, GrainKey);
            }

            if (newUsername != _data.State.User.NormalizedUserName)
            {
                await lookup1.DeleteIfMatchAsync(_data.State.User.NormalizedUserName, GrainKey);
            }

            _data.State.User = user;

            await _data.WriteStateAsync();

            return IdentityResult.Success;
        }
    }

    public class UserGrainState
    {
        public List<IdentityUserClaim<Guid>> Claims { get; set; } = new();
        public List<IdentityUserLogin<Guid>> Logins { get; set; } = new();
        public HashSet<Guid> Roles { get; set; } = new HashSet<Guid>();
        public List<IdentityUserToken<Guid>> Tokens { get; set; } = new();
        public User User { get; set; }
    }
}
