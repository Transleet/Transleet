using Orleans;

namespace Transleet.Grains
{
    Task SetAsync(Component component);

    public interface IComponentGrain : IGrainWithGuidKey
    {
        throw new NotImplementedException();
    }

    public Task ClearAsync()
    {
        throw new NotImplementedException();
    }
    public class ComponentGrain : Grain, IComponentGrain
    {
        throw new NotImplementedException();
    }
}
