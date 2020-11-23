using System;
using Microsoft.EntityFrameworkCore;

namespace Expiry.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Reminder> Reminder { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reminder>(reminder =>
            {
                reminder.HasKey(s => s.Id);
                reminder.Property(s => s.Name).IsRequired();
                reminder.Property(s => s.ExpiryDate).IsRequired();
                reminder.Property(s => s.ReminderDate);
                reminder.Property(s => s.ReminderTerm);
                reminder.Property(s => s.ReminderValue);
                reminder.Property(s => s.Price);
                reminder.Property(s => s.Notes);
            });

            modelBuilder.Entity<Reminder>().HasData
            (
                new Reminder
                {
                    Id = Guid.Parse("ae5a685c-ba5f-4d43-bc58-dde1f05d5024"),
                    Name = "Monitor Acer",
                    ExpiryDate = DateTime.Parse("14/10/2022"),
                    ReminderDate = DateTime.Parse("14/09/2022"),
                    ReminderTerm = ReminderTerm.Month,
                    ReminderValue = 1,
                    Price = 78.85m,
                    Notes = "Comprado en Amazon"
                },
                new Reminder
                {
                    Id = Guid.Parse("fc197cd7-32f9-4ccc-be69-e812009d0e1c"),
                    Name = "Spotify Premium",
                    ExpiryDate = DateTime.Parse("01/09/2020"),
                    ReminderDate = DateTime.Parse("30/11/2020"),
                    ReminderTerm = ReminderTerm.Day,
                    ReminderValue = 1,
                    Price = 9.99m,
                    Notes = "Promo 3 meses por 1",
                },
                new Reminder
                {
                    Id = Guid.Parse("6fa0ba69-8852-4722-a580-2bebc2623a7a"),
                    Name = "Seguro de hogar",
                    ExpiryDate = DateTime.Parse("01/03/2021"),
                    ReminderDate = DateTime.Parse("01/12/2020"),
                    ReminderTerm = ReminderTerm.Month,
                    ReminderValue = 3,
                    Price = 9.99m,
                    Notes = "Preguntar por hogar + vehiculo",
                }
            );
        }
    }
}
