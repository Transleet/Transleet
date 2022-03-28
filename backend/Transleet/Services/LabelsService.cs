#nullable enable
using MongoDB.Driver;
using MongoDbGenericRepository;
using Transleet.Models;

namespace Transleet.Services
{
    public class LabelsService : IService<Label>
    {
        private readonly IMongoCollection<Label> _collection;

        public LabelsService(IMongoDbContext context)
        {
            _collection = context.GetCollection<Label>("Labels");
        }

        public async Task<List<Label>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<Label?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Label obj) =>
            await _collection.InsertOneAsync(obj);

        public async Task UpdateAsync(string id, Label updated) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updated);

        public async Task RemoveAsync(string? id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
