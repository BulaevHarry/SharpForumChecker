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
using SlandoChecker;
using System.IO;
using System.Runtime.InteropServices;
using System.Media;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;

namespace SharpForumChecker
{
    public partial class Form1 : Form
    {
        public List<ISiteInterface> siteList;
        private Int16 UpdInterval;
        private Int16 UpdRandom;
        private Int16 UpdCounter;
        private bool EnableSoundAlert;
        private string SoundFileName;
        private bool EnableOpenInBrowser;
        private bool EnableSendMail;
        private bool MinimizeOnLaunch;
        private bool MinimizeToTray;
        private string EMailAdress;
        private Random rand;
        private SoundPlayer AlertPlayer;

        public Form1()
        {
            InitializeComponent();
            siteList = new List<ISiteInterface>();
            rand = new Random();

            UpdInterval = 1;
            UpdRandom = 1;
            UpdCounter = 0;

            EnableSoundAlert = true;
            SoundFileName = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\Audio\\alert.wav";
            AlertPlayer = new SoundPlayer(SoundFileName);

            EnableOpenInBrowser = false;

            EnableSendMail = false;
            EMailAdress = "address@mailer";

            MinimizeOnLaunch = false;
            MinimizeToTray = false;
        }

    #region реакции на найденое
    #region открытие в браузере
        [DllImport("shell32.dll")]
        private static extern int ShellExecute(int hWnd, string Operation, string File, string Parameters, string Directory, int nShowCmd);
        private void openUrlInBrowser(string link)
        {
            ShellExecute(0, "open", link, "", "", 1);
        }
    #endregion
    #region проигрывание звука
        private void playAlert()
        {
            try
            {
                AlertPlayer.Play();
            }
            catch (System.Exception ex)
            {
                AlertPlayer = new SoundPlayer(Properties.Resources.alert);
                AlertPlayer.Play();
            }
        }
    #endregion
    #region отправка уведомления на почту
        private void sendMailNotification(List<string> captions, List<string> links)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential("robot.forum.checker@gmail.com", "1q2w3e4r5t6y7u8");
            client.EnableSsl = true;
            client.Timeout = 10000;

            string from = "robot.forum.checker@gmail.com";
            string to = EMailAdress;
            string subject = "ForumChecker notification";
            string body = "Привет, анонимус! \nПриложение ForumChecker нашло для тебя новые интересности!\n\n";

            for (int i = 0; i < captions.Count; i++)
            {
                body += captions[i] + "\n" + links[i] + "\n\n";
            }

            MailMessage mess = new MailMessage(from, to, subject, body);
            try
            {
                client.Send(mess);
            }
            catch (System.Exception ex)
            {
                System.Media.SystemSounds.Exclamation.Play();
            }
            
        }
    #endregion
    #endregion
        
    #region обновление информации
        private void performCheck()
        {
            toolStripLabelUpdating.Visible = true;
            foreach (TreeNode nod in treeSearches.Nodes)
            {
                nod.Nodes.Clear();
            }
            
            Task<Dictionary<string, string>>[] tasks = new Task<Dictionary<string, string>>[siteList.Count];
            for (int i = 0; i < siteList.Count; i++)
            {
                tasks[i] = Task<Dictionary<string, string>>.Factory.StartNew(siteList[i].Checker);
            }
            Task.WaitAll(tasks);
            
            toolStripLabelUpdating.Visible = false;

            int foreach_iterator = 0;
            foreach (var task in tasks)
            {
                Dictionary<string, string> results = task.Result;
                foreach (var result in results)
                {
                    treeSearches.Nodes[foreach_iterator].Nodes.Insert(0, result.Key);//Add(result.Key);
                }
                foreach_iterator++;
            }

            //ковиряння в вусі через жопу
            bool IShouldDoSomething = false;
            List<string> s_names = new List<string>();
            List<string> s_links = new List<string>();

            for (int s = 0; s < siteList.Count; s++)
            {
                if (siteList[s].UpdatesCount > 0) IShouldDoSomething=true;

                //for (int n = treeView1.Nodes[s].Nodes.Count - siteList[s].UpdatesCount; n < treeView1.Nodes[s].Nodes.Count; n++)
                for (int n = 0; n < siteList[s].UpdatesCount; n++)
                {
                    if(EnableOpenInBrowser) openUrlInBrowser(siteList[s].TopicDictionary[treeSearches.Nodes[s].Nodes[n].Text]);
                    s_names.Add(treeSearches.Nodes[s].Nodes[n].Text);
                    s_links.Add(siteList[s].TopicDictionary[treeSearches.Nodes[s].Nodes[n].Text]);
                    treeSearches.Nodes[s].Nodes[n].BackColor = Color.LightGreen;
                }
                siteList[s].UpdatesCount = 0;
            }
            if (EnableSoundAlert && IShouldDoSomething)
            {
                playAlert();
            }
            if (EnableSendMail && IShouldDoSomething)
            {
                sendMailNotification(s_names, s_links);
            }

        }
        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            UpdCounter = 1;
            toolStripProgress.Value = 0;
            toolStripProgress.PerformStep();
            performCheck();
        }
        private void timerSearch_Tick(object sender, EventArgs e)
        {
            if (UpdCounter == 0)
            {
                performCheck();

                int tmp_interv = (int)(UpdInterval + (rand.Next(-UpdRandom, +UpdRandom)) / 2.0) * 600;
                if (tmp_interv < 600) { tmp_interv = 600; }

                timerSearch.Interval = tmp_interv;

            }

            UpdCounter++;
            toolStripProgress.PerformStep();
            if (UpdCounter > 99) { UpdCounter = 0; toolStripProgress.Value = 0; }

        }
        private void toolStripSplitBtnPlayPause_ButtonClick(object sender, EventArgs e)
        {
            if (timerSearch.Enabled)
            {
                toolStripSplitBtnPlayPause.Text = "Продолжить поиски";
                toolStripSplitBtnPlayPause.Image = Properties.Resources.play;
                timerSearch.Enabled = false;
            }
            else
            {
                toolStripSplitBtnPlayPause.Text = "Приостановить поиски";
                toolStripSplitBtnPlayPause.Image = Properties.Resources.pause;
                timerSearch.Enabled = true;
            }

        }
    #endregion

    #region открытие и закрытие приложения
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //зберігаю список сайтів
//             if (siteList.Count == 0)
//             {
//                 return;
//             }
            SitesIo.SaveToBin(siteList, Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath));

            

            notifyIcon.Visible = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //подтягую ліст сайтів
            siteList = SitesIo.OpenBin(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath));
            foreach (var site in siteList)
            {
                treeSearches.Nodes.Add(site.SiteName);
                treeSearches.Nodes[treeSearches.Nodes.Count - 1].BackColor = Color.Wheat;
                foreach (var s in site.TopicDictionary)
                {
                    treeSearches.Nodes[treeSearches.Nodes.Count - 1].Nodes.Add(s.Key);
                }
                //site.JustAdded = true; /*ТИМЧАСОВИЙ КОСТИЛЬ*/
            }

            //подтягую настройки
            try
            {
                BinaryReader br = new BinaryReader(File.Open(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\settings.cfg", FileMode.Open));
                UpdInterval             = br.ReadInt16();
                UpdRandom               = br.ReadInt16();
                EnableSoundAlert        = br.ReadBoolean();
                SoundFileName           = br.ReadString();
                EnableOpenInBrowser     = br.ReadBoolean();
                EnableSendMail          = br.ReadBoolean();
                EMailAdress             = br.ReadString();
                MinimizeOnLaunch        = br.ReadBoolean();
                MinimizeToTray          = br.ReadBoolean();

                timerSearch.Interval = UpdInterval * 600;
                AlertPlayer = new SoundPlayer(SoundFileName);
                br.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Не удалось загрузить настройки.");
            }

            bool onlyInstance;
            System.Threading.Mutex mtx = new System.Threading.Mutex(true, "AppName", out onlyInstance);
            if (!onlyInstance)
            {
                MessageBox.Show("Запуск второй копии приложения ForumChecker недопустим!");
                this.Close();
            }

            if (MinimizeOnLaunch)
            {
                this.WindowState = FormWindowState.Minimized;
                Form1_Resize(sender,e);
            }
        }
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    #endregion

    #region работа с списком (добавление,редактирование,удаление)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddSite addSite = new AddSite(this);
            addSite.ShowDialog();
        }
        public void addSiteIntoList(int site_type_index, string name, string url, string keys)
        {
            ISiteInterface site;
            switch (site_type_index)
            {
                case 0: site = new Checker0day(); break;
                case 1: site = new CheckerOverlockers(); break;
                case 2: site = new CheckerSlando(); break;
                default: return;
            }

            site.SiteName = name;
            site.Filter = keys;
            site.SiteUri = url;
            siteList.Add(site);

            treeSearches.Nodes.Add(name);
            treeSearches.Nodes[treeSearches.Nodes.Count - 1].BackColor = Color.Wheat;

        }
    
        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSite addSite = new AddSite(this);
            addSite.Text = "Монитор объявлений - Изменить поиск";
            addSite.btnEditSearch.Visible = true;
            addSite.SiteListIndex = treeSearches.SelectedNode.Index;

            //вибір вкладки
            if (siteList[treeSearches.SelectedNode.Index].GetType().ToString() == "SlandoChecker.CheckerSlando")
            {
                addSite.tabControl1.SelectedIndex = 2;
                addSite.tbSlandoKeys.Text = siteList[treeSearches.SelectedNode.Index].Filter;
            }
            else
                if (siteList[treeSearches.SelectedNode.Index].GetType().ToString() == "OverlockersChecker.CheckerOverlockers")
                {
                    addSite.tabControl1.SelectedIndex = 1;
                    addSite.tbOvKeys.Text = siteList[treeSearches.SelectedNode.Index].Filter;
                    addSite.selectOverclockers(siteList[treeSearches.SelectedNode.Index].SiteUri);
                }
                else
                {
                    addSite.tabControl1.SelectedIndex = 0;
                    addSite.tb0dayKeys.Text = siteList[treeSearches.SelectedNode.Index].Filter;
                    addSite.select0day(siteList[treeSearches.SelectedNode.Index].SiteUri);
                }

            //addSite.tabControl1_SelectedIndexChanged(sender, e);

            addSite.ShowDialog();
        }
        public void editSiteInTheList(int site_list_index, int site_type_index, string name, string url, string keys)
        {
            ISiteInterface site;
            switch (site_type_index)
            {
                case 0: site = new Checker0day(); break;
                case 1: site = new CheckerOverlockers(); break;
                case 2: site = new CheckerSlando(); break; 
                default: return;
            }

            site.SiteName = name;
            site.Filter = keys;
            site.SiteUri = url;
            siteList[site_list_index] = site;

            treeSearches.Nodes[site_list_index].Text = name;
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeSearches.SelectedNode.Index < 0)
            {
                return;
            }

            siteList.RemoveAt(treeSearches.SelectedNode.Index);
            treeSearches.SelectedNode.Remove();
        }
    #endregion

    #region работа с окном настрoек
        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings set = new Settings(this, UpdInterval, UpdRandom, EnableSoundAlert, SoundFileName, EnableOpenInBrowser, EnableSendMail, EMailAdress, MinimizeOnLaunch, MinimizeToTray);
            set.ShowDialog();
        }
        public void changeSettings(int interval, int randomizer, bool enableSound, string soundFN, bool enableOpenInBrowser, bool enableSendMail, string E_addr, bool minOnLaunch, bool minToTray)
        {
            UpdInterval = (Int16)interval;
            UpdRandom = (Int16)randomizer;
            EnableSoundAlert = enableSound;
            SoundFileName = soundFN;
            EnableOpenInBrowser = enableOpenInBrowser;
            EnableSendMail = enableSendMail;
            EMailAdress = E_addr;
            MinimizeOnLaunch = minOnLaunch;
            MinimizeToTray = minToTray;

            AlertPlayer = new SoundPlayer(SoundFileName);
            timerSearch.Interval = UpdInterval * 600;

            //зберігаю налаштування
            try
            {
                BinaryWriter bw = new BinaryWriter(File.Open(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\settings.cfg", FileMode.Create));
                bw.Write(UpdInterval);
                bw.Write(UpdRandom);
                bw.Write(EnableSoundAlert);
                bw.Write(SoundFileName);
                bw.Write(EnableOpenInBrowser);
                bw.Write(EnableSendMail);
                bw.Write(EMailAdress);
                bw.Write(MinimizeOnLaunch);
                bw.Write(MinimizeToTray);
                bw.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Не удалось сохранить настройки.");
            }
        }
    #endregion
        
    #region работа с древовидным списком
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {

            if (treeSearches.SelectedNode.Level == 0)
            {

            }

            if (treeSearches.SelectedNode.Level == 1)
            {
                int lv0i = treeSearches.SelectedNode.Parent.Index;
                int lv1i = treeSearches.SelectedNode.Index;
                string lv1t = treeSearches.SelectedNode.Text;
                openUrlInBrowser(siteList[lv0i].TopicDictionary[lv1t]);
                //MessageBox.Show(siteList[lv0i].TopicDictionary[lv1t]);
            }

        }
        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Select the clicked node
                treeSearches.SelectedNode = treeSearches.GetNodeAt(e.X, e.Y);
                if (treeSearches.SelectedNode.Level == 1) { return; }

                if (/*(treeView1.SelectedNode != null && treeView1.SelectedNode.Parent == null) || */ true)
                {
                    contextMenuStrip1.Show(treeSearches, e.Location);
                }
            }
        }

        private void открытьВБраузереToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openUrlInBrowser(siteList[treeSearches.SelectedNode.Index].SiteUri);
        }
    #endregion

    #region иконка в трее
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized && MinimizeToTray)
            {
                this.Hide();
                this.ShowInTaskbar = false;
                notifyIcon.Visible = true;
            }
        }
        private void notifyIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }
    #endregion

    }
}
