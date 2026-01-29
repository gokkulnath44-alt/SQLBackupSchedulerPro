using SQLBackupSchedulerPro.Services;
using SQLBackupSchedulerPro.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLBackupSchedulerPro.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly ISettingsService _settings;
        private readonly ISchedulerService _scheduler;
        private readonly DiskSpaceService _diskService;


        public DashboardViewModel(
            ISettingsService settings,
            ISchedulerService scheduler,
            DiskSpaceService diskService)
        {
            _settings = settings;
            _scheduler = scheduler;
            _diskService = diskService;
        }

        public long FreeDiskSpaceGB
        {
            get
            {
                if (string.IsNullOrEmpty(_settings.Current.BackupRootPath))
                    return 0;

                return _diskService.GetFreeSpaceGB(
                    _settings.Current.BackupRootPath);
            }
        }

        public string DiskStatus
        {
            get
            {
                return FreeDiskSpaceGB >= 10
                    ? "OK"
                    : "LOW";
            }
        }


        public DateTime? LastRunTime => _settings.Current.LastRunTime;
        public string LastRunStatus => _settings.Current.LastRunStatus;
        public DateTime? NextRunTime => _scheduler.NextRunTime;
    }
}
