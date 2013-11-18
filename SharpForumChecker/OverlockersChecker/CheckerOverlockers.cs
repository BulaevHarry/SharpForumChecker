using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using SiteMonitorInterface;
using System.Web;

namespace OverlockersChecker
{
    [Serializable]
    public class CheckerOverlockers : ISiteInterface
    {
        public string SiteName { get; set; }
        public string Filter { get; set; }
        public string SiteUri { get; set; }
        public Dictionary<string, string> TopicDictionary { get; set; }
        private string _blackList;

        public CheckerOverlockers()
        {
            _blackList = "";
        }

        public Dictionary<string, string> Checker()
        {
            HtmlDocument _htmlDoc = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb _htmlWeb = new HtmlWeb();

            TopicDictionary = new Dictionary<string, string>();
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
            foreach (var a in aList)
            {
                bool keyw = false;
                foreach (string str in _keywords)
                {
                    if (a.InnerText.Contains(str))
                    {
                        keyw = true;
                    }
                }
                if (keyw)
                {
                    string linkName = a.Attributes["href"].Value;
                    string linkNameFixed = linkName.Replace("&amp;", "&");
                    if (!_blackList.Contains(linkNameFixed))
                    {
                        TopicDictionary[a.InnerText] = "http://forum.overclockers.ua/" + linkNameFixed;
                        _blackList += linkNameFixed;
                    }
                    
                }
                
            }
            return TopicDictionary;
        }
        
    }
}
