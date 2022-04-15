using Orleans.Concurrency;

namespace Transleet.Models
{
    [Immutable]
    public record ComponentNotification(Guid Key, Component? Item);
}
