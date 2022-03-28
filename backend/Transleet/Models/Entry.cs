#nullable enable
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Transleet.Models;

public class Entry
{
    [Key]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Text { get; set; } = null!;
    public Locale Locale { get; set; } = null!;
}