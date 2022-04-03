using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Orleans;
using Transleet.IdentityStore.Grains;

namespace Transleet.IdentityStore
{
    public static class GrainFactoryExtensions
    {
        private const int BucketCount = 2459;
        
        public static IIdentityRoleGrain<TUser, TRole> Role<TUser, TRole>(this IGrainFactory factory, Guid id)
            where TUser : IdentityUser<Guid>
            where TRole : IdentityRole<Guid>
        {
            return factory.GetGrain<IIdentityRoleGrain<TUser, TRole>>(id);
        }

        public static IIdentityRoleGrain<TUser, IdentityRole<Guid>> Role<TUser>(this IGrainFactory factory, Guid id)
            where TUser : IdentityUser<Guid>
        {
            return factory.GetGrain<IIdentityRoleGrain<TUser, IdentityRole<Guid>>>(id);
        }

        public static IIdentityRoleGrain<IdentityUser<Guid>, IdentityRole<Guid>> Role(this IGrainFactory factory, Guid id)
        {
            return factory.GetGrain<IIdentityRoleGrain<IdentityUser<Guid>, IdentityRole<Guid>>>(id);
        }

        public static IAsyncEnumerable<IIdentityUserGrain<TUser, IdentityRole<Guid>>> SearchByUsername<TUser>(this IGrainFactory factory, Func<string, bool> func)
            where TUser : IdentityUser<Guid>
        {
            return factory.Search<IIdentityUserGrain<TUser, IdentityRole<Guid>>>(OrleansIdentityConstants.UsernameLookup, func);
        }

        public static IIdentityUserGrain<IdentityUser<Guid>, IdentityRole<Guid>> User(this IGrainFactory factory, Guid id)
        {
            return factory.GetGrain<IIdentityUserGrain<IdentityUser<Guid>, IdentityRole<Guid>>>(id);
        }

        public static IIdentityUserGrain<TUser, IdentityRole<Guid>> User<TUser>(this IGrainFactory factory, Guid id)
            where TUser : IdentityUser<Guid>
        {
            return factory.GetGrain<IIdentityUserGrain<TUser, IdentityRole<Guid>>>(id);
        }

        public static IIdentityUserGrain<TUser, TRole> User<TUser, TRole>(this IGrainFactory factory, Guid id)
            where TUser : IdentityUser<Guid>
            where TRole : IdentityRole<Guid>
        {
            return factory.GetGrain<IIdentityUserGrain<TUser, TRole>>(id);
        }

        public static Task<IIdentityUserGrain<TUser, TRole>> UserByEmail<TUser, TRole>(this IGrainFactory factory, string email, ILookupNormalizer normalizer = null)
            where TUser : IdentityUser<Guid>
            where TRole : IdentityRole<Guid>
        {
            return factory.Find<IIdentityUserGrain<TUser, TRole>>(OrleansIdentityConstants.EmailLookup, normalizer?.NormalizeEmail(email) ?? email.ToUpperInvariant());
        }

        public static Task<IIdentityUserGrain<TUser, IdentityRole<Guid>>> UserByEmail<TUser>(this IGrainFactory factory, string email, ILookupNormalizer normalizer = null)
            where TUser : IdentityUser<Guid>
        {
            return factory.Find<IIdentityUserGrain<TUser, IdentityRole<Guid>>>(OrleansIdentityConstants.EmailLookup, normalizer?.NormalizeEmail(email) ?? email.ToUpperInvariant());
        }

        public static Task<IIdentityUserGrain<IdentityUser<Guid>, IdentityRole<Guid>>> UserByEmail(this IGrainFactory factory, string email, ILookupNormalizer normalizer = null)
        {
            return factory.Find<IIdentityUserGrain<IdentityUser<Guid>, IdentityRole<Guid>>>(OrleansIdentityConstants.EmailLookup, normalizer?.NormalizeEmail(email) ?? email.ToUpperInvariant());
        }

        public static Task<IIdentityUserGrain<TUser, TRole>> UserByUsername<TUser, TRole>(this IGrainFactory factory, string userName, ILookupNormalizer normalizer = null)
            where TUser : IdentityUser<Guid>
            where TRole : IdentityRole<Guid>
        {
            return factory.Find<IIdentityUserGrain<TUser, TRole>>(OrleansIdentityConstants.UsernameLookup, normalizer?.NormalizeEmail(userName) ?? userName.ToUpperInvariant());
        }

        public static Task<IIdentityUserGrain<IdentityUser<Guid>, IdentityRole<Guid>>> UserByUsername(this IGrainFactory factory, string userName, ILookupNormalizer normalizer = null)
        {
            return factory.Find<IIdentityUserGrain<IdentityUser<Guid>, IdentityRole<Guid>>>(OrleansIdentityConstants.UsernameLookup, normalizer?.NormalizeEmail(userName) ?? userName.ToUpperInvariant());
        }

        public static Task<IIdentityUserGrain<TUser, IdentityRole<Guid>>> UserByUsername<TUser>(this IGrainFactory factory, string userName, ILookupNormalizer normalizer = null)
            where TUser : IdentityUser<Guid>
        {
            return factory.Find<IIdentityUserGrain<TUser, IdentityRole<Guid>>>(OrleansIdentityConstants.UsernameLookup, normalizer?.NormalizeEmail(userName) ?? userName.ToUpperInvariant());
        }


        public static Task<bool> AddOrUpdateToLookup(this IGrainFactory factory, string lookupName, string value, Guid grainKey)
        {
            return factory.GetGrain<ILookupGrain>($"{lookupName}/{GetBucket(value)}").AddOrUpdate(value, grainKey);
        }

        public static Task RemoveFromLookup(this IGrainFactory factory, string lookupName, string value)
        {
            return factory.GetGrain<ILookupGrain>($"{lookupName}/{GetBucket(value)}").Delete(value);
        }

        public static Task SafeRemoveFromLookup(this IGrainFactory factory, string lookupName, string value, Guid grainKey)
        {
            return factory.GetGrain<ILookupGrain>($"{lookupName}/{GetBucket(value)}").DeleteIfMatch(value, grainKey);
        }

        public static async Task<TGrain> Find<TGrain>(this IGrainFactory factory, string lookupName, string value) where TGrain : IGrain
        {
            var result = await factory.GetGrain<ILookupGrain>($"{lookupName}/{GetBucket(value)}").Find(value);

            if (result != null)
            {
                return (TGrain)factory.GetGrain(typeof(TGrain), result.Value);
            }

            return default;
        }

        public static async IAsyncEnumerable<TGrain> Search<TGrain>(this IGrainFactory factory, string lookupName, Func<string, bool> func) where TGrain : IGrain
        {
            for (var i = 0; i < BucketCount; i++)
            {
                var bucket = await factory.GetGrain<ILookupGrain>($"{lookupName}/{i}").GetAll();
                if (bucket != null)
                {
                    foreach (var e in bucket)
                    {
                        if (func(e.Key))
                        {
                            yield return (TGrain)factory.GetGrain(typeof(TGrain), e.Value);
                        }
                    }
                }
            }
        }

        internal static IIdentityClaimGrainInternal GetGrain(this IGrainFactory factory, Claim claim)
        {
            return factory.GetGrain<IIdentityClaimGrainInternal>($"{claim.Type}-{claim.Value}");
        }

        internal static IIdentityClaimGrainInternal GetGrain(this IGrainFactory factory, IdentityUserClaim<Guid> claim)
        {
            return factory.GetGrain<IIdentityClaimGrainInternal>($"{claim.ClaimType}-{claim.ClaimValue}");
        }

        internal static int GetBucket(string text)
        {
            unchecked
            {
                int hash = 113647;
                foreach (char c in text)
                {
                    hash = (hash * 31) + c;
                }

                return hash & BucketCount;
            }
        }
    }
}
