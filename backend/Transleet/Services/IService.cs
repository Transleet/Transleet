#nullable enable
namespace Transleet.Services;

public interface IService<T>
{
    Task<List<T>> GetAsync();
    Task<T?> GetAsync(string id);
    Task CreateAsync(T obj);
    Task UpdateAsync(string id, T updated);
    Task RemoveAsync(string? id);
}