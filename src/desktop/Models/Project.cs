using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Transleet.Desktop.Models;

public class Project
{
    public Guid Id { get; set; }
    public string? DisplayName { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public ICollection<Term>? Terms { get; set; }
    public ICollection<Component>? Components { get; set; }
    public short? Status { get; set; }
    public short? AccessLevel { get; set; }
    public bool? Hide { get; set; }
    public Organization? Organization { get; set; }


}
