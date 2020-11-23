using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Expiry.Web.ViewModels
{
    public class ReminderViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Nombre")]
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Fecha de expiración")]
        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public ReminderTermViewModel ReminderTerm { get; set; }

        [Display(Name = "Recordar cada")]
        [Required]
        public int ReminderValue { get; set; }

        [Display(Name = "Precio")]
        public decimal Price { get; set; }

        [Display(Name = "Notas")]
        public string Notes { get; set; }
    }

    public enum ReminderTermViewModel
    {
        [Display(Name = "Día")]
        DAY = 0,
        [Display(Name = "Mes")]
        MONTH = 1,
        [Display(Name = "Año")]
        YEAR = 2
    }
}
