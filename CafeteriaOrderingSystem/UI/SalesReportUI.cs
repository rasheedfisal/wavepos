using CafeteriaOrderingSystem.BLL;
using CafeteriaOrderingSystem.DAL;
using System;
using System.Collections;
using System.Windows.Forms;

namespace CafeteriaOrderingSystem.UI
{
    public partial class SalesReportUI : Form
    {
        public SalesReportUI(int SaleID)
        {
            InitializeComponent();
            this.SaleID = SaleID;
            LoadRecords();
        }
        int SaleID = 0;
        int userID = 0;

      public void LoadRecords()
        {
            DataAccess _DataAccess = new DataAccess();
            userID = _DataAccess.ReturnUsernameID(comboBox1.Text);
            int i = 0;
            int o = 0;
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            foreach (SaleItemsBLL SalesItem in _DataAccess.RetreiveSaleItems(SaleID, userID))
            {
                i += 1;
                dataGridView1.Rows.Add(i, SalesItem.ID, SalesItem.ProductName, SalesItem.Quantity, SalesItem.ProductPrice, SalesItem.ProductTotal);
            }
            foreach (ExpensesBLL cat in _DataAccess.RetreiveExpenseDetails(SaleID, userID))
            {
                o += 1;
                dataGridView2.Rows.Add(o, cat.Expense_name, cat.Ex);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private decimal CalculateTotalBill(DataGridView ProductsGridView)
        {
            decimal TotalBill = 0;

            foreach (DataGridViewRow Row in ProductsGridView.Rows)
            {
                decimal ProductTotal = Convert.ToDecimal(Row.Cells["Column6"].Value);

                TotalBill = TotalBill + ProductTotal;
            }

            return TotalBill;
        }

        private decimal CalculateTotalBillEx(DataGridView ProductsGridView)
        {
            decimal TotalBill = 0;

            foreach (DataGridViewRow Row in ProductsGridView.Rows)
            {
                decimal ProductTotal = Convert.ToDecimal(Row.Cells["dataGridViewTextBoxColumn3"].Value);

                TotalBill = TotalBill + ProductTotal;
            }

            return TotalBill;
        }

        private void SalesReportUI_Load(object sender, EventArgs e)
        {
            DataAccess _DataAccess = new DataAccess();
            foreach (UsersBLL Category in _DataAccess.RetreiveAllUsersFromDatabase())
            {
                comboBox1.Items.Add(Category.Username);
            }
            label1.Text = "Total : " + CalculateTotalBill(dataGridView1).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ArrayList ProductDetail = new ArrayList();
            foreach (DataGridViewRow Row in dataGridView1.Rows)
            {
              try
              {
                        if (Row.IsNewRow) continue;
                        {
                            string ProductName = (string)Row.Cells["Column3"].Value.ToString();
                            decimal ProductPrice = Convert.ToDecimal(Row.Cells["Column5"].Value);
                            int Quantity = Convert.ToInt32(Row.Cells["Column4"].Value);
                            decimal ProductTotal = Convert.ToDecimal(Row.Cells["Column6"].Value);
                            int ProductID = Convert.ToInt32(Row.Cells["Column2"].Value);

                            ProductDetail.Add(new SaleItemsBLL() { ProductName = ProductName, ProductPrice = ProductPrice, Quantity = Quantity, ProductTotal = ProductTotal, UserID = userID, ID = ProductID });
                        }
                    }
                    finally
                    {

                    }
            }
            if (ProductDetail.Count > 0)
            {
                PrintSales(ProductDetail);
            }
            else
            {
                CustomRegularMsgBox.Show("لا توجد مبيعات في هذا التقرير");
            }
            
        }

        private void PrintSales(ArrayList _List)
        {
            decimal sale = CalculateTotalBill(dataGridView1);
            decimal exp = CalculateTotalBillEx(dataGridView2);
            rptSales report = new rptSales();
            DataAccess dt = new DataAccess();
            string total = "Total : " + (CalculateTotalBill(dataGridView1) - CalculateTotalBillEx(dataGridView2)).ToString(); ;
            DateTime d = DateTime.Now;

            report.SetDataSource(_List);
            report.SetParameterValue("pTotal", total);
            report.SetParameterValue("pData", d);
            report.SetParameterValue("pShift", dt.ReturnUsername(userID));
            report.SetParameterValue("pSales", sale.ToString() + " : الارباح");
            report.SetParameterValue("pExpense", exp.ToString() + " : المنصرفات");
            //report.PrintOptions.PrinterName = "EPSON TM-T20II Receipt";
            report.PrintToPrinter(1, true, 0, 0);
            report.Dispose();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecords();
            label7.Text = CalculateTotalBill(dataGridView1).ToString() + ": الارباح";
            label8.Text = CalculateTotalBillEx(dataGridView2).ToString() + ": المنصرفات";

            label1.Text = "Total : " + (CalculateTotalBill(dataGridView1) - CalculateTotalBillEx(dataGridView2)).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ArrayList pp = new ArrayList();
            foreach (DataGridViewRow rr in dataGridView2.Rows)
            {
                try
                {
                    if (rr.IsNewRow) continue;
                    {
                        string expname = (string)rr.Cells["dataGridViewTextBoxColumn2"].Value;
                        decimal expprice = Convert.ToDecimal(rr.Cells["dataGridViewTextBoxColumn3"].Value);
                        pp.Add(new ExpensesBLL() { Expense_name = expname, Ex = expprice });
                    }
                }
                finally
                {

                }

            }
            if (pp.Count > 0)
            {
                PrintExpense(pp);
            }
            else
            {
                CustomRegularMsgBox.Show("لا توجد منصرفات في هذا التقرير");
            }
        }

        private void PrintExpense(ArrayList ll)
        {
            DataAccess dt = new DataAccess();
            DateTime d = DateTime.Now;
            decimal exp = CalculateTotalBillEx(dataGridView2);
            rptExpense report = new rptExpense();
            report.SetDataSource(ll);
            report.SetParameterValue("pData", d);
            report.SetParameterValue("pShift", dt.ReturnUsername(userID));
            report.SetParameterValue("pExpense", exp.ToString() + " : المنصرفات");
            //report.PrintOptions.PrinterName = "EPSON TM-T20II Receipt";
            report.PrintToPrinter(1, true, 0, 0);
            report.Dispose();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
