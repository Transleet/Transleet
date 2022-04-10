namespace Transleet.Grains
{
    public interface IGrainWithKeySetId
    {
        Task<Guid> GetKeySetId();
    }
}
