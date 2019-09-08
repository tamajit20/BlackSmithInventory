using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlackSmith
{
    public static class Utility
    {

        public static void WriteLog(Exception ex)
        {
            if (ex != null) {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Log", DateTime.Now.Date.ToShortDateString(), ".log");
                path = @"D:\BlackSmithWeb\Log\log.txt";

                string error = Environment.NewLine + DateTime.Now +
                    "--------------------" + ex.Message + Environment.NewLine + ex.StackTrace +
                    Environment.NewLine + ex.InnerException + "-------------------------"
                    + Environment.NewLine;

                File.AppendAllText(path, error);
            }
        }
    }
}
