﻿#nullable enable
using System.ComponentModel.DataAnnotations;

using Orleans;

namespace Transleet.Models;

public class Entry : IGrainWithStringKey
{
    [Key]
    public string? Id { get; set; }

    public string Text { get; set; } = null!;
    public Locale Locale { get; set; } = null!;
}