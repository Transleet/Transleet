#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Internal;


namespace Transleet.Models;


public class Entry
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Locale Locale { get; set; }

    public bool IsSource { get; set; }

    public bool IsSuggestion { get; set; }

    public Translation Translation { get; set; }
}
