#nullable enable
using System.ComponentModel.DataAnnotations;

namespace Transleet.Models;

public class TranslationCollection
{
    [Key]
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string Type { get; set; } = null!;
    public string Path { get; set; } = null!;
    public List<Label> Labels { get; set; } = null!;
    public List<Translation> Translations { get; set; } = null!;
}