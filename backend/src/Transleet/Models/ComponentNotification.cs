using Orleans.Concurrency;

namespace Transleet.Models
{
    [Immutable]
    public record ComponentNotification(Guid Id, Component? Item);
}
