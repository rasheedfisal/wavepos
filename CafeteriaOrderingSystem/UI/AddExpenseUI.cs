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
    public partial class AddExpenseUI : Form
    {
        MExpenseUI manageProduct;
        public AddExpenseUI(MExpenseUI MPU)
        {
            InitializeComponent();
            manageProduct = MPU;
        }
        string user = LoginUI.cmCashier;
        string pass = LoginUI.pass;

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && textBox2.Text == "")
            {
                CustomRegularMsgBox.Show("الرجاء املاء الاماكن الشاغرة");
            }
            else
            {
                if (CustomMsgBoxUI.Show("هل انت متأكد من حفظ المنصرف", "رسالة تاكيد", "نعم", "لا") == DialogResult.Yes)
                {
                    DataAccess _DataAccess = new DataAccess();
                    int SaleID = _DataAccess.ReturnSaleID();
                    int UserID = _DataAccess.ReturnUserID(user, pass);
                    bool ExpensAddedOrNot = _DataAccess.AddExpenses(textBox1.Text, Convert.ToDecimal(textBox2.Text), SaleID, UserID);

                    if (ExpensAddedOrNot)
                    {
                        CustomRegularMsgBox.Show("تم اضافت المنصرف");
                        clear();
                        manageProduct.LoadRecords();
                    }
                    else CustomRegularMsgBox.Show("لم يضاف المنصرف");
                }
            }
        }

        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CustomMsgBoxUI.Show("هل انت متأكد من تعديل المنصرف", "رسالة تاكيد", "نعم", "لا") == DialogResult.Yes)
            {
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    CustomRegularMsgBox.Show("الرجاء املاء الاماكن الشاغرة");
                }
                else
                {
                    DataAccess _DataAccess = new DataAccess();

                    bool CategoryAddedOrNot = _DataAccess.UpdateExpense(int.Parse(ExpenseID.Text), textBox1.Text, Convert.ToDecimal(textBox2.Text));

                    if (CategoryAddedOrNot)
                    {
                        CustomRegularMsgBox.Show("تم تعديل المنصرف");
                        clear();
                        manageProduct.LoadRecords();
                    }
                    else CustomRegularMsgBox.Show("حصل خطأ ما");
                }
            }
        }
    }
}
