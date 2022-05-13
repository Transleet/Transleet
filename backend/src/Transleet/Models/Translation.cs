namespace Transleet.Models;

public class Translation
{
    public Guid Id { get; set; }
    public Component Component { get; set; }
    public ICollection<Entry> Entries { get; set; }
}
