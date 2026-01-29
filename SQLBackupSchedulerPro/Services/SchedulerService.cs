using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLBackupSchedulerPro.Services.Interfaces;

namespace SQLBackupSchedulerPro.Services
{
    public class SchedulerService : ISchedulerService
    {
        private Timer? _timer;
        public DateTime? NextRunTime { get; private set; }

        public void StartScheduler(Func<Task> backupAction)
        {
            ScheduleNextRun(backupAction);
        }

        private void ScheduleNextRun(Func<Task> backupAction)
        {
            DateTime now = DateTime.Now;

            // 🔹 Example: user selected 02:00 AM
            TimeSpan scheduledTime = App.AppHost.Services
                .GetService(typeof(ISettingsService)) is ISettingsService settings
                ? settings.Current.ScheduledBackupTime
                : TimeSpan.FromHours(2);

            DateTime nextRun = DateTime.Today.Add(scheduledTime);

            if (nextRun <= now)
                nextRun = nextRun.AddDays(1);

            NextRunTime = nextRun;

            TimeSpan delay = nextRun - now;

            _timer?.Dispose();
            _timer = new Timer(async _ =>
            {
                await backupAction();
                ScheduleNextRun(backupAction);
            },
            null,
            delay,
            Timeout.InfiniteTimeSpan);
        }
    }
}
