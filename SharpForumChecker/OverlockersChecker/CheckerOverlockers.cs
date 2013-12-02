using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using SiteMonitorInterface;
using System.Web;
using System.Text.RegularExpressions;

namespace OverlockersChecker
{
    [Serializable]
    public class CheckerOverlockers : ISiteInterface
    {
        public string SiteName { get; set; }
        public string Filter { get; set; }
        public string SiteUri { get; set; }
        public int UpdatesCount { get; set; }
        public bool JustAdded { get; set; }
        public Dictionary<string, string> TopicDictionary { get; set; }
        private List<string> _blackList;

        public CheckerOverlockers()
        {
            _blackList = new List<string>();
            TopicDictionary = new Dictionary<string, string>();
            UpdatesCount = 0;
            JustAdded = true;
        }

        public Dictionary<string, string> Checker()
        {
            UpdatesCount = 0;
            //TopicDictionary.Clear();

            HtmlDocument _htmlDoc = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb _htmlWeb = new HtmlWeb();

            List<string> _keywords = new List<string>(Filter.Split(','));

            try
            {
                _htmlDoc = _htmlWeb.Load(SiteUri);
            }
            catch (System.Exception ex)
            {
                return TopicDictionary; 
            }


            
            var aList = _htmlDoc.DocumentNode.SelectNodes("//tr/td[@class='row1']/a");
            if (aList == null)
            {
                return TopicDictionary;
            }
            foreach (var a in aList)
            {
                if (!_blackList.Contains(a.InnerText))
                {
                    _blackList.Add(a.InnerText);

                    bool keyw = false;
                    foreach (string str in _keywords)
                    {
                        RegexOptions option = RegexOptions.IgnoreCase;
                        Regex newReg = new Regex(@str, option);
                        MatchCollection matches = newReg.Matches(a.InnerText);

                        if (matches.Count > 0)
                        {
                            keyw = true;
                            break;
                        }
                    }
                    if (keyw)
                    {
                        string linkName = a.Attributes["href"].Value;
                        string linkNameFixed = linkName.Replace("&amp;", "&");
                        TopicDictionary[a.InnerText] = "http://forum.overclockers.ua/" + linkNameFixed;
                        UpdatesCount++;
                    }
                }
            }
            if (JustAdded)
            {
                JustAdded = false;
                UpdatesCount = 0;
            }
            return TopicDictionary;
        }
        
    }
}
