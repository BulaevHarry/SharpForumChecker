using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SiteMonitorInterface
{
    public interface SiteInterface
    {
        string SiteName { get; set; } // site Name
        string Filter { get; set; } // key Words
        string SiteUri { get; set; } // site uri
        Dictionary<string, string> TopicDictionary { get; set; } //dict of topics (topicname - url) || (topicname - url)

        Dictionary<string, string> Checker(); // for async threds --- у кожного класа сайта своя реалізація
    }

    // допоміжниій клас для зберігання і відкривання списку сутністей всіх сайтів (зберігаються )
    public static class SitesIo
    {
        public static void SaveToBin(List<SiteInterface> siteList)
        {
            FileStream fs = new FileStream("sites.dat", FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, siteList);
            }
            catch (SerializationException e)
            {
                //MessageBox.Show(e.Message, "Error");
                Console.WriteLine(e.Message);
            }
            finally
            {
                fs.Close();
            }
        }

        public static void SaveToXml(List<SiteInterface> sitelList)
        {
            // think about XML... for what? o_0
        }

        public static List<SiteInterface> OpenBin()
        {
            List<SiteInterface> sites;
            using (FileStream fs = new FileStream("sites.dat", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                sites = (List<SiteInterface>)formatter.Deserialize(fs);
            }
            return sites;
        }

        public static List<SiteInterface> OpenXml()
        {
            return new List<SiteInterface>();
        }

    }
}
