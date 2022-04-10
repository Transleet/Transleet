namespace Transleet.Grains
{
    public interface IGrainWithStreamId
    {
        Task<Guid> GetStreamId();
    }
}
