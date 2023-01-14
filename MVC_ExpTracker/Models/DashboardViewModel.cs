using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ExpTracker.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<ExpenseViewModel> Expenses { get; set; }

        public decimal Limit { get; set; }
        public decimal ExpenseAmt { get; set; }
    }
}