using System.Security.Cryptography;
using System.Text;
using Orleans;
using Orleans.Streams;
using Transleet.Grains;

namespace Transleet
{
    public static class StreamProviderExtensions
    {
        public static IAsyncStream<TNotification> GetStream<TNotification, TGrain>(this IStreamProvider provider)
        {
            return provider.GetStream<TNotification>(typeof(TGrain).FullName);
        }
        public static IAsyncStream<TNotification> GetStream<TNotification>(this IStreamProvider provider,string grainType)
        {
            return provider.GetStream<TNotification>(new Guid(MD5.HashData(Encoding.UTF8.GetBytes(grainType))), grainType);
        }
    }
}
