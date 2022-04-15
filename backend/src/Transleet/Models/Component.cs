#nullable enable
using System.ComponentModel.DataAnnotations;

namespace Transleet.Models;

public class Component
{
    public Guid Key { get; set; }
    public string? Name { get; set; }
    public string Type { get; set; } = null!;
    public string Path { get; set; } = null!;
    public List<Guid> Labels { get; set; } = null!;
    public List<Guid> Translations { get; set; } = null!;
}
