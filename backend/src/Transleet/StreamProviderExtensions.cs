using System.Security.Cryptography;
using System.Text;
using Orleans;
using Orleans.Streams;
using Transleet.Grains;

namespace Transleet
{
    public static class StreamProviderExtensions
    {
        public static IAsyncStream<TNotification> GetStream<TNotification>(this IStreamProvider provider)
        {
            return provider.GetStream<TNotification>(new Guid(MD5.HashData(Encoding.UTF8.GetBytes(typeof(TNotification).FullName))), typeof(TNotification).FullName);
        }
    }
}
