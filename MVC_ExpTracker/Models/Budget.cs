using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ExpTracker.Models
{
    public class Budget
    {
        public int budgetId { get; set; }
        public decimal ExpenseLimit { get; set; }
        public decimal TotalExpense { get; set; }
    }
}