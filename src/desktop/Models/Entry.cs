#nullable enable

using System;

namespace Transleet.Desktop.Models;


public class Entry
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Locale Locale { get; set; }

    public bool IsSource { get; set; }

    public bool IsSuggestion { get; set; }

    public Translation Translation { get; set; }
}
