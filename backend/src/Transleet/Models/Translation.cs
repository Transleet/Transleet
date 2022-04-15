#nullable enable
using System.ComponentModel.DataAnnotations;

using Orleans;

namespace Transleet.Models;

public class Translation
{
    public Guid Key { get; set; }
    public Entry From { get; set; } = null!;
    public Entry To { get; set; } = null!;
}
