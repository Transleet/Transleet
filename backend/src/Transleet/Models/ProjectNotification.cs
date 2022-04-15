using Orleans;
using Orleans.Concurrency;

namespace Transleet.Models
{
    [Immutable]
    public record ProjectNotification(Guid Key, Project? Item);
}
