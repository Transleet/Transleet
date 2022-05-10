using System.ComponentModel.DataAnnotations;

namespace Transleet.Models;

public class Component
{
    [Key]
    public Guid Id { get; set; }
    public int Version { get; set; }
    public string? Name { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<Label>? Labels { get; set; }
    public List<Translation>? Translations { get; set; }
}
