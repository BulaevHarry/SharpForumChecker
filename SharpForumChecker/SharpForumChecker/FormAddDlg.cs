using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SharpForumChecker
{
    public partial class FormAddDlg : Form
    {
        private Form1 form_parent;

        public FormAddDlg(Form1 parent)
        {
            InitializeComponent();
            form_parent = parent;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form_parent.addWord(textBox1.Text);
            textBox1.Text = "";
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                button1_Click(sender, e);
            }

            if (e.KeyCode == Keys.Escape)
            {
                buttonClose_Click(sender, e);
            }
            
        }
    }
}
