using System.ComponentModel.DataAnnotations.Schema;


namespace Transleet.Models;


public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Project>? Projects { get; set; }
    public List<User> Users { get; set; }
}
