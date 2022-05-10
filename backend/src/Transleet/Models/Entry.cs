#nullable enable

using System.ComponentModel.DataAnnotations.Schema;

namespace Transleet.Models;

public class Entry
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Locale Locale { get; set; }
}
