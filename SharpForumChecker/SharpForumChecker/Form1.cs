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

namespace SharpForumChecker
{
    public partial class Form1 : Form
    {
        public List<ISiteInterface> siteList;
        private Int16 UpdInterval;
        private Int16 UpdRandom;
        private Int16 UpdCounter;
        private Random rand;

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
            toolStripProgress.PerformStep();

            //             Checker0day Zday = new Checker0day();
            //             Zday.SiteName = "0day Авто Мото";
            //             Zday.Filter = textBoxKeyWords.Text;
            //             Zday.SiteUri = "http://forum.0day.kiev.ua/index.php?showforum=302";
            //             siteList.Add(Zday);

            //             CheckerOverlockers OvLck = new CheckerOverlockers();
            //             OvLck.SiteName = "Overclockers";
            //             OvLck.Filter = textBoxKeyWords.Text;
            //             OvLck.SiteUri = "http://forum.overclockers.ua/viewforum.php?f=26";
            //             siteList.Add(OvLck);

            //             CheckerAukro OvLck = new CheckerAukro();
            //             OvLck.SiteName = "Aukro";
            //             OvLck.Filter = textBoxKeyWords.Text;
            //             OvLck.SiteUri = "http://aukro.ua/listing/listing.php?string=телефон+нокиа&search_scope=все+разделы";
            //             siteList.Add(OvLck);

            //siteList[0].Checker();

            Task<Dictionary<string, string>>[] tasks = new Task<Dictionary<string, string>>[siteList.Count];
            for (int i = 0; i < siteList.Count; i++)
            {
                tasks[i] = Task<Dictionary<string, string>>.Factory.StartNew(siteList[i].Checker);
            }
            Task.WaitAll(tasks);

            foreach (var task in tasks)
            {
                Dictionary<string, string> resuts = task.Result;
                foreach (var result in resuts)
                {
                    listBox1.Items.Add(result.Key);
                }
            }
            //             
            //             
            // 
            //             Console.WriteLine("--------------------------------------------------------------------");
            //             Console.WriteLine("-------------А ТЕПЕР ЧИТАЄМО ЗБЕРЕЖЕНЕ------------------------------");
            //             Console.WriteLine("--------------------------------------------------------------------");
            // 
            //             List<ISiteInterface> sites = SitesIo.OpenBin();
            //             
            //             Console.ForegroundColor = ConsoleColor.Green;
            //             foreach (var site in sites)
            //             {
            //                 Console.WriteLine(site.SiteName + "\t" + site.Filter);
            //                 foreach (var s in site.TopicDictionary)
            //                 {
            //                     Console.WriteLine(s.Key + "\t" + s.Value);
            //                 }
            //                 Console.WriteLine(new string('_', 30) + "\n");
            //             }
            // 
            //             string useless = Console.ReadLine();
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

            LBSites.Items.Add(name);
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

            LBSites.Items[site_list_index] = name;
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
                LBSites.Items.Add(site.SiteName);
                foreach (var s in site.TopicDictionary)
                {
                    listBox1.Items.Add(s.Key);
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

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
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

        private void btnSiteRem_Click(object sender, EventArgs e)
        {
            if (LBSites.SelectedIndex == -1)
            {
                return;
            }

            siteList.RemoveAt(LBSites.SelectedIndex);
            LBSites.Items.RemoveAt(LBSites.SelectedIndex);

        }

        private void LBSites_DoubleClick(object sender, EventArgs e)
        {
            AddSite addSite = new AddSite(this);
            addSite.tbName.Text = siteList[LBSites.SelectedIndex].SiteName;
            addSite.tbKeywords.Text=siteList[LBSites.SelectedIndex].Filter;
            addSite.button3.Visible = true;
            addSite.site_list_index = LBSites.SelectedIndex;
            addSite.ShowDialog();
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
    }
}
