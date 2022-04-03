using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Orleans;
using Transleet.Grains;

namespace Transleet
{
    public static class GrainFactoryExtensions
    {
        public static Task RemoveFromLookup(this IGrainFactory factory, Guid lookupName, string value)
        {
            return factory.GetGrain<ILookupGrain>(lookupName).DeleteAsync(value);
        }

        public static Task SafeRemoveFromLookup(this IGrainFactory factory, Guid lookupName, string value, Guid grainKey)
        {
            return factory.GetGrain<ILookupGrain>(lookupName).DeleteIfMatchAsync(value, grainKey);
        }

        public static async Task<TGrain?> Find<TGrain>(this IGrainFactory factory, Guid lookupName, string value) where TGrain : IGrain
        {
            var result = await factory.GetGrain<ILookupGrain>(lookupName).FindAsync(value);

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
