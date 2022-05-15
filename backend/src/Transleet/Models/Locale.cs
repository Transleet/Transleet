using System.ComponentModel.DataAnnotations;


namespace Transleet.Models;


public class Locale
{

    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string DisplayName { get; set; }
}
