#nullable enable
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Transleet.Models;

public class Component : IDocument<ObjectId>
{
    [Key]
    public ObjectId Id { get; set; }
    public int Version { get; set; }
    public ObjectId ProjectId { get; set; }
    public string? Name { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<Label>? Labels { get; set; }
    public List<Translation>? Translations { get; set; }
}
