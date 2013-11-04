using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using HtmlAgilityPack;
using System.Media;

namespace SharpForumChecker.Resources
{
    class Checker0day
    {
        public Resources.Sections sections; //ссилки на сайти, де шукаємо
        public List<string> keyWords; //список ключових слов
        public List<string> founded;  //список всього найденого (назви тем)
        private List<string> blackList; //"чорний список" куди заносяться останні цифри в ссилкі на тему

        System.IO.Stream SoundStream; //для витягування вавки з ресурсів
        private SoundPlayer soundP;   //для проігрування цеї вавки

        public bool ActBrowser; //настройки реакції на знайдене
        public bool ActBeep;
        
        public Checker0day() {          //ініціалізація
            sections = new Resources.Sections();
            keyWords = new List<string>();
            founded = new List<string>();
            ActBeep = true;
            blackList = new List<string>();
            SoundStream = Properties.Resources.alert;
            soundP = new SoundPlayer(SoundStream);
        }

        [DllImport("shell32.dll")] //для відкриття сторінки в браузері через winapi, було впадляк гуглить як воно у вас, шарперів, робиться
        private static extern int ShellExecute(int hWnd, string Operation, string File, string Parameters, string Directory, int nShowCmd);

        public bool Check()
        {
            bool result = false; 

            for (int i = 0; i < 7; i++)
            {
                if (sections.GetIndex(i))
                {
                    if (CheckIt(sections.GetSite(i)))
                    {
                        result = true; //якшо хоч раз шось знайшлося
                    }
                    
                }
            }

            if (result && ActBeep)
            {
                soundP.Play();
            }

            return result;
        }

        private bool CheckIt(string site){
            bool res = false;

            HtmlAgilityPack.HtmlDocument DOC = new HtmlAgilityPack.HtmlDocument(); //я хз треба чи не треба кожен раз пересоздавать цей екзкмпляр. шо ти думаєш, Круел?
            var web = new HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = System.Text.ASCIIEncoding.GetEncoding(1251), //дякую за цю подсказку, Круел.
            };

            DOC = web.Load(site);
            if (DOC == null)
            {
                return false; 
            }


            //і тут я починаю розгрібать html'ку.. розбирав як умію, якшо є кращий спосіб - крітікуй
            var trList = DOC.DocumentNode.SelectNodes("//tr");
            foreach (var tr in trList) //забавна штука цей форіч
            {
               var tdList = tr.ChildNodes.Where(x => x.Name == "td");
               foreach (var td in tdList)
               {
                   var divList = td.ChildNodes.Where(x => x.Name == "div");
                   foreach (var div in divList)
                   {
                        var spanList = div.ChildNodes.Where(x => x.Name == "span"); //тут у нас в тексті назва теми
                        foreach (var span in spanList)
                        {
                              if (span.InnerHtml.Contains("Тема создана:")) //а так я вирішив виділить тільки спани з темами
                              {
                                  foreach (string word in keyWords) //перевіряю чи є ключові слова в назві теми
                                  {
                                      if (span.InnerText.Contains(word))
                                      {
                                          var aList = span.ChildNodes.Where(x => x.Name == "a"); //витягую номер топіка
                                          foreach (var a in aList)
                                          {
                                              string temp_str = a.Attributes["href"].Value; //тут якась не дуже робоча ссилка в кінці якої наш номер
                                              string target_str = "";

                                              for (int ii = temp_str.LastIndexOf('=') + 1; ii < temp_str.Length; ii++)
                                              {
                                                  target_str += temp_str[ii].ToString();
                                              } //а тепер в таргет_стр наш номер топіка

                                              if (blackList.Contains(target_str) == false) //якшо ще не реагував на такий номер топіка
                                              {
                                                  founded.Add(span.InnerText/*.Trim()*/);
                                                  res = true;//наче як все нормально, а значить в кінці вернемо "правду"

                                                  if (ActBrowser == true) ShellExecute(0, "open", "http://forum.0day.kiev.ua/index.php?showtopic=" + target_str, "", "", 1);//открить в браузері

                                                  blackList.Add(target_str); //реагую і добавляю в блек ліст
                                              }

                                          }
                                      }
                                  }
                              }
                        }
                   }
               }
            }

            return res;
        }
        
    }
}
