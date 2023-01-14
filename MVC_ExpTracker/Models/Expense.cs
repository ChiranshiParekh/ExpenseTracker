using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ExpTracker.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }
        public int CategoryId { get; set; }
        public string ExpenseTitle { get; set; }
        public string ExpenseDescription { get; set; } = string.Empty;
        public DateTime ExpenseDate { get; set; } = DateTime.Now;
        public decimal ExpenseAmount { get; set; }

        public virtual Category Category { get; set; }
    }
}