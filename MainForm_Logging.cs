using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class MainForm
    {
        private static StringBuilder _logFile = new StringBuilder();
        private static readonly StringBuilder PluginLogFile = new StringBuilder(100000);
        private static DateTime _logStartDateTime = DateTime.Now;
        
        internal static void LogExceptionToFile(Exception ex, string info)
        {
            ex.HelpLink = info + ": " + ex.Message;
            LogExceptionToFile(ex);
        }


        internal static void LogExceptionToFile(Exception ex)
        {
            if (!_logging || !Conf.Logging.Enabled)
                return;

            try
            {
                string em = ex.HelpLink + "<br/>" + ex.Message + "<br/>" + ex.Source + "<br/>" + ex.StackTrace +
                            "<br/>" + ex.InnerException + "<br/>" + ex.Data;
                _logFile.Append("<tr><td style=\"color:red\" valign=\"top\">Exception:</td><td valign=\"top\">" +
                               DateTime.Now.ToLongTimeString() + "</td><td valign=\"top\">" + em + "</td></tr>");
            }
            catch
            {
                // ignored
            }
        }
        internal static void LogMessageToFile(String message, string e)
        {
            LogMessageToFile(String.Format(message, e));
        }
        internal static void LogMessageToFile(String message)
        {
            if (!_logging || !Conf.Logging.Enabled)
                return;

            try
            {
                _logFile.Append("<tr><td style=\"color:green\" valign=\"top\">Message</td><td valign=\"top\">" +
                               DateTime.Now.ToLongTimeString() + "</td><td valign=\"top\">" + message + "</td></tr>");
            }
            catch
            {
                //do nothing
            }
        }

        internal static void LogPluginToFile(string name, int id, string action, string detail)
        {
            DateTime dt = Helper.Now;
            PluginLogFile.Append("<message name=\"" + name + "\" id=\"" + id + "\" action=\"" + action + "\" timestamp=\"" + dt.Ticks+"\">" + detail.Replace("&", "&amp;") + "</message>");
        }

        internal static void LogErrorToFile(String message)
        {
            if (!_logging || !Conf.Logging.Enabled)
                return;

            try
            {
                _logFile.Append("<tr><td style=\"color:red\" valign=\"top\">Error</td><td valign=\"top\">" +
                               DateTime.Now.ToLongTimeString() + "</td><td valign=\"top\">" + message + "</td></tr>");
            }
            catch
            {
                //do nothing
            }
        }
        internal static void LogErrorToFile(String message, string message2)
        {
            if (!_logging || !Conf.Logging.Enabled)
                return;

            try
            {
                _logFile.Append("<tr><td style=\"color:red\" valign=\"top\">Error</td><td valign=\"top\">" +
                               DateTime.Now.ToLongTimeString() + "</td><td valign=\"top\">" + message + ", "+message2+"</td></tr>");
            }
            catch
            {
                //do nothing
            }
        }

        internal static void LogWarningToFile(String message)
        {
            if (!_logging || !Conf.Logging.Enabled)
                return;

            try
            {
                _logFile.Append("<tr><td style=\"color:orange\" valign=\"top\">Warning</td><td valign=\"top\">" +
                               DateTime.Now.ToLongTimeString() + "</td><td valign=\"top\">" + message + "</td></tr>");
            }
            catch
            {
                //do nothing
            }
        }


        private void WriteLogs()
        {
            if (!Conf.Logging.Enabled)
                return;
            if (DateTime.Now.DayOfYear != _logStartDateTime.DayOfYear)
            {
                //start new log
                _logging = true;
                _logStartDateTime = DateTime.Now;
                _logFile = new StringBuilder();
                InitLogging(false);
                return;
            }
            if (_logging)
            {
                try
                {
                    if (_logFile.Length > Conf.Logging.FileSize * 1024)
                    {
                        _logFile.Append("<tr><td style=\"color:red\" valign=\"top\">Logging Exiting</td><td valign=\"top\">" +
                            DateTime.Now.ToLongTimeString() +
                            "</td><td valign=\"top\">Logging is being disabled as it has reached the maximum size (" +
                            Conf.Logging.FileSize + "kb).</td></tr>");
                        _logging = false;
                    }
                    if (_lastlog.Length != _logFile.Length)
                    {
                        string logTemplate = "<html><head><title>iSpy v"+ProductVersion+" Log File</title><style type=\"text/css\">body,td,th,div {font-family:Verdana;font-size:10px}</style></head><body><h1>"+Conf.ServerName+": Log Start (v"+Application.
                                                                                                          ProductVersion + " Platform: " + Program.Platform+"): " + _logStartDateTime + "</h1><p><table cellpadding=\"2px\"><!--CONTENT--></table></p></body></html>";
                        string fc = logTemplate.Replace("<!--CONTENT-->", _logFile.ToString());
                        File.WriteAllText(Program.AppDataPath + @"log_" + NextLog + ".htm", fc);
                        _lastlog = _logFile.ToString();
                    }
                }
                catch (Exception)
                {
                    _logging = false;
                }
            }
            

            try
            {
                if (_lastPluginLog.Length != PluginLogFile.Length)
                {
                    string fc = PluginLogTemplate.Replace("<!--CONTENT-->", PluginLogFile.ToString());
                    File.WriteAllText(Program.AppDataPath + @"plugin_log_" + NextLog + ".xml", fc);
                    _lastPluginLog = PluginLogFile.ToString();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void InitLogging(bool warnonerror = true)
        {
            DateTime logdate = DateTime.Now;

            foreach (string s in Directory.GetFiles(Program.AppDataPath, "log_*", SearchOption.TopDirectoryOnly))
            {
                var fi = new FileInfo(s);
                if (fi.CreationTime < Helper.Now.AddDays(0-Conf.Logging.KeepDays))
                    FileOperations.Delete(s);
            }
            NextLog = Zeropad(logdate.Day) + Zeropad(logdate.Month) + logdate.Year;
            int i = 1;
            if (File.Exists(Program.AppDataPath + "log_" + NextLog + ".htm"))
            {
                while (File.Exists(Program.AppDataPath + "log_" + NextLog + "_" + i + ".htm"))
                    i++;
                NextLog += "_" + i;
            }
            try
            {
                File.WriteAllText(Program.AppDataPath + "log_" + NextLog + ".htm", Helper.Now + Environment.NewLine);
                _logging = true;
            }
            catch (Exception ex)
            {
                if (warnonerror && MessageBox.Show(LocRm.GetString("LogStartError").Replace("[MESSAGE]", ex.Message), LocRm.GetString("Warning"), MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    ShuttingDown = true;
                    Close();
                }
            }
        }
    }
}
