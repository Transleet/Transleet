using System.Reactive.Subjects;
using Microsoft.EntityFrameworkCore;
using Transleet.Models;

namespace Transleet.Services
{
    public interface IComponentService
    {
        Task<Component?> GetByIdAsync(Guid id);
        Task AddAsync(Component item);
        Task UpdateAsync(Component item);
        Task DeleteByIdAsync(Guid id);
        void Subscribe(Action<ComponentNotification> onNext);
    }

    public class ComponentService : IComponentService
    {
        private readonly AppDbContext _dbContext;
        private readonly IProjectService _projectService;
        private readonly ILogger<ComponentService> _logger;
        private readonly ISubject<ComponentNotification> _subject;

        public ComponentService(AppDbContext dbContext, IProjectService projectService, ILogger<ComponentService> logger)
        {
            _dbContext = dbContext;
            _projectService = projectService;
            _logger = logger;
            _subject = new Subject<ComponentNotification>();
        }

        public Task<Component?> GetByIdAsync(Guid id)
        {
            return _dbContext.Components.FindAsync(id).AsTask();
        }

        public async Task AddAsync(Component item)
        {
            item.CreatedAt = DateTimeOffset.Now;
            item.UpdatedAt =DateTimeOffset.Now;
            await _dbContext.Components.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            _subject.OnNext(new ComponentNotification(item.Id,NotificationOperation.Added));
        }

        public async Task UpdateAsync(Component item)
        {
            _dbContext.Components.Update(item);
            await _dbContext.SaveChangesAsync();
            _subject.OnNext(new ComponentNotification(item.Id,NotificationOperation.Updated));
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var item = await _dbContext.Components.FindAsync(id);
            if (item != null)
            {
                _dbContext.Components.Remove(item);
                await _dbContext.SaveChangesAsync();
                _subject.OnNext(new ComponentNotification(id, NotificationOperation.Removed));
            }
        }

        public void Subscribe(Action<ComponentNotification> onNext)
        {
            _subject.Subscribe(onNext);
        }
    }
}
