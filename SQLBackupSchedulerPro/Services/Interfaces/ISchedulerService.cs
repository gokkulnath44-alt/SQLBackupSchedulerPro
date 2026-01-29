using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLBackupSchedulerPro.Services.Interfaces
{
    public interface ISchedulerService
    {
        void StartScheduler(Func<Task> backupAction);
        DateTime? NextRunTime { get; }
    }
}
