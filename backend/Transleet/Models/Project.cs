#nullable enable
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Transleet.Models;

public class Project
{
    [Key]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string Name { get; set; } = null!;
    public string? ProjectImage { get; set; }
    public string? Description { get; set; }
    public List<Term>? Terms { get; set; }
    public List<TranslationCollection> TranslationCollections { get; set; } = null!;
    public short Status { get; set; }
    public short AccessLevel { get; set; }
    public bool Hide { get; set; }
}