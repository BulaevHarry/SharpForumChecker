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
        public int SiteListIndex;
        public string _name, _link, _keys;

        public AddSite(Form1 pf)
        {
            InitializeComponent();
            parent_form = pf;

            _name = "";
            _link = "";
            _keys = "";
        }

    #region Диалоговые кнопки
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    #endregion

    #region OnLoad
        private void AddSite_Load(object sender, EventArgs e)
        {
            tree0day.ExpandAll();
            treeSlandoReg.ExpandAll();

            var temp_node = treeSlandoReg.Nodes[0];
            treeSlandoReg.SelectedNode = temp_node;

            this.Height = 400;
            this.Top = parent_form.Top+10;

            tabControl1_SelectedIndexChanged(sender, e);
        }
    #endregion
        
    #region Вкладки
        public void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0: 
                    this.Height = 400;
                break;
                case 1: 
                    this.Height = 250;
                break;
                case 2: 
                    this.Height = 475;
                break;
            }

        }
    #endregion
        
    #region добавление
        private void btnAddSrch_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0: if (!process0day()) { return; } break;
                case 1: if (!processOverclockers()) { return; } break;
                case 2: if (!processSlando()) { return; } break;
            }

            parent_form.addSiteIntoList(tabControl1.SelectedIndex, _name, _link, _keys);
            this.Close();
        }
    #endregion

    #region изменение
        private void btnEditSearch_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0: if (!process0day()) { return; } break;
                case 1: if (!processOverclockers()) { return; } break;
                case 2: if (!processSlando()) { return; } break;
            }

            parent_form.editSiteInTheList(SiteListIndex, tabControl1.SelectedIndex, _name, _link, _keys);
            this.Close();
        }
    #endregion

    #region одей
        private void tree0day_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tree0day.SelectedNode.Level == 0 && tree0day.SelectedNode.Index == 1)
            {
                var temp_node = tree0day.Nodes[1].Nodes[0];
                tree0day.SelectedNode = temp_node;
                tree0day.Select();
                return;
            }
        }
        private bool process0day()
        {
            if (tree0day.SelectedNode == null || tb0dayKeys.Text == "") { System.Media.SystemSounds.Asterisk.Play(); return false; }

            _name = "0day -> ";
            if (tree0day.SelectedNode.Level == 0)
            { _name += tree0day.SelectedNode.Text; }
            else
            { _name += tree0day.SelectedNode.Parent.Text + " -> " + tree0day.SelectedNode.Text; }
            _name += " [" + tb0dayKeys.Text + "]";

            _link = tree0day.SelectedNode.Tag.ToString();

            _keys = tb0dayKeys.Text;

            return true;
        }
        public void select0day(string lnk)
        {

            if (lnk == tree0day.Nodes[0].Tag.ToString())
            {
                tree0day.SelectedNode = tree0day.Nodes[0];
            }
            else
                if (lnk == tree0day.Nodes[2].Tag.ToString())
                {
                    tree0day.SelectedNode = tree0day.Nodes[2];
                }
                else
                    foreach (TreeNode node in tree0day.Nodes[1].Nodes)
                    {
                        if (lnk == node.Tag.ToString())
                        {
                            tree0day.SelectedNode = node;
                        }
                    }
        }
    #endregion

    #region оверклокерс
        private bool processOverclockers()
        {
            if ((!rbOv0.Checked && !rbOv1.Checked && !rbOv2.Checked && !rbOv3.Checked) || tbOvKeys.Text == "") { System.Media.SystemSounds.Asterisk.Play(); return false; }

            _name = "Overclockers -> ";
            _keys = tbOvKeys.Text;

            if (rbOv0.Checked)
            {
                _name += rbOv0.Text;
                _link = "http://forum.overclockers.ua/viewforum.php?f=26";
            }
            if (rbOv1.Checked)
            {
                _name += rbOv1.Text;
                _link = "http://forum.overclockers.ua/viewforum.php?f=27";
            }
            if (rbOv2.Checked)
            {
                _name += rbOv2.Text;
                _link = "http://forum.overclockers.ua/viewforum.php?f=28";
            }
            if (rbOv3.Checked)
            {
                _name += rbOv3.Text;
                _link = "http://forum.overclockers.ua/viewforum.php?f=29";
            }

            _name += " [" + _keys + "]";
            return true;
        }
        public void selectOverclockers(string lnk)
        {
            if (lnk == "http://forum.overclockers.ua/viewforum.php?f=26")
            {
                rbOv0.Checked = true;
            }
            else
                if (lnk == "http://forum.overclockers.ua/viewforum.php?f=27")
                {
                    rbOv1.Checked = true;
                }
                else
                    if (lnk == "http://forum.overclockers.ua/viewforum.php?f=28")
                    {
                        rbOv2.Checked = true;
                    }
                    else
                        if (lnk == "http://forum.overclockers.ua/viewforum.php?f=29")
                        {
                            rbOv3.Checked = true;
                        }
        }
    #endregion

    #region сландо 
        private void treeSlandoReg_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeSlandoReg.SelectedNode.Level == 0 && treeSlandoReg.SelectedNode.Text != "Вся Украина")
            {
                var temp_node = treeSlandoReg.Nodes[treeSlandoReg.SelectedNode.Index].Nodes[0];
                treeSlandoReg.SelectedNode = temp_node;
                treeSlandoReg.Select();
                return;
            }
        }
        private bool processSlando()
        {
            if (treeSlando.SelectedNode == null || tbSlandoKeys.Text == "") { System.Media.SystemSounds.Asterisk.Play(); return false; }

            _link += "http://";
            _name = "Slando[" + treeSlandoReg.SelectedNode.Text + "]";

            if (!(treeSlandoReg.SelectedNode.Level == 0 && treeSlandoReg.SelectedNode.Index == 0))
            {
                _link += treeSlandoReg.SelectedNode.Tag.ToString() + ".";
                _link += treeSlandoReg.SelectedNode.Parent.Tag.ToString() + ".";
            }
            _link += "olx.ua/";
            if (treeSlando.SelectedNode.Level >= 2)
            {
                _link += treeSlando.SelectedNode.Parent.Parent.Tag.ToString() + "/";
                _name += " -> " + treeSlando.SelectedNode.Parent.Parent.Text;
            }
            if (treeSlando.SelectedNode.Level >= 1)
            {
                _link += treeSlando.SelectedNode.Parent.Tag.ToString() + "/";
                _name += " -> " + treeSlando.SelectedNode.Parent.Text;
            }
            if (treeSlando.SelectedNode.Level >= 0)
            {
                _link += treeSlando.SelectedNode.Tag.ToString() + "/";
                _name += " -> " + treeSlando.SelectedNode.Text;
            }

            _name += " [" + tbSlandoKeys.Text + "]";

            if (tbSlandoPriceFrom.Text != "" && tbSlandoPriceTo.Text == "")
            {
                _link += "?search%5Bfilter_float_price%3Afrom%5D=" + tbSlandoPriceFrom.Text;
                _name += " [цена: от " + tbSlandoPriceFrom.Text + " грн.]";
            }
            else if (tbSlandoPriceFrom.Text == "" && tbSlandoPriceTo.Text != "")
            {
                _link += "?search%5Bfilter_float_price%3Ato%5D=" + tbSlandoPriceTo.Text;
                _name += " [цена: до " + tbSlandoPriceTo.Text + " грн.]";
            }
            else if (tbSlandoPriceFrom.Text != "" && tbSlandoPriceTo.Text != "")
            {
                _link += "?search%5Bfilter_float_price%3Afrom%5D=" + tbSlandoPriceFrom.Text + "&search%5Bfilter_float_price%3Ato%5D=" + tbSlandoPriceTo.Text;
                _name += " [цена: от " + tbSlandoPriceFrom.Text + " до " + tbSlandoPriceTo.Text + " грн.]";
            }

            _keys = tbSlandoKeys.Text;
            return true;
        }
    #endregion


    }
}
