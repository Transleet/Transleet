using Orleans;
using Transleet.Models;

namespace Transleet.Grains
{
    public interface IProjectGrain:IGrainWithGuidKey
    {
        Task SetAsync(Project item);

        Task ClearAsync();

        Task<Project?> GetAsync();
    }
}
