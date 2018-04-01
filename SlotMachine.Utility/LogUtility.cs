using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SlotMachine.Utility
{
    public class LogUtility
    {
        private static readonly object oLocker = new object();

        static LogUtility()
        {

        }

        public static void Log(LogType oType, string sMessage)
        {
            Log(oType, sMessage, string.Empty);
        }

        public static void Log(LogType oType, string sMessage, string sException)
        {
            if (oType.Equals(LogType.SystemError) && !AppConfigUtility.bLogSystemError) return;

            if (oType.Equals(LogType.Information) && !AppConfigUtility.bLogInformation) return;

            lock (oLocker)
            {
                var oLog = !File.Exists(AppConfigUtility.sLogFileName)
                    ? new Log { Logs = new List<LogItem>() }
                    : File.ReadAllText(AppConfigUtility.sLogFileName).DeserializeFromXml<Log>();

                oLog.Logs.RemoveAll(item => item.DateTime <= DateTime.Now.AddDays(-30));

                oLog.Logs.Add(new LogItem { DateTime = DateTime.Now, Type = oType.ToString(), Exception = sException, Message = sMessage });

                var oXmlDoc = new XmlDocument();
                oXmlDoc.LoadXml(oLog.SerializeToXml());
                oXmlDoc.Save(AppConfigUtility.sLogFileName);
            }
        }

        public enum LogType
        {
            Error,
            Warning,
            Information,
            SystemError
        }
    }

    [Serializable]
    [XmlRoot("Log")]
    public class Log
    {
        [XmlArray("Logs"), XmlArrayItem(typeof(LogItem), ElementName = "LogItem")]
        public List<LogItem> Logs { get; set; }
    }

    [Serializable]
    public class LogItem
    {
        public string Type { get; set; }

        public DateTime DateTime { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }
    }
}
