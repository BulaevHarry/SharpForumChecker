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
    public partial class Settings : Form
    {
        private Form1 parent_form;

        public Settings(Form1 prnt, int intrvl, int rndm)
        {
            InitializeComponent();
            parent_form = prnt;
            trackBar1.Value = intrvl;
            trackBar1_Scroll(this, new EventArgs());
            trackBar2.Value = rndm;
            trackBar2_Scroll(this, new EventArgs());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            labelReloadRate.Text = Convert.ToString(trackBar1.Value) + " мин.";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            labelRand.Text = "±" + Convert.ToString(trackBar2.Value/2.0) + " мин.";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            parent_form.changeSettings(trackBar1.Value, trackBar2.Value);
            this.Close();
        }

    }
}
