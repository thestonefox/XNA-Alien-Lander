using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ContentPipelineExtension
{
    /// <summary>
    /// Log Writer
    /// </summary>
    public static class LogWriter
    {
        /// <summary>
        /// Method to write to log file.
        /// </summary>
        /// <param name="data"></param>
        public static void WriteToLog(string data)
        {
            StreamWriter sw = new StreamWriter("BlacksunContentPipeline.log", true);
            sw.WriteLine(string.Format("{0} - {1}", DateString(DateTime.Now), data));
            sw.Close();
        }
        /// <summary>
        /// Retruns date time in the following format DD/MM/YYYY HH:MM:SS:mm
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateString(DateTime dt)
        {
            return string.Format("{0:00}/{1:00}/{2:0000} {3:00}:{4:00}:{5:00}.{6:00}", dt.Day, dt.Month, dt.Year, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }
    }
}