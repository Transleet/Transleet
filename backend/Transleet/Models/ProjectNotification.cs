using Orleans;
using Orleans.Concurrency;

namespace Transleet.Models
{
    [Immutable]
    public record ProjectNotification(Guid Id, Project? Item);
}
