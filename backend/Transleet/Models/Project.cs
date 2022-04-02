#nullable enable
using System.ComponentModel.DataAnnotations;
using Orleans;

namespace Transleet.Models;

public class Project:IGrainWithStringKey
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid OwnerId { get; set; }

    public string? DisplayName { get; set; }
    public string Name { get; set; } = null!;
    public string? ProjectImage { get; set; }
    public string? Description { get; set; }
    public List<Term>? Terms { get; set; }
    public List<TranslationCollection>? TranslationCollections { get; set; } = null!;
    public short? Status { get; set; }
    public short? AccessLevel { get; set; }
    public bool? Hide { get; set; }
}