using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Annotations;
using Transleet.MongoDbGenericRepository.Models;

namespace Transleet.Models;

public class Project : IDocument
{
    [Key]
    public ObjectId Id { get; set; }
    public int Version { get; set; }
    public string? DisplayName { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<Term>? Terms { get; set; }
    public List<Component>? Components { get; set; }
    public short? Status { get; set; }
    public short? AccessLevel { get; set; }
    public bool? Hide { get; set; }
}
