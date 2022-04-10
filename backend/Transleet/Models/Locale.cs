using System.ComponentModel.DataAnnotations;

using Orleans;

namespace Transleet.Models;

public class Locale : IGrainWithStringKey
{
    [Key]
    public string? Id { get; set; }
}