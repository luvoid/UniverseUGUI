using System;
using System.Xml.Linq;
using UnityEngine;

namespace UniverseLib
{
    [Obsolete]
    internal class LegacyLogHandler : ILogHandler
    {
        readonly Action<string, LogType> logHandler;
        public LegacyLogHandler(Action<string, LogType> logHandler)
        {
            this.logHandler = logHandler;
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            if (logHandler == null) return;

            logHandler($"[{Universe.NAME}] {string.Format(format, args)}", logType);
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            LogFormat(LogType.Error, context, exception.ToString());
        }
    }
}
