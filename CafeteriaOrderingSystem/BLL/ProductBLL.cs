using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeteriaOrderingSystem.BLL
{
    class ProductBLL
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductCategoryID { get; set; }
        public byte[] ProductPicture { get; set; }
    }
}
