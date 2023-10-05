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
    public partial class AddUserUI : Form
    {
        MUsersUI manageProduct;
        public AddUserUI(MUsersUI MPU)
        {
            InitializeComponent();
            manageProduct = MPU;
        }

        private void AddUserUI_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clear()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            comboBox1.Text = string.Empty;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || comboBox1.Text == "")
            {
                CustomRegularMsgBox.Show("الرجاء املاء الاماكن الشاغرة");
            }
            else
            {
                if (CustomMsgBoxUI.Show("هل انت متأكد من حفظ المستخدم", "رسالة تاكيد", "نعم", "لا") == DialogResult.Yes)
                {
                    DataAccess _DataAccess = new DataAccess();

                    bool CategoryAddedOrNot = _DataAccess.AddNewUser(textBox1.Text, textBox2.Text, comboBox1.SelectedItem.ToString());

                    if (CategoryAddedOrNot)
                    {
                        CustomRegularMsgBox.Show("تم اضافت المستخدم");
                        clear();
                        manageProduct.LoadRecords();
                    }
                    else CustomRegularMsgBox.Show("لم يضاف المستخدم");
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CustomMsgBoxUI.Show("هل انت متأكد من تعديل المستخدم", "رسالة تاكيد", "نعم", "لا") == DialogResult.Yes)
            {
                if (textBox1.Text == "" || textBox2.Text == "" || comboBox1.Text == "")
                {
                    CustomRegularMsgBox.Show("الرجاء املاء الاماكن الشاغرة");
                }
                else
                {
                    DataAccess _DataAccess = new DataAccess();

                    bool CategoryAddedOrNot = _DataAccess.UpdateUser(int.Parse(userID.Text), textBox1.Text, textBox2.Text, comboBox1.Text);

                    if (CategoryAddedOrNot)
                    {
                        CustomRegularMsgBox.Show("تم تعديل المسنخدم");
                        clear();
                        manageProduct.LoadRecords();
                    }
                    else CustomRegularMsgBox.Show("حصل خطأ ما");
                }
            }
        }
    }
}
