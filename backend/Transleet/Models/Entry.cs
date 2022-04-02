#nullable enable
using System.ComponentModel.DataAnnotations;

namespace Transleet.Models;

public class Entry
{
    [Key]
    public string? Id { get; set; }

    public string Text { get; set; } = null!;
    public Locale Locale { get; set; } = null!;
}