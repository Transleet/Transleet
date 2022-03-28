#nullable enable
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Transleet.Models;

public class Term
{
    [Key]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public Entry From { get; set; } = null!;
    public Entry To { get; set; } = null!;
    public List<Label> Labels { get; set; } = null!;
    public string? Description { get; set; }
    public List<Entry> Variants { get; set; } = null!;
}