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
    public partial class Form1 : Form
    {
        private Resources.Checker0day ch0day;
        public FormAddDlg AddDlg;
        private int progressCounter;

        public Form1()
        {
            InitializeComponent();
            ch0day = new Resources.Checker0day();
            progressCounter = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (buttonSearch.Text == "Искать")
            {
                timerSearch.Enabled = true;
                buttonSearch.Text = "Стоп";
            }
            else
            {
                timerSearch.Enabled = false;
                progressBarSearch.Value = 0;
                buttonSearch.Text = "Искать";
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var SelIndexes = checkedListBoxSections.SelectedIndices;
            foreach (int Index in SelIndexes)
            {
                ch0day.mySect.Setup(Index, checkedListBoxSections.GetItemChecked(Index));
            }
        }

        private void buttonAddWord_Click(object sender, EventArgs e)
        {
            AddDlg = new FormAddDlg(this);
            AddDlg.ShowDialog();            
        }

        public void addWord(string word){
            listBoxWords.Items.Add(word);
            ch0day.keyWords.Add(word);
        }

        private void buttonRemoveWord_Click(object sender, EventArgs e)
        {
            if (listBoxWords.SelectedIndex >= 0)
            {
                ch0day.keyWords.Remove(listBoxWords.SelectedIndex.ToString());
                listBoxWords.Items.Remove(listBoxWords.SelectedItem);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ch0day.ActBeep = checkBox1.Checked; 
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            ch0day.ActBrowser = checkBox2.Checked;
        }

        private void timerSearch_Tick(object sender, EventArgs e)
        {
            if (progressCounter > 9)
            {
                progressCounter = 0;
                progressBarSearch.Value = 0;
            }
            else progressBarSearch.Value++;

            if (progressCounter==0)
            {
                if (ch0day.Check())
                {
                    listBox1.Items.Clear();
                    foreach (string str in ch0day.founded)
                    {
                        listBox1.Items.Add(str);
                    }
                }
            }
            

            progressCounter++;
        }
    }
}
