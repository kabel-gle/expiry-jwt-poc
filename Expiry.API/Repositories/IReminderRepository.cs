using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Expiry.Dtos;
using Expiry.Models;

namespace Expiry.Repositories
{
    public interface IReminderRepository : IRepositoryBase<Reminder>
    {
        Task<IEnumerable<ReminderDto>> GetAllRemindersAsync();

        Task<ReminderDto> GetReminderByIdAsync(Guid reminderId);

        void CreateReminder(ReminderDto reminder);

        void UpdateReminder(Guid id, ReminderDto reminder);

        void DeleteReminder(Guid id);
    }
}
