namespace Transleet.Grains
{
    public interface IGrainWithIndexId
    {
        Task<Guid> GetIndexIdForProperty(string propertyName);
    }
}
