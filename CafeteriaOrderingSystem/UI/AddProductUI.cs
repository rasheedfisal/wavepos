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
    public partial class AddProductUI : Form
    {
        MProductUI manageProduct;
        public AddProductUI(MProductUI MPU)
        {
            InitializeComponent();
            manageProduct = MPU;

        }

        private void picBox_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Select Image file..";
            ofd.DefaultExt = ".jpg";
            ofd.Filter = "Media Files|*.jpg;*.png;*.gif;*.bmp;*.jpeg|All Files|*.*";

            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                picBox.Load(ofd.FileName);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void clear()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            comboBox1.Text = string.Empty;
            picBox.Image.Dispose();
            picBox.Image = null;
        }

        private void AddProductUI_Load(object sender, EventArgs e)
        {
            DataAccess _DataAccess = new DataAccess();

            foreach (CategoryBLL Category in _DataAccess.RetreiveAllCategoriesFromDatabase())
            {
                comboBox1.Items.Add(Category.Category_name);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || comboBox1.Text == "" || picBox.Image == null)
            {
                CustomRegularMsgBox.Show("الرجاء املاء الاماكن الشاغرة");
            }
            else
            {
                if (CustomMsgBoxUI.Show("هل انت متأكد من حفظ المنتج", "رسالة تاكيد", "نعم", "لا") == DialogResult.Yes)
                {
                    DataAccess _DataAccess = new DataAccess();

                    bool CategoryAddedOrNot = _DataAccess.AddNewProductToDatabase(textBox1.Text, Convert.ToDecimal(textBox2.Text), _DataAccess.ReturnCategoryID(comboBox1.SelectedItem.ToString()), picBox);

                    if (CategoryAddedOrNot)
                    {
                        CustomRegularMsgBox.Show("تم اضافت المنتج");
                        clear();
                        manageProduct.LoadRecords();
                    }
                    else CustomRegularMsgBox.Show("لم يضاف المنتج");
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CustomMsgBoxUI.Show("هل انت متأكد من تعديل المنتج", "رسالة تاكيد", "نعم", "لا") == DialogResult.Yes)
            {
                if (textBox1.Text == "" || textBox2.Text == "" || comboBox1.Text == "" || picBox.Image == null)
                {
                    CustomRegularMsgBox.Show("الرجاء املاء الاماكن الشاغرة");
                }
                else
                {
                    DataAccess _DataAccess = new DataAccess();

                    bool CategoryAddedOrNot = _DataAccess.UpdateProduct(int.Parse(ProductID.Text), textBox1.Text, Convert.ToDecimal(textBox2.Text), _DataAccess.ReturnCategoryID((comboBox1.Text)) ,picBox);

                    if (CategoryAddedOrNot)
                    {
                        CustomRegularMsgBox.Show("تم تعديل المنتج");
                        clear();
                        manageProduct.LoadRecords();
                    }
                    else CustomRegularMsgBox.Show("حصل خطأ ما");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
