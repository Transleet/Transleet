#nullable enable
using System.ComponentModel.DataAnnotations;

using Orleans;

namespace Transleet.Models;

public class Translation : IGrainWithStringKey
{
    [Key]
    public string? Id { get; set; }
    public Entry From { get; set; } = null!;
    public Entry To { get; set; } = null!;
}