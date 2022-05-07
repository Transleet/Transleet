using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using Transleet.MongoDbGenericRepository.Models;

namespace Transleet.Models;

public class Component : IDocument
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
