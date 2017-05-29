using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace WindowsService1
{
    class Library
    {
        public static void writeErrorLog(Exception ex) {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\log.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ":" + ex.Source.ToString().Trim() + ":" + ex.Message.ToString());
                sw.Flush();
                sw.Close();
            }
            catch{ 

            }
        }
    }
}
