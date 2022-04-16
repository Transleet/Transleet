using Microsoft.AspNetCore.Identity;
using Orleans;
using Orleans.Runtime;
using Transleet.Models;

namespace Transleet.Grains
{
    public interface IUserGrain :IIdentityUserGrain<User,Role>
    {

    }

    public class UserGrain : IdentityUserGrain<User,Role>,IUserGrain
    {
        public UserGrain(
            ILookupNormalizer normalizer,
            [PersistentState(nameof(IdentityUserGrainState<User, Role>), "Default")] IPersistentState<IdentityUserGrainState<User,Role>> data) : base(normalizer, data)
        {
        }
    }
}
