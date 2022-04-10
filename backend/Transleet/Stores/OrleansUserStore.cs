using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Orleans;
using Transleet.Grains;

namespace Transleet.Stores
{
    public class OrleansUserStore<TUser, TRole> :
        UserStoreBase<TUser, TRole, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, IdentityRoleClaim<Guid>>
        where TUser : IdentityUser<Guid>
        where TRole : IdentityRole<Guid>
    {
        private const string ValueCannotBeNullOrEmpty = "Value cannot be null or empty";
        private readonly IClusterClient _client;
        private readonly IRoleClaimStore<TRole> _roleStore;
        
        public OrleansUserStore(IClusterClient client, IRoleClaimStore<TRole> roleStore) : base(new IdentityErrorDescriber())
        {
            _client = client;
            _roleStore = roleStore;
        }
        
        public override IQueryable<TUser> Users => throw new NotSupportedException();
        
        public override Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            if (claims.Any())
            {
                return UserGrain(user.Id).AddClaims(claims.Select(c => CreateUserClaim(user, c)).ToList());
            }

            return Task.CompletedTask;
        }
        
        public override Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            return UserGrain(user.Id).AddLogin(CreateUserLogin(user, login));
        }
        
        public override async Task AddToRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(ValueCannotBeNullOrEmpty, nameof(normalizedRoleName));
            }

            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (role != null)
            {
                await UserGrain(user.Id).AddToRole(role.Id);
            }
        }
        
        public override Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.Id == default)
            {
                user.Id = Guid.NewGuid();
            }

            return UserGrain(user.Id).Create(user);
        }

        public override Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return UserGrain(user.Id).Delete();
        }

        
        public override async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var grain = await _client.FindAsync<IIdentityUserGrain<TUser, TRole>>("Emails", normalizedEmail);
            if (grain != null)
            {
                return await grain.Get();
            }

            return null;
        }

        public override Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            return UserGrain(ConvertIdFromString(userId)).Get();
        }
        public override async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var grain = await _client.FindAsync<IIdentityUserGrain<TUser, TRole>>("Usernames", normalizedUserName);
            if (grain != null)
            {
                return await grain.Get();
            }

            return null;
        }
        
        public override async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return (await UserGrain(user.Id).GetClaims())?
                .Select(c => c.ToClaim())
                .ToList() ?? new List<Claim>();
        }

        public override async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return (await UserGrain(user.Id).GetLogins())?
                .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName))
                .ToList() ?? new List<UserLoginInfo>();
        }

        public override async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return (await UserGrain(user.Id).GetRoles())?.ToList() ?? new List<string>();
        }

        public override async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var ids = await _client.GetGrain(claim).GetUserIds();

            return (await Task.WhenAll(ids.Select(i => UserGrain(i).Get()))).ToList();
        }

        public override async Task<IList<TUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrEmpty(normalizedRoleName))
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }

            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (role != null)
            {
                var users = await _client.GetGrain<IIdentityRoleGrain<TUser, TRole>>(role.Id).GetUsers();
                return (await Task.WhenAll(users.Select(u => UserGrain(u).Get()))).ToList();
            }

            return new List<TUser>();
        }
        
        public override async Task<bool> IsInRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(ValueCannotBeNullOrEmpty, nameof(normalizedRoleName));
            }
            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (role != null)
            {
                return await UserGrain(user.Id).ContainsRole(role.Id);
            }

            return false;
        }

        public override Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            if (claims.Any())
            {
                return UserGrain(user.Id).RemoveClaims(claims.ToList());
            }

            return Task.CompletedTask;
        }

        public override async Task RemoveFromRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(ValueCannotBeNullOrEmpty, nameof(normalizedRoleName));
            }

            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (role != null)
            {
                await UserGrain(user.Id).RemoveRole(role.Id, true);
            }
        }

        public override Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return UserGrain(user.Id).RemoveLogin(loginProvider, providerKey);
        }

        public override Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            if (newClaim == null)
            {
                throw new ArgumentNullException(nameof(newClaim));
            }

            return UserGrain(user.Id).ReplaceClaims(claim, newClaim);
        }

        public override Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.Id == default)
            {
                throw new ArgumentException("Key cannot be default", nameof(user.Id));
            }

            return UserGrain(user.Id).Update(user);
        }

        protected override Task AddUserTokenAsync(IdentityUserToken<Guid> token)
        {
            ThrowIfDisposed();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return UserGrain(token.UserId).AddToken(token);
        }

        protected override Task<TRole> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            return _roleStore.FindByNameAsync(normalizedRoleName, cancellationToken);
        }

        protected override Task<IdentityUserToken<Guid>> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            return UserGrain(user.Id).GetToken(loginProvider, name);
        }

        protected override Task<TUser> FindUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return UserGrain(userId).Get();
        }

        protected override async Task<IdentityUserLogin<Guid>> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var grain = await _client.FindAsync<IIdentityUserGrain<TUser, TRole>>("Logins", providerKey);
            if (grain != null)
            {
                return await grain.GetLogin(loginProvider, providerKey);
            }

            return null;
        }

        protected override Task<IdentityUserLogin<Guid>?> FindUserLoginAsync(Guid userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return UserGrain(userId).GetLogin(loginProvider, providerKey);
        }

        protected override async Task<IdentityUserRole<Guid>?> FindUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            if (await UserGrain(userId).ContainsRole(roleId))
            {
                return new IdentityUserRole<Guid>
                {
                    RoleId = roleId,
                    UserId = userId
                };
            }
            return null;
        }

        protected override Task RemoveUserTokenAsync(IdentityUserToken<Guid> token)
        {
            return UserGrain(token.UserId).RemoveToken(token);
        }

        private IIdentityUserGrain<TUser, TRole> UserGrain(Guid id)
        {
            return _client.GetGrain<IIdentityUserGrain<TUser, TRole>>(id);
        }
    }
}
