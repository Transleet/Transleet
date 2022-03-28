#nullable enable
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Transleet.Models;

public class Translation
{
    [Key]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public Entry From { get; set; } = null!;
    public Entry To { get; set; } = null!;
}