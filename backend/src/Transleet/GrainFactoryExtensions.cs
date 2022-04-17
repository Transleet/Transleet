using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Orleans;
using Transleet.Grains;

namespace Transleet
{
    public static class GrainFactoryExtensions
    {
        public static ILookupGrain<TKey> GetLookup<TKey, TGrain>(this IGrainFactory factory, string propertyName) where TKey : notnull
        {
            return factory.GetLookup<TKey>(typeof(TGrain).FullName, propertyName);
        }
        public static ILookupGrain<TKey> GetLookup<TKey>(this IGrainFactory factory, string grainType, string propertyName)
        {
            return factory.GetGrain<ILookupGrain<TKey>>(new Guid(MD5.HashData(Encoding.UTF8.GetBytes(grainType + propertyName))));
        }
        public static IKeySetGrain GetKeySet<TGrain>(this IGrainFactory factory)
        {
            return factory.GetKeySet(typeof(TGrain).FullName);
        }
        public static IKeySetGrain GetKeySet(this IGrainFactory factory, string grainType)
        {
            return factory.GetGrain<IKeySetGrain>(new Guid(MD5.HashData(Encoding.UTF8.GetBytes(grainType))));
        }

        public static async Task<TGrain?> FindAsync<TKey, TGrain>(this IGrainFactory factory, string propertyName, TKey value) where TGrain : IGrain where TKey : notnull
        {
            var result = await factory.GetLookup<TKey, TGrain>(propertyName).FindAsync(value);

            if (result != null)
            {
                return (TGrain)factory.GetGrain(typeof(TGrain), result.Value);
            }

            return default;
        }
        internal static IIdentityClaimGrainInternal GetGrain(this IGrainFactory factory, Claim claim)
        {
            return factory.GetGrain<IIdentityClaimGrainInternal>($"{claim.Type}-{claim.Value}");
        }

        internal static IIdentityClaimGrainInternal GetGrain(this IGrainFactory factory, IdentityUserClaim<Guid> claim)
        {
            return factory.GetGrain<IIdentityClaimGrainInternal>($"{claim.ClaimType}-{claim.ClaimValue}");
        }
    }
}
