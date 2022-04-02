#nullable enable
using System.ComponentModel.DataAnnotations;

namespace Transleet.Models;

public class Translation
{
    [Key]
    public string? Id { get; set; }
    public Entry From { get; set; } = null!;
    public Entry To { get; set; } = null!;
}