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
    public partial class AddCategoryUI : Form
    {
        MCategoryUI managecuI;
        public AddCategoryUI(MCategoryUI mCUI)
        {
            InitializeComponent();
            managecuI = mCUI;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || picBox.Image == null)
            {
                CustomRegularMsgBox.Show("الرجاء املاء الاماكن الشاغرة");
            }
            else
            {
                if (CustomMsgBoxUI.Show("هل انت متأكد من حفظ القائمة", "رسالة تاكيد", "نعم", "لا") == DialogResult.Yes)
                {
                    DataAccess _DataAccess = new DataAccess();

                    bool CategoryAddedOrNot = _DataAccess.AddNewCategoryToDatabase(textBox1.Text, picBox);

                    if (CategoryAddedOrNot)
                    {
                        CustomRegularMsgBox.Show("اضيفت القائمة");
                        clear();
                        managecuI.LoadRecords();
                    }
                    else CustomRegularMsgBox.Show("لم تضاف القائمة");
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
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

        private void clear()
        {
            textBox1.Text = string.Empty;
            picBox.Image.Dispose();
            picBox.Image = null;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CustomMsgBoxUI.Show("هل انت متأكد من تعديل القائمة", "رسالة تاكيد", "نعم", "لا") == DialogResult.Yes)
            {
                if (textBox1.Text == "" || picBox.Image == null)
                {
                    CustomRegularMsgBox.Show("الرجاء املاء الاماكن الشاغرة");
                }
                else
                {
                    DataAccess _DataAccess = new DataAccess();

                    bool CategoryAddedOrNot = _DataAccess.UpdateCategory(int.Parse(CatID.Text), textBox1.Text, picBox);

                    if (CategoryAddedOrNot)
                    {
                        CustomRegularMsgBox.Show("تم تعديل القائمة");
                        clear();
                        managecuI.LoadRecords();
                    }
                    else CustomRegularMsgBox.Show("حصل خطأ ما");
                }
            }
        }

        private void AddCategoryUI_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
