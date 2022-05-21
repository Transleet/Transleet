using System;
using System.Collections.Generic;

namespace Transleet.Desktop.Models;


public class Component
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public ICollection<Label>? Labels { get; set; }
    public ICollection<Translation>? Translations { get; set; }
}
