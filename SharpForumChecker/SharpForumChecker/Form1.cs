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
            List<ISiteInterface> siteList = new List<ISiteInterface>();

            Checker0day Zday = new Checker0day();
            Zday.SiteName = "0day Авто Мото";
            Zday.Filter = "Продам";
            Zday.SiteUri = "http://forum.0day.kiev.ua/index.php?showforum=302";
            siteList.Add(Zday);

            CheckerOverlockers OvLck = new CheckerOverlockers();
            OvLck.SiteName = "Overclockers";
            OvLck.Filter = "Продам";
            OvLck.SiteUri = "http://forum.overclockers.ua/viewforum.php?f=26";
            siteList.Add(OvLck);

            Task<Dictionary<string, string>>[] tasks = new Task<Dictionary<string, string>>[2]; 
            for (int i = 0; i < 2; i++)
            {
                tasks[i] = Task<Dictionary<string, string>>.Factory.StartNew(siteList[i].Checker); 
            }
            Task.WaitAll(tasks); 

            foreach (var task in tasks)
            {
                Dictionary<string, string> resuts = task.Result;
                foreach (var result in resuts)
                {
                    listBox1.Items.Add(result.Key + "\t" + result.Value);
                    listBox1.Items.Add(new string('-', 350) + "\n");
                }
            }
            
            SitesIo.SaveToBin(siteList);

            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine("-------------А ТЕПЕР ЧИТАЄМО ЗБЕРЕЖЕНЕ------------------------------");
            Console.WriteLine("--------------------------------------------------------------------");

            List<ISiteInterface> sites = SitesIo.OpenBin();
            
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var site in sites)
            {
                Console.WriteLine(site.SiteName + "\t" + site.Filter);
                foreach (var s in site.TopicDictionary)
                {
                    Console.WriteLine(s.Key + "\t" + s.Value);
                }
                Console.WriteLine(new string('_', 30) + "\n");
            }

            string useless = Console.ReadLine();
        }
    }
}
