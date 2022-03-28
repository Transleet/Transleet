#nullable enable
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Transleet.Models;

public class TranslationCollection
{
    [Key]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string Path { get; set; } = null!;
    public List<Label> Labels { get; set; } = null!;
    public List<Translation> Translations { get; set; } = null!;
}