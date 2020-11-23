using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expiry.Dtos
{
    public class ReminderDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime ExpiryDate { get; set; }

        public ReminderTermDto ReminderTerm { get; set; }

        public int ReminderValue { get; set; }

        public decimal Price { get; set; }

        public string Notes { get; set; }
    }

    public enum ReminderTermDto
    {
        Day = 0,
        Month = 1,
        Year = 2
    }
}
