﻿using System.ComponentModel.DataAnnotations;

namespace Transleet.Models;

public class Component
{
    [Key]
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public ICollection<Label>? Labels { get; set; }
    public ICollection<Translation>? Translations { get; set; }
}
