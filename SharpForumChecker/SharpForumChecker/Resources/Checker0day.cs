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
        System.IO.Stream SoundStream;
        public Resources.Sections mySect;
        public List<string> keyWords;
        public List<string> founded;
        private List<string> blackList;

        public SoundPlayer soundP;
        public bool ActBrowser;
        public bool ActBeep;
        
        public Checker0day() {
            mySect = new Resources.Sections();
            keyWords = new List<string>();
            founded = new List<string>();
            ActBeep = true;
            blackList = new List<string>();
            SoundStream = Properties.Resources.alert;
            soundP = new SoundPlayer(SoundStream);
        }

        [DllImport("shell32.dll")]
        private static extern int ShellExecute(int hWnd, string Operation, string File, string Parameters, string Directory, int nShowCmd);

        public bool Check()
        {
            bool resault = false;

            for (int i = 0; i < 7; i++)
            {
                if (mySect.GetIndex(i))
                {
                    resault = CheckIt(mySect.GetSite(i));
                }
            }

            if (resault && ActBeep)
            {
                soundP.Play();
            }

            return resault;
        }

        public bool CheckIt(string site){
            bool res = false;

            HtmlAgilityPack.HtmlDocument DOC = new HtmlAgilityPack.HtmlDocument();
            var web = new HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = System.Text.ASCIIEncoding.GetEncoding(1251),
            };
            DOC = web.Load(site);

            var trList = DOC.DocumentNode.SelectNodes("//tr");
            foreach (var tr in trList)
            {
               var tdList = tr.ChildNodes.Where(x => x.Name == "td");
               foreach (var td in tdList)
               {
                   var divList = td.ChildNodes.Where(x => x.Name == "div");
                   foreach (var div in divList)
                   {
                        var spanList = div.ChildNodes.Where(x => x.Name == "span");
                        foreach (var span in spanList)
                        {
                              if (span.InnerHtml.Contains("Тема создана:"))
                              {
                                  foreach (string word in keyWords)
                                  {
                                      if (span.InnerText.Contains(word))
                                      {
                                          var aList = span.ChildNodes.Where(x => x.Name == "a");
                                          foreach (var a in aList)
                                          {
                                              string temp_str = a.Attributes["href"].Value;
                                              string target_str = "";

                                              for (int ii = temp_str.LastIndexOf('=') + 1; ii < temp_str.Length; ii++)
                                              {
                                                  target_str += temp_str[ii].ToString();
                                              }

                                              if (blackList.Contains(target_str) == false)
                                              {
                                                  founded.Add(span.InnerText/*.Trim()*/);
                                                  res = true;

                                                  if (ActBrowser == true) ShellExecute(0, "open", "http://forum.0day.kiev.ua/index.php?showtopic=" + target_str, "", "", 1);

                                                  blackList.Add(target_str);
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
