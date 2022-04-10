#nullable enable
using System.ComponentModel.DataAnnotations;

using Orleans;

namespace Transleet.Models;

public class Term : IGrainWithStringKey
{
    [Key]
    public string? Id { get; set; }

    public string Type { get; set; } = null!;
    public Entry From { get; set; } = null!;
    public Entry To { get; set; } = null!;
    public List<Guid> Labels { get; set; } = null!;
    public string? Description { get; set; }
    public List<Guid> Variants { get; set; } = null!;
}