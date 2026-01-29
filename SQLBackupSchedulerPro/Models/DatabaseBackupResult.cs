using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLBackupSchedulerPro.Models
{
    public class DatabaseBackupResult
    {
        public string DatabaseName { get; set; } = "";
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
