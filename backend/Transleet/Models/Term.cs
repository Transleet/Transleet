#nullable enable
using System.ComponentModel.DataAnnotations;

namespace Transleet.Models;

public class Term
{
    [Key]
    public string? Id { get; set; }

    public Entry From { get; set; } = null!;
    public Entry To { get; set; } = null!;
    public List<Label> Labels { get; set; } = null!;
    public string? Description { get; set; }
    public List<Entry> Variants { get; set; } = null!;
}