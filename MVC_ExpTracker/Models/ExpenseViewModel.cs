using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ExpTracker.Models
{
    public class ExpenseViewModel
    {
        public int ExpenseId { get; set; }

        public int CategoryId { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}