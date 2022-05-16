using System.Reactive.Subjects;
using Microsoft.EntityFrameworkCore;
using Transleet.Models;

namespace Transleet.Services
{
    public interface IUserService
    {
        IAsyncEnumerable<User> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        void Subscribe(Action<UserNotification> onNext);
    }

    public class UserService:IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly ISubject<UserNotification> _subject;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _subject = new Subject<UserNotification>();
        }

        public IAsyncEnumerable<User> GetAllAsync()
        {
            return _dbContext.Users.AsAsyncEnumerable();
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            string strId = id.ToString();
            return _dbContext.Users.FirstOrDefaultAsync(_ => _.Id == strId);
        }

        public void Subscribe(Action<UserNotification> onNext)
        {
            _subject.Subscribe(onNext);
        }
    }
}
