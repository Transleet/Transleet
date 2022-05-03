#nullable enable

namespace Transleet.Models;

public class Entry
{
    public Guid Key { get; set; }

    public string Text { get; set; } = null!;
    public Locale Locale { get; set; } = null!;
}
