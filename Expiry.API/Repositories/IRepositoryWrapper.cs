using System.Threading.Tasks;

namespace Expiry.Repositories
{
    public interface IRepositoryWrapper
    {
        IReminderRepository Reminder { get; }

        Task SaveAsync();
    }
}
