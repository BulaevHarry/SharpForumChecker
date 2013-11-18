using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZeroDayChecker;
using SiteMonitorInterface;
using OverlockersChecker;
using HtmlAgilityPack;

namespace SharpForumChecker
{
    public partial class AddSite : Form
    {
        Form1 parent_form;
        string[,] ZeroDayLinks;
        public int site_list_index;

        public AddSite(Form1 pf)
        {
            InitializeComponent();
            parent_form = pf;

            ZeroDayLinks = new string[7, 2];
            ZeroDayLinks[0, 0] = "Авто, мото, вело";
            ZeroDayLinks[1, 0] = "ПК, комплектующие, оргтехника";
            ZeroDayLinks[2, 0] = "Фото и видео";
            ZeroDayLinks[3, 0] = "Электроника, бытовая техника, техника для дома";
            ZeroDayLinks[4, 0] = "Связь, мобильные телефоны и смартфоны";
            ZeroDayLinks[5, 0] = "Одежда и аксессуары";
            ZeroDayLinks[6, 0] = "Разное";
            ZeroDayLinks[0, 1] = "http://forum.0day.kiev.ua/index.php?showforum=302";
            ZeroDayLinks[1, 1] = "http://forum.0day.kiev.ua/index.php?showforum=303";
            ZeroDayLinks[2, 1] = "http://forum.0day.kiev.ua/index.php?showforum=305";
            ZeroDayLinks[3, 1] = "http://forum.0day.kiev.ua/index.php?showforum=306";
            ZeroDayLinks[4, 1] = "http://forum.0day.kiev.ua/index.php?showforum=307";
            ZeroDayLinks[5, 1] = "http://forum.0day.kiev.ua/index.php?showforum=308";
            ZeroDayLinks[6, 1] = "http://forum.0day.kiev.ua/index.php?showforum=310";
        }

        private void LBsites_Click(object sender, EventArgs e)
        {
            if (lbSites.SelectedIndex == 0)
            {
                lbThreads.Items.Clear();
                for (int i = 0; i < 7; i++)
                {
                    lbThreads.Items.Add(ZeroDayLinks[i, 0]);
                }
            }
            else
            if (lbSites.SelectedIndex == 1)
            {
                lbThreads.Items.Clear();
                lbThreads.Items.Add("список подфорумів оверклокерса");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lbSites.SelectedIndex == -1 || lbThreads.SelectedIndex == -1 || tbKeywords.Text == "")
            {
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }

            parent_form.addSiteIntoList(lbSites.SelectedIndex, tbName.Text, ZeroDayLinks[lbThreads.SelectedIndex, 1], tbKeywords.Text);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lbSites.SelectedIndex == -1 || lbThreads.SelectedIndex == -1 || tbKeywords.Text == "")
            {
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }
            parent_form.editSiteInTheList(lbSites.SelectedIndex, site_list_index, tbName.Text, ZeroDayLinks[lbThreads.SelectedIndex, 1], tbKeywords.Text);
            this.Close();
        }
    }
}
