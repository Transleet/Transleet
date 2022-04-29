﻿using System.ComponentModel.DataAnnotations;

using Orleans;

namespace Transleet.Models;

public record Project : IGrainWithStringKey
{
    public Guid Id { get; set; }
    public string? DisplayName { get; set; }
    public string Name { get; set; }
    public string? Avatar { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<Guid>? Terms { get; set; }
    public HashSet<Guid>? Components { get; set; }
    public short? Status { get; set; }
    public short? AccessLevel { get; set; }
    public bool? Hide { get; set; }
}
