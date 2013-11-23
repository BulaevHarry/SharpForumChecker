using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using SiteMonitorInterface;
using ZeroDayChecker;
using OverlockersChecker;
using AukroChecker;
using System.IO;
using System.Runtime.InteropServices;

namespace SharpForumChecker
{
    public partial class Form1 : Form
    {
        public List<ISiteInterface> siteList;
        private Int16 UpdInterval;
        private Int16 UpdRandom;
        private Int16 UpdCounter;
        private Random rand;

        [DllImport("shell32.dll")]
        private static extern int ShellExecute(int hWnd, string Operation, string File, string Parameters, string Directory, int nShowCmd);

        public Form1()
        {
            InitializeComponent();
            siteList = new List<ISiteInterface>();
            rand = new Random();

            UpdInterval = 1;
            UpdRandom = 1;
            UpdCounter = 0;
        }

        private void performCheck()
        {
            /*listBox1.Items.Clear();*/
            foreach (TreeNode nod in treeView1.Nodes)
            {
                nod.Nodes.Clear();
            }
            toolStripProgress.PerformStep();
           
            Task<Dictionary<string, string>>[] tasks = new Task<Dictionary<string, string>>[siteList.Count];
            for (int i = 0; i < siteList.Count; i++)
            {
                tasks[i] = Task<Dictionary<string, string>>.Factory.StartNew(siteList[i].Checker);
            }
            Task.WaitAll(tasks);

            int foreach_iterator = 0;
            foreach (var task in tasks)
            {
                Dictionary<string, string> resuts = task.Result;
                foreach (var result in resuts)
                {
                    treeView1.Nodes[foreach_iterator].Nodes.Add(result.Key);
                }
                foreach_iterator++;
            }

        }

        public void changeSettings(int interval, int randomizer) 
        {
            UpdInterval = (Int16)interval;
            UpdRandom = (Int16)randomizer;

            timerSearch.Interval = UpdInterval * 600;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            performCheck();
        }

        public void addSiteIntoList(int site_type_index, string name, string url, string keys)
        {
            ISiteInterface site;
            switch (site_type_index)
            {
                case 0: site = new Checker0day(); break;
                case 1: site = new CheckerOverlockers(); break;
                default: return;
            }
            
            site.SiteName = name;
            site.Filter = keys;
            site.SiteUri = url;
            siteList.Add(site);

            treeView1.Nodes.Add(name);
            treeView1.Nodes[treeView1.Nodes.Count - 1].BackColor = Color.Wheat;

        }

        public void editSiteInTheList(int site_type_index, int site_list_index, string name, string url, string keys)
        {
            
            ISiteInterface site;
            switch (site_type_index)
            {
                case 0: site = new Checker0day(); break;
                case 1: site = new CheckerOverlockers(); break;
                default: return;
            }

            site.SiteName = name;
            site.Filter = keys;
            site.SiteUri = url;
            siteList[site_list_index] = site;

            treeView1.Nodes[site_list_index].Text = name; 
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddSite addSite = new AddSite(this);
            addSite.ShowDialog();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (siteList.Count == 0)
            {
                return;
            }
            SitesIo.SaveToBin(siteList);
           
            try
            {
                BinaryWriter bw = new BinaryWriter(File.Open(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\settings.cfg", FileMode.Create));
                bw.Write(UpdInterval);
                bw.Write(UpdRandom);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Не удалось сохранить настройки.");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            siteList = SitesIo.OpenBin();
            foreach (var site in siteList)
            {
                treeView1.Nodes.Add(site.SiteName);
                treeView1.Nodes[treeView1.Nodes.Count - 1].BackColor = Color.Wheat;
                foreach (var s in site.TopicDictionary)
                {
                    treeView1.Nodes[treeView1.Nodes.Count-1].Nodes.Add(s.Key);
                }
            }

            try
            {
                BinaryReader br = new BinaryReader(File.Open(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath)+"\\settings.cfg", FileMode.Open));
                UpdInterval  = br.ReadInt16();
                UpdRandom    = br.ReadInt16();
                timerSearch.Interval = UpdInterval * 600;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Не удалось загрузить настройки.");
            }
            
        }

        private void timerSearch_Tick(object sender, EventArgs e)
        {
            if (UpdCounter==0)
            {
                performCheck();

                int tmp_interv = (UpdInterval + (rand.Next(-UpdRandom, +UpdRandom)) / 2) * 600;
                if (tmp_interv < 600) { tmp_interv = 600; }

                timerSearch.Interval = tmp_interv;
                
            }

            UpdCounter++;
            toolStripProgress.PerformStep();
            if (UpdCounter > 99) { UpdCounter = 0; toolStripProgress.Value = 0; }
            
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            toolStripProgress.Value = 0;
            UpdCounter = 0;
            performCheck();
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings set = new Settings(this,UpdInterval,UpdRandom);
            set.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Index < 0)
            {
                return;
            }

            siteList.RemoveAt(treeView1.SelectedNode.Index);
            treeView1.SelectedNode.Remove();
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            
            if (treeView1.SelectedNode.Level == 0)
            {
                
            }

            if (treeView1.SelectedNode.Level == 1)
            {
                int lv0i = treeView1.SelectedNode.Parent.Index;
                int lv1i = treeView1.SelectedNode.Index;
                string lv1t = treeView1.SelectedNode.Text;
                openUrlInBrowser(siteList[lv0i].TopicDictionary[lv1t]);

            }

        }

        private void openUrlInBrowser(string link)
        {
            ShellExecute(0, "open", link, "", "", 1);
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Select the clicked node
                treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
                if (treeView1.SelectedNode.Level == 1) { return; }

                if (/*(treeView1.SelectedNode != null && treeView1.SelectedNode.Parent == null) || */ true)
                {
                    contextMenuStrip1.Show(treeView1, e.Location);
                }
            }
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode.Collapse();
            AddSite addSite = new AddSite(this);
            addSite.Text = "Редактировать сайт";
            addSite.selectEditedInLists(siteList[treeView1.SelectedNode.Index].SiteUri);
            addSite.tbName.Text = siteList[treeView1.SelectedNode.Index].SiteName;
            addSite.tbKeywords.Text = siteList[treeView1.SelectedNode.Index].Filter;
            addSite.button3.Visible = true;
            addSite.site_list_index = treeView1.SelectedNode.Index;
            addSite.ShowDialog();
        }

    }
}
