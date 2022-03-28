#nullable enable
using MongoDB.Driver;
using MongoDbGenericRepository;
using Transleet.Models;

namespace Transleet.Services
{
    public class ProjectsService:IService<Project>
    {
        private readonly IMongoCollection<Project> _collection;

        public ProjectsService(IMongoDbContext context)
        {
            _collection= context.GetCollection<Project>("Projects");
        }

        public async Task<List<Project>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<Project?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Project obj) =>
            await _collection.InsertOneAsync(obj);

        public async Task UpdateAsync(string id, Project updated) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updated);

        public async Task RemoveAsync(string? id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
