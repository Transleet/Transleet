using Orleans;

using Transleet.Models;

namespace Transleet.Grains;

public interface IComponentGrain : IGrainWithGuidKey
{
    Task SetAsync(Component component);

    Task ClearAsync();

    Task<Component?> GetAsync();
}

public class ComponentGrain : IComponentGrain, IGrain
{
    public Task SetAsync(Component component)
    {
        throw new NotImplementedException();
    }

    public Task ClearAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Component?> GetAsync()
    {
        throw new NotImplementedException();
    }
}