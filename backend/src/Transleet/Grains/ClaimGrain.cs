using System.Security.Cryptography;
using System.Text;
using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace Transleet.Grains
{
    public interface IClaimGrain : IGrainWithStringKey
    {
        [AlwaysInterleave]
        Task<IList<Guid>> GetUserIds();
    }

    internal interface IClaimGrainInternal : IClaimGrain
    {
        Task AddUserId(Guid id);

        Task RemoveUserId(Guid id);
    }

    internal class ClaimGrain : Grain, IClaimGrainInternal
    {
        private readonly IPersistentState<ClaimGrainState> _data;


        public ClaimGrain(
                    [PersistentState(nameof(ClaimGrainState), "Default")] IPersistentState<ClaimGrainState> data)
        {
            _data = data;
        }

        public Task AddUserId(Guid id)
        {
            if (_data.State.Users.Add(id))
                return _data.WriteStateAsync();

            return Task.CompletedTask;
        }

        public Task<IList<Guid>> GetUserIds()
        {
            return Task.FromResult<IList<Guid>>(_data.State.Users.ToList());
        }

        public Task RemoveUserId(Guid id)
        {
            if (_data.State.Users.Remove(id))
                return _data.WriteStateAsync();

            return Task.CompletedTask;
        }

        public class ClaimGrainState
        {
            public HashSet<Guid> Users { get; set; } = new();
        }
    }
}
