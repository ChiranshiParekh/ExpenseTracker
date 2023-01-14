using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ExpTracker.Models
{
    public class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            this.Expenses = new HashSet<Expense>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal CategoryLimit { get; set; }
        public decimal CategoryExpense { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}