#nullable enable
using System.ComponentModel.DataAnnotations;

namespace Transleet.Models;

public class Component
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<Guid>? Labels { get; set; }
    public List<Guid>? Translations { get; set; }
}
