using Orleans.Concurrency;

namespace Transleet.Models
{
    [Immutable]
    public record TranslationNotification(Guid Id, Translation? Item);
}
