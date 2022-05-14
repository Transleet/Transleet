#nullable enable

namespace Transleet.Models;

public class Term
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public ICollection<Label> Labels { get; set; }
    public string? Description { get; set; }
}
