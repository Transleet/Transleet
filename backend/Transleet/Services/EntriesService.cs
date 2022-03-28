#nullable enable
using MongoDB.Driver;
using MongoDbGenericRepository;
using Transleet.Models;

namespace Transleet.Services;


public class EntriesService:IService<Entry>
{
    private readonly IMongoCollection<Entry> _collection;

    public EntriesService(IMongoDbContext context)
    {
        _collection = context.GetCollection<Entry>("Entries");
    }
        
    public async Task<List<Entry>> GetAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<Entry?> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Entry obj) =>
        await _collection.InsertOneAsync(obj);

    public async Task UpdateAsync(string id, Entry updated) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, updated);

    public async Task RemoveAsync(string? id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}