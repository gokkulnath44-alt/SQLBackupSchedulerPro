using Microsoft.Data.SqlClient;
using SQLBackupSchedulerPro.Models;
using SQLBackupSchedulerPro.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLBackupSchedulerPro.Services
{
    public class BackupService : IBackupService
    {
        private readonly ISettingsService _settings;
        private readonly DiskSpaceService _diskSpaceService;

        public BackupService(
            ISettingsService settings,
            DiskSpaceService diskSpaceService)
        {
            _settings = settings;
            _diskSpaceService = diskSpaceService;
        }

        public async Task RunBackupAsync(
            IProgress<DatabaseBackupResult> progress,
            CancellationToken cancellationToken)
        {
            var settings = _settings.Current;

            // 🛡️ Disk space safety check
            long freeGB = _diskSpaceService.GetFreeSpaceGB(settings.BackupRootPath);
            if (freeGB < 10)
            {
                Serilog.Log.Error(
                    "Backup aborted. Low disk space: {FreeGB} GB available",
                    freeGB);

                throw new InvalidOperationException(
                    $"Not enough disk space. Only {freeGB} GB available.");
            }

            // 📁 Create dated folder
            string dateFolder = Path.Combine(
                settings.BackupRootPath,
                DateTime.Now.ToString("yyyy-MM-dd"));
            Directory.CreateDirectory(dateFolder);

            // 📋 Fetch database names (SEPARATE CONNECTION)
            var databases = new List<string>();

            using (var listConnection = new SqlConnection(settings.SqlConnectionString))
            {
                await listConnection.OpenAsync(cancellationToken);

                using var cmd = new SqlCommand(
                    "SELECT name FROM sys.databases WHERE name NOT IN ('master','model','msdb','tempdb')",
                    listConnection);

                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    databases.Add(reader.GetString(0));
                }
            }

            // 💾 Backup databases ONE BY ONE (NEW CONNECTION EACH)
            foreach (var db in databases)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = new DatabaseBackupResult
                {
                    DatabaseName = db
                };

                try
                {
                    using var backupConnection = new SqlConnection(settings.SqlConnectionString);
                    await backupConnection.OpenAsync(cancellationToken);

                    string filePath = Path.Combine(
                        dateFolder,
                        $"{db}_{DateTime.Now:yyyyMMdd_HHmm}.bak");

                    string sql = $@"
BACKUP DATABASE [{db}]
TO DISK = '{filePath}'
WITH INIT";

                    using var backupCmd = new SqlCommand(sql, backupConnection);
                    await backupCmd.ExecuteNonQueryAsync(cancellationToken);

                    result.Success = true;
                    Serilog.Log.Information(
                        "Backup successful for database {Database}", db);
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.ErrorMessage = ex.Message;

                    Serilog.Log.Error(
                        ex,
                        "Backup failed for database {Database}", db);
                }

                progress.Report(result);
            }
        }
    }
}
