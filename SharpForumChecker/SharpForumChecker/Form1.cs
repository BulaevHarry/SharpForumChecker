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

namespace SharpForumChecker
{
    public partial class Form1 : Form
    {
        public List<ISiteInterface> siteList;

        public Form1()
        {
            InitializeComponent();
            siteList = new List<ISiteInterface>();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
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
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void timerSearch_Tick(object sender, EventArgs e)
        {

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
    }
}
