using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Expiry.Dtos;
using Expiry.Exceptions;
using Expiry.Models;

namespace Expiry.Repositories
{
    public class ReminderRepository : RepositoryBase<Reminder>, IReminderRepository
    {
        private readonly IMapper _mapper;

        public ReminderRepository(AppDbContext repositoryContext, IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReminderDto>> GetAllRemindersAsync()
        {
            var reminders = await FindAll().OrderBy(x => x.Name).ToListAsync();
            var remindersResult = _mapper.Map<IEnumerable<ReminderDto>>(reminders);

            return remindersResult;
        }

        public async Task<ReminderDto> GetReminderByIdAsync(Guid reminderId)
        {
            var reminder = await FindByCondition(owner => owner.Id.Equals(reminderId)).FirstOrDefaultAsync();
            var reminderResult = _mapper.Map<ReminderDto>(reminder);

            return reminderResult;
        }

        public void CreateReminder(ReminderDto reminder)
        {
            var reminderDb = _mapper.Map<Reminder>(reminder);

            Create(reminderDb);
        }

        public void UpdateReminder(Guid id, ReminderDto reminder)
        {
            var reminderDb = FindByCondition(x => x.Id.Equals(id)).FirstOrDefault();

            if (reminderDb == null)
            {
                throw new NotFoundException();
            }

            reminderDb.Name = reminder.Name;
            reminderDb.Price = reminder.Price;
            reminderDb.Notes = reminder.Notes;
            reminderDb.ReminderTerm = ToReminderTerm(reminder.ReminderTerm);
            reminderDb.ReminderValue = reminder.ReminderValue;
            reminderDb.ExpiryDate = reminder.ExpiryDate;

            Update(reminderDb);
        }

        private ReminderTerm ToReminderTerm(ReminderTermDto reminderTerm)
        {
            switch (reminderTerm)
            {
                case ReminderTermDto.Day:
                    return ReminderTerm.Day;
                case ReminderTermDto.Month:
                    return ReminderTerm.Month;
                case ReminderTermDto.Year:
                    return ReminderTerm.Year;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void DeleteReminder(Guid id)
        {
            var reminder = FindByCondition(x => x.Id.Equals(id)).FirstOrDefault();

            if (reminder == null)
            {
                throw new NotFoundException();
            }

            Delete(reminder);
        }
    }
}
