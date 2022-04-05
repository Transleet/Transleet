using System.ComponentModel.DataAnnotations;
using Orleans;

namespace Transleet.Models;

public record Project:IGrainWithStringKey
{
    public Guid Key { get; set; }
    public string? DisplayName { get; set; }
    public string Name { get; set; } = null!;
    public string? Avatar { get; set; }
    public string? Description { get; set; }
    public List<Term>? Terms { get; set; }
    public List<TranslationCollection>? TranslationCollections { get; set; } = null!;
    public short? Status { get; set; }
    public short? AccessLevel { get; set; }
    public bool? Hide { get; set; }
}