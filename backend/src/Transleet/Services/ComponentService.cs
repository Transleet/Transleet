using System.Reactive.Subjects;
using MongoDB.Bson;
using Transleet.Models;
using Transleet.MongoDbGenericRepository.Abstractions;

namespace Transleet.Services
{
    public interface IComponentService
    {
        IAsyncEnumerable<Component> GetAllByProjectIdAsync(ObjectId projectId);
        Task<Component> GetByIdAsync(ObjectId id);
        Task AddAsync(Component item);
        Task UpdateAsync(Component item);
        Task DeleteByIdAsync(ObjectId id);
        void Subscribe(Action<ComponentNotification> onNext);
    }

    public class ComponentService : IComponentService
    {
        private readonly IMongoRepository<Component> _componentRepository;
        private readonly IProjectService _projectService;
        private readonly ILogger<ComponentService> _logger;
        private readonly ISubject<ComponentNotification> _subject;

        public ComponentService(IMongoRepository<Component> componentRepository, IProjectService projectService, ILogger<ComponentService> logger)
        {
            _componentRepository = componentRepository;
            _projectService = projectService;
            _logger = logger;
            _subject = new Subject<ComponentNotification>();
        }

        public async IAsyncEnumerable<Component> GetAllByProjectIdAsync(ObjectId projectId)
        {
            var project = await _projectService.GetByIdAsync(projectId);
            if (project.Components is not null)
            {
                foreach (var item in project.Components)
                {
                    yield return item;
                }
            }
        }

        public Task<Component> GetByIdAsync(ObjectId id)
        {
            return _componentRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Component item)
        {
            item.Id = ObjectId.GenerateNewId();
            item.CreatedAt = DateTimeOffset.Now;
            item.UpdatedAt =DateTimeOffset.Now;
            await _componentRepository.AddOneAsync(item);
            _subject.OnNext(new ComponentNotification(item.Id,NotificationOperation.Added));
        }

        public async Task UpdateAsync(Component item)
        {
            await _componentRepository.UpdateOneAsync(item);
            _subject.OnNext(new ComponentNotification(item.Id,NotificationOperation.Updated));
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            await _componentRepository.DeleteOneAsync(_ => _.Id == id);
            _subject.OnNext(new ComponentNotification(id,NotificationOperation.Removed));
        }

        public void Subscribe(Action<ComponentNotification> onNext)
        {
            _subject.Subscribe(onNext);
        }
    }
}
