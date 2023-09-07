using System;
using UnityEngine;

namespace UniverseLib
{
    internal class UniverseLogger : Logger
    {
#pragma warning disable IDE1006 // Naming Styles
        public new UniverseLogHandler logHandler
        {
            get => base.logHandler as UniverseLogHandler;
            set => base.logHandler = value;
        }
#pragma warning restore IDE1006 // Naming Styles

        public UniverseLogger()
        { }

        public UniverseLogger(UniverseLogHandler logHandler) 
            : base(logHandler)
        { }

        public UniverseLogger(ILogHandler logHandler, bool prependTag)
            : base(new UniverseLogHandler(logHandler, prependTag))
        { }

        public void Log(object message, UnityEngine.Object context = null)
        {
            Log(LogType.Log, message, context);
        }

        public void LogWarning(object message, UnityEngine.Object context = null)
        {
            Log(LogType.Warning, message, context);
        }

        public void LogError(object message, UnityEngine.Object context = null)
        {
            Log(LogType.Error, message, context);
        }

        public void LogException(string invocationDetails, Exception exception, UnityEngine.Object context = null)
        {
            exception = new Exception($"{invocationDetails}: {exception.Message}", exception);
            base.LogException(exception, context);
        }
    }
}
