using System.Collections.Immutable;
using Orleans;

namespace Transleet.Grains
{
    public interface IProjectManagerGrain :  IGrainWithGuidKey

    {
        Task RegisterAsync(Guid itemKey);
        Task UnregisterAsync(Guid itemKey);

        Task<ImmutableArray<Guid>> GetAllAsync();
    }
}
