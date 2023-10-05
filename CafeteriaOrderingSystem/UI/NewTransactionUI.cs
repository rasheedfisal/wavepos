using CafeteriaOrderingSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeteriaOrderingSystem.UI
{
    public partial class NewTransactionUI : Form
    {
        CashierUI cash = new CashierUI();
        public NewTransactionUI()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DateTime SaleTime = DateTime.Now.Date;
            DataAccess _DataAccess = new DataAccess();
            DateTime DBSaleTime = _DataAccess.ReturnDateTime().Date;
            int result = DateTime.Compare(SaleTime, DBSaleTime);
            if (result == 0)
            {
                CustomRegularMsgBox.Show("لقد بدأت معاملة جديدة مسبقا");
            }
            else
            {
                _DataAccess.AddANewSale();
                this.Hide();
                cash.ShowDialog();
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            cash.ShowDialog();
        }
    }
}
