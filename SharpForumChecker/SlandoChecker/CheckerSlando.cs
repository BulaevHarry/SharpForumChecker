using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using SiteMonitorInterface;
using System.Text.RegularExpressions;

namespace SlandoChecker
{
    [Serializable]
    public class CheckerSlando : ISiteInterface
    {
        public string SiteName { get; set; }
        public string Filter { get; set; }
        public string SiteUri { get; set; }
        public int UpdatesCount { get; set; }
        public bool JustAdded { get; set; }
        public Dictionary<string, string> TopicDictionary { get; set; }

        private List<string> _blackList;

        public CheckerSlando()
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
            HtmlWeb      _htmlWeb = new HtmlWeb
            {
                //AutoDetectEncoding = false,
                //OverrideEncoding = System.Text.ASCIIEncoding.GetEncoding(1251),
            };

            List<string> _keywords = new List<string>(Filter.Split(','));

            try
            {
                _htmlDoc = _htmlWeb.Load(SiteUri);
            }
            catch (System.Exception ex)
            {
                return TopicDictionary; 
            }

            HtmlNodeCollection aList = _htmlDoc.DocumentNode.SelectNodes("//tr/td/h3/a");
            if (aList == null) { return TopicDictionary; }
            
            foreach (HtmlNode a in aList)
            {
                var spanList = a.ChildNodes.Where(x => x.Name == "span");
                foreach (HtmlNode span in spanList)
                {
                    bool keyw = false;
                    foreach (string str in _keywords)
                    {
                        RegexOptions option = RegexOptions.IgnoreCase;
                        Regex newReg = new Regex(@str, option);
                        MatchCollection matches = newReg.Matches(span.InnerText);

                        if (matches.Count > 0)
                        {
                            keyw = true;
                            break;
                        }
                    }
                    if (keyw)
                    {
                        if (!_blackList.Contains(span.InnerText))
                        {
                            TopicDictionary[span.InnerText] = a.Attributes["href"].Value;
                            _blackList.Add(span.InnerText);
                            UpdatesCount++;
                        }
                    }
                }
                
                
            }
            if (JustAdded)
            {
                JustAdded = false;
                UpdatesCount = 0;
            }
            //UpdatesCount = 1; //для теста
            return TopicDictionary;
        }
    }
}
