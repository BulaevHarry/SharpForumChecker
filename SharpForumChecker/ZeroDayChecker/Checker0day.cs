﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using SiteMonitorInterface;

namespace ZeroDayChecker
{
    [Serializable] 
    public class Checker0day : ISiteInterface
    {
        public string SiteName { get; set; }
        public string Filter { get; set; }
        public string SiteUri { get; set; }
        public int UpdatesCount { get; set; }
        public bool JustAdded { get; set; }
        public Dictionary<string, string> TopicDictionary { get; set; }

        private List<string> _blackList;

        public Checker0day()
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
                AutoDetectEncoding = false,
                OverrideEncoding = System.Text.ASCIIEncoding.GetEncoding(1251),
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

            var spanList = _htmlDoc.DocumentNode.SelectNodes("//tr/td/div/span");
            if (spanList == null) { return TopicDictionary; }
            
            foreach (HtmlNode span in spanList)
            {
                if (span.InnerHtml.Contains("Тема создана:"))
                {
                    string topicText = span.InnerText.Replace("&amp;","&"); 
                    bool keyw = false;
                    foreach (string str in _keywords)
                    {
                        if (topicText.Contains(str))
                        {
                            keyw = true;
                        }
                    }
                    if (keyw)
                    {
                        var aList = span.ChildNodes.Where(x => x.Name == "a"); //витягую номер топіка
                        foreach (var a in aList)
                        {
                            string temp_str = a.Attributes["href"].Value; //тут якась не дуже робоча ссилка в кінці якої наш номер
                            string linkNubmer = "";

                            for (int ii = temp_str.LastIndexOf('=') + 1; ii < temp_str.Length; ii++)
                            {
                                linkNubmer += temp_str[ii].ToString(); //а тепер в лінкНамбер наш номер топіка
                            }

                            if (!_blackList.Contains(linkNubmer)) 
                            {
                                TopicDictionary[topicText] = "http://forum.0day.kiev.ua/index.php?showtopic=" + linkNubmer;
                                _blackList.Add(linkNubmer);
                                UpdatesCount++;
                            }
                        }
                    }
                }
            }
            if (JustAdded)
            {
                JustAdded = false;
                UpdatesCount = 0;
            }
            UpdatesCount = 1;
            return TopicDictionary;
        }
    }
}
