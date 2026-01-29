using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLBackupSchedulerPro.Models
{
    public class AppSettings
    {
        public string SqlConnectionString { get; set; } = "";
        public string BackupRootPath { get; set; } = "";
        public TimeSpan ScheduledBackupTime { get; set; } = TimeSpan.FromHours(2);
        public DateTime? LastRunTime { get; set; }
        public string LastRunStatus { get; set; } = "Never";
    }
}
