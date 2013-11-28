using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SiteMonitorInterface
{
    public interface ISiteInterface
    {
        string SiteName { get; set; }
        string Filter { get; set; }
        string SiteUri { get; set; }
        int UpdatesCount { get; set; }
        bool JustAdded { get; set; }
        Dictionary<string, string> TopicDictionary { get; set; } 

        Dictionary<string, string> Checker();
    }

    public static class SitesIo
    {
        public static void SaveToBin(List<ISiteInterface> siteList, string directory_name)
        {
            FileStream fs = new FileStream(directory_name + "\\sites.dat", FileMode.Create);

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

        public static void SaveToXml(List<ISiteInterface> sitelList)
        {
            // think about XML... for what? o_0
        }

        public static List<ISiteInterface> OpenBin(string directory_name)
        {
            List<ISiteInterface> sites;

            try
            {
                using (FileStream fs = new FileStream(directory_name + "\\sites.dat", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    sites = (List<ISiteInterface>)formatter.Deserialize(fs);
                }
            }
            catch (System.Exception ex)
            {
                sites = new List<ISiteInterface>();
            };
            
            return sites;
        }

        public static List<ISiteInterface> OpenXml()
        {
            return new List<ISiteInterface>();
        }

    }
}
