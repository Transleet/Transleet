using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MongoDB.Bson;
using Transleet.Models;
using Transleet.Repositories;

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
        private readonly IProjectRepository _repository;
        private readonly ILogger<ProjectService> _logger;
        private readonly ISubject<ProjectNotification> _subject;

        public ProjectService(IProjectRepository repository, ILogger<ProjectService> logger)
        {
            _repository = repository;
            _logger = logger;
            _subject = new Subject<ProjectNotification>();

        }

        public async IAsyncEnumerable<Project> GetAllAsync()
        {
            foreach (var item in await _repository.GetAllAsync<Project>(_=>true))
            {
                yield return item;
            }
        }

        public Task<Project> GetByIdAsync(ObjectId id)
        {
            return _repository.GetByIdAsync<Project>(id);
        }

        public async Task AddAsync(Project item)
        {
            item.Id = ObjectId.GenerateNewId();
            item.CreatedAt = DateTimeOffset.Now;
            item.UpdatedAt = DateTimeOffset.Now;
            await _repository.AddOneAsync(item);
            _subject.OnNext(new ProjectNotification(item.Id, NotificationOperation.CreatedOrUpdated));

        }

        public async Task UpdateAsync(Project item)
        {
            await _repository.UpdateOneAsync(item);
            _subject.OnNext(new ProjectNotification(item.Id, NotificationOperation.CreatedOrUpdated));
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            await _repository.DeleteOneAsync<Project>(_ => _.Id == id);
            _subject.OnNext(new ProjectNotification(id, NotificationOperation.Removed));
        }

        public void Subscribe(Action<ProjectNotification> onNext)
        {
            _subject.Subscribe(onNext);
        }
    }
}
