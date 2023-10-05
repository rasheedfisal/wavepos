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
    public partial class CustomMsgBoxUI : Form
    {
        public CustomMsgBoxUI()
        {
            InitializeComponent();
        }

        public static CustomMsgBoxUI MsgBox;
        public static DialogResult result = DialogResult.No;

        public static DialogResult Show(string Text, string Caption, string btnOK, string btnCancel)
        {
            MsgBox = new CustomMsgBoxUI();
            MsgBox.label2.Text = Text;
            MsgBox.label1.Text = Caption;
            MsgBox.btnOK.Text = btnOK;
            MsgBox.btnCancel.Text = btnCancel;
            MsgBox.ShowDialog();
            return result;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            result = DialogResult.Yes;
            MsgBox.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            result = DialogResult.No;
            MsgBox.Close();
        }

        private void CustomMsgBoxUI_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
