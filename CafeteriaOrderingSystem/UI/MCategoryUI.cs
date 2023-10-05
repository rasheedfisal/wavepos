using CafeteriaOrderingSystem.BLL;
using CafeteriaOrderingSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeteriaOrderingSystem.UI
{
    public partial class MCategoryUI : Form
    {
        public MCategoryUI()
        {
            InitializeComponent();
            LoadRecords();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (ColName == "Edit")
            {
                AddCategoryUI edit = new AddCategoryUI(this);
                edit.CatID.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                edit.textBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                
                byte[] imgInBytes = (byte[]) dataGridView1.CurrentRow.Cells[3].Value;
                MemoryStream imgBytesMemoryStream = new MemoryStream(imgInBytes);
                Image ConvertedImg = Image.FromStream(imgBytesMemoryStream);
                edit.picBox.Image = ConvertedImg;
                edit.btnSave.Enabled = false;
                edit.ShowDialog();
            }
            else if (ColName == "Delete")
            {

                if(CustomMsgBoxUI.Show("هل انت متأكد من حذف هذه القائمة؟", "رسالة تحذير", "نعم", "لا") == DialogResult.Yes)
                {
                    DataAccess _DataAccess = new DataAccess();
                    _DataAccess.DeleteCategory(Convert.ToInt32(dataGridView1[1, e.RowIndex].Value));
                    _DataAccess.DeleteProductAccossiatedWithCategory(Convert.ToInt32(dataGridView1[1, e.RowIndex].Value));
                    CustomRegularMsgBox.Show("تم حذف القائمة بنجاح");
                    LoadRecords();
                }
            }
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            AddCategoryUI acu = new AddCategoryUI(this);
            acu.btnUpdate.Enabled = false;
            acu.Dock = DockStyle.Fill;
            acu.ShowDialog();
            
        }
        public void LoadRecords()
        {
            DataAccess _DataAccess = new DataAccess();
            int i = 0;
            dataGridView1.Rows.Clear();

            foreach (CategoryBLL cat in _DataAccess.RetreiveAllCategoriesFromDatabase())
            {
                i += 1;
                dataGridView1.Rows.Add(i, cat.ID, cat.Category_name, cat.Category_pics);
            }
        }
    }
}
