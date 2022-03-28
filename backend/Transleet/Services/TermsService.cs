#nullable enable
using MongoDB.Driver;
using MongoDbGenericRepository;
using Transleet.Models;

namespace Transleet.Services;

public class TermsService : IService<Term>
{
    private readonly IMongoCollection<Term> _collection;

    public TermsService(IMongoDbContext context)
    {
        _collection = context.GetCollection<Term>("Vocabularies");
    }

    public async Task<List<Term>> GetAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<Term?> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Term obj) =>
        await _collection.InsertOneAsync(obj);

    public async Task UpdateAsync(string id, Term updated) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, updated);

    public async Task RemoveAsync(string? id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}