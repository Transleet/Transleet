using System.ComponentModel.DataAnnotations;

namespace Transleet.Models;

public class Locale
{

    [Key]
    public Guid Id { get; set; }
}
