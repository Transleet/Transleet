using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MongoDB.Bson;
using Transleet.Models;
using Transleet.MongoDbGenericRepository.Abstractions;

namespace Transleet.Services
{
    public interface IProjectService
    {
        IAsyncEnumerable<Project> GetAllAsync();
        Task<Project> GetByIdAsync(ObjectId id);
        Task AddAsync(Project item);
        Task UpdateAsync(Project item);
        Task DeleteByIdAsync(ObjectId id);
        void Subscribe(Action<ProjectNotification> onNext);
    }

    public class ProjectService:IProjectService
    {
        private readonly IMongoRepository<Project> _repository;
        private readonly ILogger<ProjectService> _logger;
        private readonly ISubject<ProjectNotification> _subject;

        public ProjectService(IMongoRepository<Project> repository, ILogger<ProjectService> logger)
        {
            _repository = repository;
            _logger = logger;
            _subject = new Subject<ProjectNotification>();

        }

        public async IAsyncEnumerable<Project> GetAllAsync()
        {
            foreach (var item in await _repository.GetAllAsync(_=>true))
            {
                yield return item;
            }
        }

        public Task<Project> GetByIdAsync(ObjectId id)
        {
            return _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Project item)
        {
            item.Id = ObjectId.GenerateNewId();
            item.CreatedAt = DateTimeOffset.Now;
            item.UpdatedAt = DateTimeOffset.Now;
            await _repository.AddOneAsync(item);
            _subject.OnNext(new ProjectNotification(item.Id, NotificationOperation.Added));

        }

        public async Task UpdateAsync(Project item)
        {
            await _repository.UpdateOneAsync(item);
            _subject.OnNext(new ProjectNotification(item.Id, NotificationOperation.Updated));
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            await _repository.DeleteOneAsync(_ => _.Id == id);
            _subject.OnNext(new ProjectNotification(id, NotificationOperation.Removed));
        }

        public void Subscribe(Action<ProjectNotification> onNext)
        {
            _subject.Subscribe(onNext);
        }
    }
}
