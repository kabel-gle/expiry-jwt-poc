using System;

namespace Expiry.Models
{
    public class Reminder
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime ReminderDate { get; set; }

        public ReminderTerm ReminderTerm { get; set; }

        public int ReminderValue { get; set; }

        public decimal Price { get; set; }

        public string Notes { get; set; }
    }

    public enum ReminderTerm
    {
        Day = 0,
        Month = 1,
        Year = 2
    }
}
