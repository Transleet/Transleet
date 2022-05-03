using MongoDB.Bson;
using MongoDbGenericRepository;

namespace Transleet.Repositories
{
    public interface IComponentRepository:IBaseMongoRepository<ObjectId>
    {

    }
    public class ComponentRepository:BaseMongoRepository<ObjectId>,IComponentRepository
    {
        public ComponentRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
