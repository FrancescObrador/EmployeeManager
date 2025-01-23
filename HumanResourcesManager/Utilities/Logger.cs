using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourcesManager.Utilities
{
    public class Logger
    {
        private TraceSource traceSource;

        public Logger(string sourceName)
        {
            traceSource = new TraceSource(sourceName, SourceLevels.All);

            traceSource.Switch = new SourceSwitch("defaultSwitch");

            traceSource.Listeners.Clear();
            // Logs for console
            traceSource.Listeners.Add(new ConsoleTraceListener());
            // Logs for file
            traceSource.Listeners.Add(new TextWriterTraceListener("logfile.log")
            {
                TraceOutputOptions = TraceOptions.DateTime
            });
            Trace.AutoFlush = true;

            traceSource.TraceEvent(TraceEventType.Information, 0, "Logger Initialized");
        }


        public void LogInfo(string message)
        {
            traceSource.TraceEvent(TraceEventType.Information, 0, message);
        }

        public void LogWarning(string message)
        {
            traceSource.TraceEvent(TraceEventType.Warning, 1, message);
        }

        public void LogError(string message)
        {
            traceSource.TraceEvent(TraceEventType.Error, 2, message);
        }

        public void Close()
        {
            traceSource.Flush();
            traceSource.Close();
        }
    }
}
