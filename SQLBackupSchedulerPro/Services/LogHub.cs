using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLBackupSchedulerPro.Services
{
    public static class LogHub
    {
        public static ObservableCollection<LogEvent> LogEvents { get; }
            = new ObservableCollection<LogEvent>();
    }
}
