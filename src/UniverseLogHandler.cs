using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UniverseLib
{
    internal class UniverseLogHandler : ILogHandler
    {
        private readonly ILogHandler wrappedLogHandler;
        private readonly bool prependTag;

        public UniverseLogHandler(ILogHandler toWrap, bool prependTag)
        {
            wrappedLogHandler = toWrap;
            this.prependTag = prependTag;
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            if (wrappedLogHandler == null) return;

            if (prependTag)
            {
                format = $"[{Universe.NAME}] {format}";
            }
            wrappedLogHandler.LogFormat(logType, context, format, args);
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            if (prependTag)
            {
                exception.Source = $"[{Universe.NAME}] {exception.Source}";
            }
            wrappedLogHandler.LogException(exception, context);
        }
    }
}
