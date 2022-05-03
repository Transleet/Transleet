using MongoDB.Bson;
using MongoDbGenericRepository;

namespace Transleet.Repositories
{
    public interface IProjectRepository : IBaseMongoRepository<ObjectId>
    {

    }

    public class ProjectRepository : BaseMongoRepository<ObjectId>, IProjectRepository
    {
        public ProjectRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
