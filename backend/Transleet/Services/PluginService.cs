namespace Transleet.Services;

public class PluginService : IService<IPlugin>
{
    public Task<List<IPlugin>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IPlugin?> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(IPlugin obj)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(string id, IPlugin updated)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string? id)
    {
        throw new NotImplementedException();
    }
}