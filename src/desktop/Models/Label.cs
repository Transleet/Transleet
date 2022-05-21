using System;

namespace Transleet.Desktop.Models;


public class Label
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string? Description { get; set; }
}
