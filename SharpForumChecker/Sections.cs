using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpForumChecker.Resources
{
    class Sections
    {
        private static bool[]   arr   = new bool[7];
        private static string[] sites = new string[7];
        private List<string> blackList = new List<string>();  

        public Sections(){
            for (int i=0; i<7; i++)
            {
                arr[i] = false;
            }

            sites[0] = "http://forum.0day.kiev.ua/index.php?showforum=302";
            sites[1] = "http://forum.0day.kiev.ua/index.php?showforum=303";
            sites[2] = "http://forum.0day.kiev.ua/index.php?showforum=305";
            sites[3] = "http://forum.0day.kiev.ua/index.php?showforum=306";
            sites[4] = "http://forum.0day.kiev.ua/index.php?showforum=307";
            sites[5] = "http://forum.0day.kiev.ua/index.php?showforum=308";
            sites[6] = "http://forum.0day.kiev.ua/index.php?showforum=310";

        }    

        public void Setup(int index, bool value) {
            if (index < 0 || index > 6) return;

            arr[index] = value;
        }

        public bool GetIndex(int index)
        {
            return arr[index];
        }

        public string GetSite(int index)
        {
            return sites[index];
        }
    }
}
