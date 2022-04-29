using System.ComponentModel.DataAnnotations;

using Orleans;

namespace Transleet.Models;

public class Locale : IGrainWithStringKey
{
    public Locale(string? id)
    {
        Id = id;
    }

    [Key] private string? Id { get; }

    public override bool Equals(object? obj)
    {
        if (obj is not Locale l)
        {
            return false;
        }
        return l.Id == Id;
    }

    protected bool Equals(Locale other) => Id == other.Id;

    public override int GetHashCode() => (Id != null ? Id.GetHashCode() : 0);
}
