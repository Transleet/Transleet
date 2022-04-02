using System.ComponentModel.DataAnnotations;

namespace Transleet.Models;

public class Label
{
    [Key]
    public string? Id { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string? Description { get; set; }
}