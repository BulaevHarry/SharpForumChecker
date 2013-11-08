using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZeroDayChecker;
using OverlockersChecker;

namespace SharpForumChecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
//             Checker0day ch0day = new Checker0day();
//             ch0day.Check(textBoxKeyWords.Text, "http://forum.0day.kiev.ua/index.php?showforum=303");

            CheckerOverlockers chOvrLck = new CheckerOverlockers();
            chOvrLck.Check(textBoxKeyWords.Text, "http://forum.overclockers.ua/viewforum.php?f=26");

            listBox1.Items.Clear();
            listBox1.Items.AddRange(chOvrLck.Founded.Split('½'));
        }
    }
}
