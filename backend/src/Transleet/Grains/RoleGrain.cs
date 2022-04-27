﻿using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace Transleet.Grains
{
    public interface IRoleGrain<TUser, TRole> : IGrainWithGuidKey
        where TUser : IdentityUser<Guid>
        where TRole : IdentityRole<Guid>
    {
        Task AddClaim(IdentityRoleClaim<Guid> claim);

        Task AddUser(Guid id);

        Task<IdentityResult> Create(TRole role);

        Task<IdentityResult> Delete();

        [AlwaysInterleave]
        Task<TRole> Get();

        [AlwaysInterleave]
        Task<IList<IdentityRoleClaim<Guid>>> GetClaims();

        [AlwaysInterleave]
        Task<IList<Guid>> GetUsers();

        Task RemoveClaim(Claim claim);

        Task RemoveUser(Guid id);

        Task<IdentityResult> Update(TRole role);
    }

    internal class RoleGrain<TUser, TRole> : Grain, IRoleGrain<TUser, TRole>
        where TUser : IdentityUser<Guid>
        where TRole : IdentityRole<Guid>
    {
        private readonly IPersistentState<RoleGrainState<TRole>> _data;
        private readonly ILookupNormalizer _normalizer;

        public RoleGrain(
            ILookupNormalizer normalizer,
            [PersistentState("IdentityRole", "Default")] IPersistentState<RoleGrainState<TRole>> data)
        {
            _data = data;
            _normalizer = normalizer;
        }

        private bool Exists => _data.State?.Role != null;

        private static string GrainType => typeof(IRoleGrain<TUser, TRole>).FullName;
        private Guid GrainKey => this.GetPrimaryKey();
        public Task<Guid> GetIndexIdForProperty(string propertyName)
        {
            return Task.FromResult(propertyName switch
            {
                "Roles" => new Guid(
                    MD5.HashData(Encoding.UTF8.GetBytes(GrainType + "Roles"))),
                _ => new Guid()
            });
        }

        public Task AddClaim(IdentityRoleClaim<Guid> claim)
        {
            if (Exists && claim != null)
            {
                _data.State.Claims.Add(claim);
                return _data.WriteStateAsync();
            }

            return Task.CompletedTask;
        }

        public Task AddUser(Guid id)
        {
            if (Exists && _data.State.Users.Add(id))
                return _data.WriteStateAsync();

            return Task.CompletedTask;
        }

        public async Task<IdentityResult> Create(TRole role)
        {
            if (Exists || string.IsNullOrEmpty(role.Name))
            {
                return IdentityResult.Failed();
            }

            // Normalize name
            role.NormalizedName = _normalizer.NormalizeName(role.Name);

            var lookup = GrainFactory.GetLookup<string>(GrainType, "Roles");
            if (!await lookup.AddOrUpdateAsync(role.NormalizedName, GrainKey))
                return IdentityResult.Failed();

            _data.State.Role = role;
            await _data.WriteStateAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> Delete()
        {
            if (_data.State.Role == null)
                return IdentityResult.Failed();
            var lookup = GrainFactory.GetLookup<string>(GrainType,"Roles");
            await lookup.DeleteAsync(_data.State.Role.NormalizedName);
            await Task.WhenAll(_data.State.Users.Select(u => GrainFactory.GetGrain<IUserGrain>(u).RemoveRole(GrainKey, false)));
            await _data.ClearStateAsync();

            return IdentityResult.Success;
        }

        public Task<TRole> Get()
        {
            return Task.FromResult(_data.State.Role);
        }

        public Task<IList<IdentityRoleClaim<Guid>>> GetClaims()
        {
            if (Exists)
            {
                return Task.FromResult<IList<IdentityRoleClaim<Guid>>>(_data.State.Claims);
            }

            return Task.FromResult<IList<IdentityRoleClaim<Guid>>>(null);
        }

        public Task<IList<Guid>> GetUsers()
        {
            return Task.FromResult<IList<Guid>>(_data.State.Users.ToList());
        }

        public Task RemoveClaim(Claim claim)
        {
            if (Exists)
            {
                var writeRequired = false;
                foreach (var m in _data.State.Claims.Where(rc => rc.ClaimValue == claim.Value && rc.ClaimType == claim.Type))
                {
                    writeRequired = true;
                    _data.State.Claims.Remove(m);
                }

                if (writeRequired)
                    return _data.WriteStateAsync();
            }

            return Task.CompletedTask;
        }

        public Task RemoveUser(Guid id)
        {
            if (_data.State.Users.Remove(id))
                return _data.WriteStateAsync();

            return Task.CompletedTask;
        }

        public async Task<IdentityResult> Update(TRole role)
        {
            if (!Exists || string.IsNullOrEmpty(role.Name))
                return IdentityResult.Failed();

            // Normalize name
            var newRoleName = _normalizer.NormalizeName(role.Name);

            var lookup = GrainFactory.GetLookup<string>(GrainType,"Roles");
            if (newRoleName != _data.State.Role.NormalizedName && !await lookup.AddOrUpdateAsync(newRoleName, GrainKey))
            {
                return IdentityResult.Failed();
            }

            _data.State.Role = role;
            await _data.WriteStateAsync();

            return IdentityResult.Success;
        }
    }

    internal class RoleGrainState<TRole>
    {
        public List<IdentityRoleClaim<Guid>> Claims { get; set; } = new List<IdentityRoleClaim<Guid>>();
        public TRole Role { get; set; }
        public HashSet<Guid> Users { get; set; } = new HashSet<Guid>();
    }
}