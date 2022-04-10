using Orleans;
using Orleans.Streams;
using Transleet.Grains;

namespace Transleet
{
    public static class StreamProviderExtensions
    {
        public static IAsyncStream<TNotification> GetStream<TNotification, TGrain>(this IStreamProvider provider, TGrain grain) where TGrain : IGrainWithStreamId
        {
            return provider.GetStream<TNotification>(grain.GetStreamId().Result, nameof(TGrain));
        }
    }
}
