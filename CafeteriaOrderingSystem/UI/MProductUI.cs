using CafeteriaOrderingSystem.BLL;
using CafeteriaOrderingSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeteriaOrderingSystem.UI
{
    public partial class MProductUI : Form
    {
        public MProductUI()
        {
            InitializeComponent();
            LoadRecords();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            AddProductUI acu = new AddProductUI(this);
            acu.btnUpdate.Enabled = false;
            acu.Dock = DockStyle.Fill;
            acu.ShowDialog();
        }
        public void LoadRecords()
        {
            DataAccess _DataAccess = new DataAccess();
            int i = 0;
            dataGridView1.Rows.Clear();

            foreach (ProductBLL product in _DataAccess.RetreiveAllProducts())
            {
                i += 1;
                dataGridView1.Rows.Add(i, product.ID, product.ProductName, _DataAccess.ReturnCategoryName(product.ProductCategoryID), product.ProductPrice, product.ProductPicture );
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (ColName == "Edit")
            {
                AddProductUI edit = new AddProductUI(this);
                edit.ProductID.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                edit.textBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                edit.comboBox1.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                edit.textBox2.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();

                byte[] imgInBytes = (byte[])dataGridView1.CurrentRow.Cells[5].Value;
                MemoryStream imgBytesMemoryStream = new MemoryStream(imgInBytes);
                Image ConvertedImg = Image.FromStream(imgBytesMemoryStream);
                edit.picBox.Image = ConvertedImg;
                edit.btnSave.Enabled = false;
                edit.ShowDialog();
            }
            else if (ColName == "Delete")
            {

                if (CustomMsgBoxUI.Show("هل انت متأكد من حذف هذه القائمة؟", "رسالة تحذير", "نعم", "لا") == DialogResult.Yes)
                {
                    DataAccess DA = new DataAccess();
                    DA.DeleteProduct(Convert.ToInt32(dataGridView1[1, e.RowIndex].Value));
                    CustomRegularMsgBox.Show("تم حذف القائمة بنجاح");
                    LoadRecords();
                }
            }
        }
    }
}
