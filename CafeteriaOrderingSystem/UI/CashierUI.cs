using CafeteriaOrderingSystem.BLL;
using CafeteriaOrderingSystem.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeteriaOrderingSystem.UI
{
    public partial class CashierUI : Form
    {
        public CashierUI()
        {
            InitializeComponent();
        }
        public int RowIndex = 0;

        private void label4_Click(object sender, EventArgs e)
        {
            if (CustomMsgBoxUI.Show("هل انت متأكد من الخروج", "رسالة تأكيد", "نعم", "لا") == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            LoginUI admin = new LoginUI();
            this.Hide();
            admin.ShowDialog();


        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void CashierUI_Load(object sender, EventArgs e)
        {
            DataAccess _DataAccess = new DataAccess();
            label2.Text = DateTime.Now.ToLongDateString();
            //label2.Text = DateTime.Now.ToLocalTime().ToLongTimeString();
            int SaleID = _DataAccess.ReturnSaleID();
            label8.Text = "اليوم#" + Convert.ToString(SaleID);

            ArrayList AllCategories = _DataAccess.RetreiveAllCategoriesFromDatabase();

            foreach (CategoryBLL Category in AllCategories)
            {
                Button btn = new Button();
                Label Leb = new Label(); ;
                Leb.AutoSize = false;
                Leb.TextAlign = ContentAlignment.TopLeft;
                Leb.Text = Category.Category_name;
                btn.Size = new System.Drawing.Size(100, 100);

                MemoryStream ms = new MemoryStream(Category.Category_pics);
                btn.Image = Image.FromStream(ms);
                btn.Image = new Bitmap(btn.Image, btn.Size);
                btn.Tag = Category.ID;
                flowLayoutPanel1.Controls.Add(btn);
                flowLayoutPanel1.Controls.Add(Leb);

                btn.Click += CategoryButtonClick;
            }
            textBox3.Text = LoginUI.cmCashier;
            textBox3.Enabled = false;

        }
        #region CategoryButtonClick
        void CategoryButtonClick(object sender, EventArgs e)
        {
            flowLayoutPanel2.Controls.Clear();

            Button btn = (Button)sender;

            int CategoryID = Convert.ToInt32(btn.Tag);

            DataAccess _DataAccess = new DataAccess();

            foreach (ProductBLL Product in _DataAccess.RetreiveProductsFromCategory(CategoryID))
            {
                Button ProductButton = new Button();
                Label Leb = new Label();
                Leb.AutoSize = true;
                Leb.TextAlign = ContentAlignment.TopRight;
                Leb.Text = Product.ProductName;

                ProductButton.Size = new System.Drawing.Size(100, 100);

                MemoryStream ms = new MemoryStream(Product.ProductPicture);
                ProductButton.Image = Image.FromStream(ms);
                ProductButton.Image = new Bitmap(ProductButton.Image, ProductButton.Size);

                ProductButton.Tag = Product.ID;


                flowLayoutPanel2.Controls.Add(ProductButton);
                flowLayoutPanel2.Controls.Add(Leb);

                ProductButton.Click += ProductButton_Click;
               

            }
        }
        #endregion

        

        #region CheckProductAlreadyAdded
        public bool CheckProductAlreadyAdded(int ProductID)
        {
            foreach (DataGridViewRow Row in dataGridView1.Rows)
            {
                if (Convert.ToInt32(Row.Cells["Column2"].Value) == ProductID)
                {
                    RowIndex = Row.Index;
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region ProductButton_Click
        void ProductButton_Click(object sender, EventArgs e)
        {
            Button ProductButton = sender as Button;

            DataAccess _DataAccess = new DataAccess();

            int ProductID = Convert.ToInt32(ProductButton.Tag);

            ProductBLL ProductDetails = _DataAccess.RetreiveProductDetails(ProductID);

            if (CheckProductAlreadyAdded(ProductID))
            {
                int Quantity = Convert.ToInt32(dataGridView1.Rows[RowIndex].Cells["Column4"].Value);
                decimal Price = Convert.ToDecimal(dataGridView1.Rows[RowIndex].Cells["Column5"].Value);

                Quantity++;
                
                decimal TotalPrice = Convert.ToDecimal(Quantity * Price);

                dataGridView1.Rows[RowIndex].Cells["Column4"].Value = Quantity;
                dataGridView1.Rows[RowIndex].Cells["Column1"].Value = TotalPrice;

                textBox1.Text = CalculateTotalBill(dataGridView1).ToString();
            }
            else
            {
                dataGridView1.Rows.Add(ProductID, ProductDetails.ProductName, 1, Convert.ToDecimal(ProductDetails.ProductPrice), Convert.ToDecimal(ProductDetails.ProductPrice * 1));

                textBox1.Text = CalculateTotalBill(dataGridView1).ToString();
            }
        }
        #endregion

        #region CalculateTotalBill
        public decimal CalculateTotalBill(DataGridView ProductsGridView)
        {
            decimal TotalBill = 0;

            foreach (DataGridViewRow Row in ProductsGridView.Rows)
            {
                decimal ProductTotal = Convert.ToDecimal(Row.Cells["Column1"].Value);

                TotalBill = TotalBill + ProductTotal;
            }

            return TotalBill;
        }
        #endregion

        public void button1_Click(object sender, EventArgs e)
        {
            ArrayList ProductDetail = new ArrayList();
            DataAccess _DataAccess = new DataAccess();
            int SaleID = _DataAccess.ReturnSaleID();

            int cashier = _DataAccess.ReturnUsernameID(textBox3.Text);
            if (textBox3.Text == "")
            {
                CustomRegularMsgBox.Show("اختار كاشير لو سمحت");
            }
            else
            {
                foreach (DataGridViewRow Row in dataGridView1.Rows)
                {
                    try
                    {
                        if (Row.IsNewRow) continue;
                        {
                            string ProductName = (string)Row.Cells["Column3"].Value.ToString();
                            decimal ProductPrice = Convert.ToDecimal(Row.Cells["Column5"].Value);
                            int Quantity = Convert.ToInt32(Row.Cells["Column4"].Value);
                            decimal ProductTotal = Convert.ToDecimal(Row.Cells["Column1"].Value);
                            int ProductID = Convert.ToInt32(Row.Cells["Column2"].Value);

                            ProductDetail.Add(new SaleItemsBLL() { ProductName = ProductName, ProductPrice = ProductPrice, Quantity = Quantity, ProductTotal = ProductTotal, UserID = cashier, ID = ProductID });
                            }
                    }
                    finally
                    {
                    }
                }

               // DataAccess _DataAccess = new DataAccess();
                
                if (_DataAccess.RecordASale(ProductDetail))
                {
                    if (ProductDetail.Count > 0 )
                    {
                        try
                        {
                            //PrintOrders(ProductDetail);
                            CustomRegularMsgBox.Show("success");
                             clear();
                          
                        }
                        catch (Exception ee)
                        {

                            MessageBox.Show(ee.Message);
                        }
                       
                     }
                    else
                    {
                        CustomRegularMsgBox.Show("لا يوجد مبيعات");
                    }
                    
                }
                else CustomRegularMsgBox.Show("حصل خطأ ما؟");
            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
                {
                   
                        decimal DeletedProductTotal = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value);

                        decimal CurrentTotalBill = Convert.ToDecimal(textBox1.Text);

                        CurrentTotalBill = CurrentTotalBill - DeletedProductTotal;

                        dataGridView1.Rows.RemoveAt(e.RowIndex);
                        textBox1.Text = CurrentTotalBill.ToString();
                    
                }
            }
        }

        private void clear()
        {
            textBox1.Text = Convert.ToDecimal("0.00").ToString();
             dataGridView1.Rows.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            DataAccess _DataAccess = new DataAccess();
            if (CustomMsgBoxUI.Show("هل تريد بدء معاملة جديدة", "تأكيد المعاملة", "موافق", "الغاء") == DialogResult.Yes)
            {
                DateTime SaleTime = DateTime.Now.Date;
                DateTime DBSaleTime = _DataAccess.ReturnDateTime().Date;
                int result = DateTime.Compare(SaleTime, DBSaleTime);
                if (result == 0)
                {
                    CustomRegularMsgBox.Show("لقد بدأت معاملة جديدة مسبقا");
                }
                else
                {
                    _DataAccess.AddANewSale();
                    int SaleID = _DataAccess.ReturnSaleID();
                    label8.Text = "المعاملة#" + Convert.ToString(SaleID);
                }
                
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataAccess _DataAccess = new DataAccess();
            int SaleID = _DataAccess.ReturnSaleID();
            int userID = _DataAccess.ReturnUsernameID(textBox3.Text);
            if (textBox3.Text == "")
            {
                CustomRegularMsgBox.Show("اختار الكاشير لو سمحت");
            }
            else
            {

                CashierSalesUI CashierRep = new CashierSalesUI(SaleID, userID);
                CashierRep.ShowDialog();
            }
            
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Button ProductButton = sender as Button;

            DataAccess _DataAccess = new DataAccess();

            int ProductID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Column2"].Value);

            ProductBLL ProductDetails = _DataAccess.RetreiveProductDetails(ProductID);

            if (CheckProductAlreadyAdded(ProductID))
            {
                int Quantity = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Column4"].Value);
                decimal Price = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["Column5"].Value);
                if (Quantity == -Quantity)
                {
                    Quantity = 0;
                }else
                Quantity--;

                decimal TotalPrice = Convert.ToDecimal(Quantity * Price);

                dataGridView1.Rows[e.RowIndex].Cells["Column4"].Value = Quantity;
                dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value = TotalPrice;

                textBox1.Text = CalculateTotalBill(dataGridView1).ToString();
            }
            
        }

        private void PrintOrders(ArrayList _List)
        {
            rptMainOrders report = new rptMainOrders();
            Random rand = new Random();
            string s = "#" + rand.Next(0,99999).ToString("0000");
            string total = CalculateTotalBill(dataGridView1).ToString();
            DateTime d = DateTime.Now;
            report.SetDataSource(_List);
            report.SetParameterValue("pRecieteID", s);
            report.SetParameterValue("pTotal", total);
            report.SetParameterValue("pDate", d);
            report.PrintOptions.PrinterName = "EPSON TM-T20II Receipt";
            report.PrintToPrinter(1, true, 0, 0);
            report.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MExpenseUI exp = new MExpenseUI();
            exp.ShowDialog();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        
    }

}
