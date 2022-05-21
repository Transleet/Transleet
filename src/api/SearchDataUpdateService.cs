using Meilisearch;

namespace Transleet;

public class SearchDataUpdateService : BackgroundService
{
    private readonly IConfiguration _configuration;

    public SearchDataUpdateService(IConfiguration configuration) => _configuration = configuration;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_configuration.GetValue<bool>("EnableSearchService"))
        {
            MeilisearchClient client = new("http://search:7700", "masterKey");
            //await stream.SubscribeAsync(onNextAsync: async list =>
            //{
            //    var index = client.Index("projects");
            //    await index.AddDocumentsAsync(list.Select(_ => _.Item), cancellationToken: stoppingToken);
            //});
        }
        return Task.CompletedTask;
    }
}
