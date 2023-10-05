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
    public partial class MUsersUI : Form
    {
        public MUsersUI()
        {
            InitializeComponent();
            LoadRecords();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            AddUserUI acu = new AddUserUI(this);
            acu.btnUpdate.Enabled = false;
            acu.Dock = DockStyle.Fill;
            acu.ShowDialog();
        }

        public void LoadRecords()
        {
            DataAccess _DataAccess = new DataAccess();
            int i = 0;
            dataGridView1.Rows.Clear();

            foreach (UsersBLL product in _DataAccess.RetreiveAllUsersFromDatabase())
            {
                i += 1;
                dataGridView1.Rows.Add(i, product.ID, product.Username, product.U_password, product.Usertype);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (ColName == "Edit")
            {
                AddUserUI edit = new AddUserUI(this);
                edit.userID.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                edit.textBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                edit.textBox2.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                edit.comboBox1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                
                edit.btnSave.Enabled = false;
                edit.ShowDialog();
            }
            else if (ColName == "Delete")
            {

                if (CustomMsgBoxUI.Show("هل انت متأكد من حذف المستخدم؟", "رسالة تحذير", "نعم", "لا") == DialogResult.Yes)
                {
                    DataAccess DA = new DataAccess();
                    DA.DeleteUser(Convert.ToInt32(dataGridView1[1, e.RowIndex].Value));
                    CustomRegularMsgBox.Show("تم حذف المستخدم بنجاح");
                    LoadRecords();
                }
            }
        }
    }
}
