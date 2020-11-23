using System.Threading.Tasks;
using AutoMapper;
using Expiry.Models;

namespace Expiry.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _repoContext;
        private IReminderRepository _reminder;

        public RepositoryWrapper(AppDbContext repositoryContext, IMapper mapper)
        {
            _repoContext = repositoryContext;
            _repoContext.Database.EnsureCreated();
            _mapper = mapper;
        }

        public IReminderRepository Reminder
        {
            get
            {
                if (_reminder == null)
                    _reminder = new ReminderRepository(_repoContext, _mapper);

                return _reminder;
            }
        }

        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}
