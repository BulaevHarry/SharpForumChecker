using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using SiteMonitorInterface;

namespace AukroChecker
{
    [Serializable]
    public class CheckerAukro : ISiteInterface
    {
        public string SiteName { get; set; }
        public string Filter { get; set; }
        public string SiteUri { get; set; }
        public Dictionary<string, string> TopicDictionary { get; set; }
        private string _blackList;

        public CheckerAukro()
        {
            _blackList = "";
            TopicDictionary = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Checker()
        {
            HtmlDocument _htmlDoc = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb      _htmlWeb = new HtmlWeb
            {
                //AutoDetectEncoding = false,
                //OverrideEncoding = System.Text.ASCIIEncoding.GetEncoding(1251),
            };

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

            var spanList = _htmlDoc.DocumentNode.SelectNodes("//body/article");
            if (spanList == null)
            {
                return TopicDictionary;
            }
            foreach (HtmlNode span in spanList)
            {
                TopicDictionary[span.InnerHtml] = "";
            }
            return TopicDictionary;
        }
    }
}
