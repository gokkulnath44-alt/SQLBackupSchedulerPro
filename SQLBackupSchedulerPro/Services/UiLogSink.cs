using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Core;
using Serilog.Events;
using System.Windows;


namespace SQLBackupSchedulerPro.Services
{
    public class UiLogSink : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                LogHub.LogEvents.Add(logEvent);

                // Keep only last 1000 logs (prevent memory leak)
                if (LogHub.LogEvents.Count > 1000)
                    LogHub.LogEvents.RemoveAt(0);
            });
        }
    }
}
