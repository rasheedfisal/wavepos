using CafeteriaOrderingSystem.BLL;
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
    public partial class SalesUI : Form
    {
        public SalesUI()
        {
            InitializeComponent();
            LoadRecords();
        }
        public void LoadRecords()
        {
            DataAccess _DataAccess = new DataAccess();
            int i = 0;
            dataGridView1.Rows.Clear();

            foreach (SalesBLL Sales in _DataAccess.RetreiveAllSalesFromDatabase())
            {
                i += 1;
                dataGridView1.Rows.Add(i, Sales.ID, Sales.DateOfSale, "كاشير");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (ColName == "cashier")
            {
                int SaleID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Column2"].Value);
                SalesReportUI SalesRep = new SalesReportUI(SaleID);
                SalesRep.ShowDialog();
            }
            
        }

        private void SalesUI_Load(object sender, EventArgs e)
        {
        }
    }
}
