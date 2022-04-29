using Meilisearch;

using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;
using Orleans.Streams;

using Transleet.Grains;
using Transleet.Models;

namespace Transleet
{
    interface ISearchDataUpdateGrain : IGrainWithGuidKey
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }

    [StatelessWorker(1)]
    public class SearchDataUpdateGrain : Grain, ISearchDataUpdateGrain
    {
        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var stream = GetStreamProvider("SMS").GetStream<ProjectNotification>();
            MeilisearchClient client = new("http://search:7700", "masterKey");
            await stream.SubscribeAsync(onNextAsync: async list =>
            {
                var index = client.Index("projects");
                await index.AddDocumentsAsync(list.Select(_ => _.Item), cancellationToken: stoppingToken);
            });
        }
    }

    public class SearchDataUpdateService : BackgroundService
    {
        private readonly IGrainFactory _grainFactory;
        private readonly IConfiguration _configuration;

        public SearchDataUpdateService(IGrainFactory grainFactory, IConfiguration configuration)
        {
            _grainFactory = grainFactory;
            _configuration = configuration;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_configuration.GetValue<bool>("EnableSearchService"))
            {
                var grain = _grainFactory.GetGrain<ISearchDataUpdateGrain>(Guid.Empty);
                return grain.ExecuteAsync(stoppingToken);
            }
            return Task.CompletedTask;
        }
    }
}
