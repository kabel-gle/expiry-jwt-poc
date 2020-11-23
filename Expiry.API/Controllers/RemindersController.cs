using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Expiry.Dtos;
using Expiry.Exceptions;
using Expiry.Repositories;

namespace Expiry.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RemindersController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositories;

        public RemindersController(IRepositoryWrapper repositories)
        {
            _repositories = repositories;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReminderDto>>> GetReminder()
        {
            var reminders = await _repositories.Reminder.GetAllRemindersAsync();
            return Ok(reminders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReminderDto>> GetReminder(Guid id)
        {
            var reminder = await _repositories.Reminder.GetReminderByIdAsync(id);

            if (reminder == null)
            {
                return NotFound();
            }

            return reminder;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReminder(Guid id, ReminderDto reminder)
        {
            if (id != reminder.Id)
            {
                return BadRequest();
            }

            try
            {
                _repositories.Reminder.UpdateReminder(id, reminder);
                await _repositories.SaveAsync();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ReminderDto>> PostReminder(ReminderDto reminder)
        {
            try
            {
                _repositories.Reminder.CreateReminder(reminder);
                await _repositories.SaveAsync();

                return CreatedAtAction("GetReminder", new { id = reminder.Id }, reminder);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("{id}")]
        public async void DeleteReminder(Guid id)
        {
            _repositories.Reminder.DeleteReminder(id);
            await _repositories.SaveAsync();
        }
    }
}
