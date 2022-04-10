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
        public static ILookupGrain GetLookup<TGrain>(this IGrainFactory factory, string propertyName)
        {
            return factory.GetLookup(typeof(TGrain).FullName, propertyName);
        }
        public static ILookupGrain GetLookup(this IGrainFactory factory, string grainType, string propertyName)
        {
            return factory.GetGrain<ILookupGrain>(new Guid(MD5.HashData(Encoding.UTF8.GetBytes(grainType + propertyName))));
        }
        public static IKeySetGrain GetKeySet<TGrain>(this IGrainFactory factory)
        {
            return factory.GetKeySet(typeof(TGrain).FullName);
        }
        public static IKeySetGrain GetKeySet(this IGrainFactory factory, string grainType)
        {
            return factory.GetGrain<IKeySetGrain>(new Guid(MD5.HashData(Encoding.UTF8.GetBytes(grainType))));
        }

        public static async Task<TGrain?> FindAsync<TGrain>(this IGrainFactory factory, string propertyName, string value) where TGrain : IGrain
        {
            var result = await factory.GetLookup<TGrain>(propertyName).FindAsync(value);

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
