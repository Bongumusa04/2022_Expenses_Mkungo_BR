using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTOs
{
    public class ExpenseDto
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; } = DateTime.Now;
 
        [MaxLength(500)]
        public string Description { get; set; }

        [Range(0, 1000000)]
        public decimal Amount { get; set; }
        public int AppUserId { get; set; }

         public static explicit operator ExpenseDto(Expense e) => new ExpenseDto
        {
            Id = e.Id,
            Date = e.Date,
            Description = e.Description,
            Amount = e.Amount
        };
    }
}