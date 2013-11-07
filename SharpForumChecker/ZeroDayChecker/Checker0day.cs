using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace ZeroDayChecker
{
    public class Checker0day
    {
        private HtmlDocument _htmlDoc;
        private HtmlWeb _htmlWeb;

        private bool _checkResult;
        public bool MyProperty
        {
            get { return _checkResult; }
        }
        
        private StringBuilder _founded;
        public string Founded
        {
            get { var result = _founded.ToString(); _founded.Clear(); return result; }
        }
        
        private string _blackList;

        public Checker0day()
        {
            _checkResult = false;
            _founded = new StringBuilder();
            _blackList = "";

            _htmlDoc = new HtmlAgilityPack.HtmlDocument();
            _htmlWeb = new HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = System.Text.ASCIIEncoding.GetEncoding(1251),
            };
        }


        public void Check(string Keywords, string ThreadLink)
        {
            _checkResult = false;
            List<string> _keywords = new List<string>(Keywords.Split(','));

            try
            {
                _htmlDoc = _htmlWeb.Load(ThreadLink);
            }
            catch (System.Exception ex)
            {
                return; //TODO: убрать try-catch хай Жека єбеться з проблемами сєті
            }

            var spanList = _htmlDoc.DocumentNode.SelectNodes("//tr/td/div/span");
            foreach (var span in spanList)
            {
                if (span.InnerHtml.Contains("Тема создана:"))
                {
                    bool keyw = false;
                    foreach (string str in _keywords)
                    {
                        if (span.InnerText.Contains(str))
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

                            if (!_blackList.Contains(linkNubmer)) //якшо ще не реагував на такий номер топіка
                            {
                                _founded.Append(span.InnerText + "¼" + "http://forum.0day.kiev.ua/index.php?showtopic=" + linkNubmer + "|");
                                _checkResult = true;
                                _blackList += linkNubmer; //реагую і добавляю в блек ліст
                            }
                        }
                    }
                }
            }
        }

    }
}
