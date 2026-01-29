using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLBackupSchedulerPro.Models;

namespace SQLBackupSchedulerPro.Services.Interfaces
{
    public interface IRestoreService
    {
        Task RestoreAsync(
            string connectionString,
            string backupFolder,
            bool overwriteExisting,
            IProgress<DatabaseRestoreResult> progress,
            CancellationToken cancellationToken);
    }
}
