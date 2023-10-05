using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeteriaOrderingSystem.BLL
{
    class SaleItemsBLL
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public decimal ProductTotal { get; set; }
        public int UserID { get; set; }
        public int PIDs { get; set; }
    }
}
