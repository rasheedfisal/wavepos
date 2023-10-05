using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeteriaOrderingSystem.BLL
{
    class ExpensesBLL
    {
        public int ID { get; set; }
        public string Expense_name { get; set; }
        public decimal Ex { get; set; }
        public int SaleID { get; set; }
        public int UserID { get; set; }

    }
}
