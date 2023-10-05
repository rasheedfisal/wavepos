using CafeteriaOrderingSystem.BLL;
using CafeteriaOrderingSystem.DAL;
using System;
using System.Collections;
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
    public partial class LoginUI : Form
    {
        public LoginUI()
        {
            InitializeComponent();
        }
        DataAccess dt = new DataAccess();
        UsersBLL l = new UsersBLL();
        public static string cmCashier;
        public static string pass;
        private void btnSave_Click(object sender, EventArgs e)
        {
            l.Username = textBox1.Text;
            l.U_password = textBox2.Text;
            bool succes= dt.CheckLogin(l);

            if (succes == true)
            {
                cmCashier = l.Username;
                pass = l.U_password;
                if (dt.ReturnUsetype(textBox1.Text, textBox2.Text) == "مدير")
                {
                    AdminUI ADM = new AdminUI();
                    this.Hide();
                    ADM.ShowDialog();
                }
                else if (dt.ReturnUsetype(textBox1.Text, textBox2.Text) == "كاشير")
                {
                    DateTime SaleTime = DateTime.Now.Date;
                    DataAccess _DataAccess = new DataAccess();
                    DateTime DBSaleTime = _DataAccess.ReturnDateTime().Date;
                    int result = DateTime.Compare(SaleTime, DBSaleTime);
                    if (result == 0)
                    {
                        CashierUI ADM = new CashierUI();
                        this.Hide();
                        ADM.ShowDialog();
                    }
                    else
                    {
                        NewTransactionUI newTrans = new NewTransactionUI();
                        this.Hide();
                        newTrans.ShowDialog();

                    }
                }
                else
                {
                    CustomRegularMsgBox.Show("الاسم او كلمة السر خطأ");
                }
                
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
