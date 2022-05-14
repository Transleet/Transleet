using System.Reactive.Subjects;
using Microsoft.EntityFrameworkCore;
using Transleet.Models;

namespace Transleet.Services
{
    public interface IProjectService
    {
        IAsyncEnumerable<Project> GetAllAsync();
        Task<Project?> GetByIdAsync(Guid id);
        Task AddAsync(Project item);
        Task UpdateAsync(Project item);
        Task DeleteByIdAsync(Guid id);
        void Subscribe(Action<ProjectNotification> onNext);
    }

    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ProjectService> _logger;
        private readonly ISubject<ProjectNotification> _subject;

        public ProjectService(AppDbContext dbContext, ILogger<ProjectService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _subject = new Subject<ProjectNotification>();

        }

        public IAsyncEnumerable<Project> GetAllAsync()
        {
            return _dbContext.Projects.AsAsyncEnumerable();
        }

        public Task<Project?> GetByIdAsync(Guid id)
        {
            return _dbContext.Projects.Include(_ => _.Components).FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task AddAsync(Project item)
        {
            item.CreatedAt = DateTimeOffset.Now;
            item.UpdatedAt = DateTimeOffset.Now;
            await _dbContext.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            _subject.OnNext(new ProjectNotification(item.Id, NotificationOperation.Added));

        }

        public async Task UpdateAsync(Project item)
        {
            _dbContext.Projects.Update(item);
            await _dbContext.SaveChangesAsync();
            _subject.OnNext(new ProjectNotification(item.Id, NotificationOperation.Updated));
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var item = await _dbContext.Projects.Include(_=>_.Components).FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                _dbContext.Projects.Remove(item);
                await _dbContext.SaveChangesAsync();
                _subject.OnNext(new ProjectNotification(id, NotificationOperation.Removed));
            }

        }

        public void Subscribe(Action<ProjectNotification> onNext)
        {
            _subject.Subscribe(onNext);
        }
    }
}
