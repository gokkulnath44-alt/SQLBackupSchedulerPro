using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLBackupSchedulerPro.Models;

namespace SQLBackupSchedulerPro.Services.Interfaces
{
    public interface ISettingsService
    {
        AppSettings Current { get; }
        Task<bool> TestConnectionAsync();
        void Save();
    }
}
