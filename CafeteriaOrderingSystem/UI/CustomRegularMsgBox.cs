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
    public partial class CustomRegularMsgBox : Form
    {
        public CustomRegularMsgBox()
        {
            InitializeComponent();
        }

        public static CustomRegularMsgBox MsgBox;
        public static DialogResult result = DialogResult.No;

        public static DialogResult Show(string Text)
        {
            MsgBox = new CustomRegularMsgBox();
            MsgBox.label2.Text = Text;
            MsgBox.ShowDialog();
            return result;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            result = DialogResult.Yes;
            MsgBox.Close();
        }

        private void CustomRegularMsgBox_Load(object sender, EventArgs e)
        {

        }
    }
}
