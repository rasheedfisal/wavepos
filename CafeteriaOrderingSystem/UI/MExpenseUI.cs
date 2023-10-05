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
    public partial class MExpenseUI : Form
    {
        
        public MExpenseUI()
        {
            InitializeComponent();
            LoadRecords();
        }
        string users = LoginUI.cmCashier;
        string pass = LoginUI.pass;

        public void LoadRecords()
        {
            DataAccess _DataAccess = new DataAccess();
            int SaleID = _DataAccess.ReturnSaleID();
            int UserID = _DataAccess.ReturnUserID(users, pass);
            int i = 0;
            dataGridView1.Rows.Clear();

            foreach (ExpensesBLL cat in _DataAccess.RetreiveExpenseDetails(SaleID, UserID))
            {
                i += 1;
                dataGridView1.Rows.Add(i, cat.ID, cat.Expense_name, cat.Ex);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            AddExpenseUI addexp = new AddExpenseUI(this);
            addexp.btnUpdate.Enabled = false;
            addexp.Dock = DockStyle.Fill;
            addexp.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (ColName == "Edit")
            {
                AddExpenseUI edit = new AddExpenseUI(this);
                edit.ExpenseID.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                edit.textBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                edit.textBox2.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();

                edit.btnSave.Enabled = false;
                edit.ShowDialog();
            }
            else if (ColName == "Delete")
            {

                if (CustomMsgBoxUI.Show("هل انت متأكد من حذف المنصرف؟", "رسالة تحذير", "نعم", "لا") == DialogResult.Yes)
                {
                    DataAccess DA = new DataAccess();
                    DA.DeleteExpense(Convert.ToInt32(dataGridView1[1, e.RowIndex].Value));
                    CustomRegularMsgBox.Show("تم حذف المنصرف بنجاح");
                    LoadRecords();
                }
            }
        }
    }
}
