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
            List<ISiteInterface> siteList = new List<ISiteInterface>(); // список сайтів


            //створили один сайт -- 0day
            Checker0day Zday = new Checker0day();
            Zday.SiteName = "0day Авто Мото";
            Zday.Filter = "Продам";
            Zday.SiteUri = "http://forum.0day.kiev.ua/index.php?showforum=302";
            siteList.Add(Zday);

            Checker0day Zday2 = new Checker0day();
            Zday2.SiteName = "0day Компи";
            Zday2.Filter = "Продам";
            Zday2.SiteUri = "http://forum.0day.kiev.ua/index.php?showforum=303";
            siteList.Add(Zday2);
            

            Task<Dictionary<string, string>>[] tasks = new Task<Dictionary<string, string>>[2]; // порахували скільки елементів в списку --- ListView.Count; ---  у нас зараз 2 елемента. Для кожного хуярим свій асинхронний поток
            for (int i = 0; i < 2; i++)
            {
                tasks[i] = Task<Dictionary<string, string>>.Factory.StartNew(siteList[i].Checker); // кожного запускаэмо метод в якому виконується вся логіка
            }
            Task.WaitAll(tasks); // чекаємо завершення задач-потоків

            // виводимо кудись всю цю муть гуі чи ще куди
            foreach (var task in tasks)
            {
                Dictionary<string, string> res = task.Result;
                Console.ForegroundColor = ConsoleColor.Cyan;
                foreach (var re in res)
                {
                    Console.WriteLine(Zday.SiteName + "\t" + Zday.Filter); //отут у тебе трохи косячок, через нього виводить назву теми ы ключове слово з 1 класа (Славон)<---------=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
                    Console.WriteLine(re.Key + "\t" + re.Value);
                    Console.WriteLine(new string('_', 30) + "\n");
                }
            }
            // закрили прогу і зберігли
            SitesIo.SaveToBin(siteList);
            ///////////////////////////

            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine("-------------А ТЕПЕР ЧИТАЄМО ЗБЕРЕЖЕНЕ------------------------------");
            Console.WriteLine("--------------------------------------------------------------------");

            // відкрили прогу і прочитали з файлу
            List<ISiteInterface> sites = SitesIo.OpenBin();
            // вивели
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
