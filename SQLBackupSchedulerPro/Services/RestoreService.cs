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
    public class RestoreService : IRestoreService
    {
        public async Task RestoreAsync(
            string connectionString,
            string backupFolder,
            bool overwriteExisting,
            IProgress<DatabaseRestoreResult> progress,
            CancellationToken cancellationToken)
        {
            // 1️⃣ Get all .bak files
            var bakFiles = Directory.GetFiles(backupFolder, "*.bak");

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);

            foreach (var file in bakFiles)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Extract DB name from filename
                string fileName = Path.GetFileNameWithoutExtension(file);
                string dbName = fileName.Split('_')[0];

                var result = new DatabaseRestoreResult
                {
                    DatabaseName = dbName
                };

                try
                {
                    string sql = overwriteExisting
                        ? $@"
ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
RESTORE DATABASE [{dbName}]
FROM DISK = '{file}'
WITH REPLACE;
ALTER DATABASE [{dbName}] SET MULTI_USER;"
                        : $@"
RESTORE DATABASE [{dbName}]
FROM DISK = '{file}';";

                    using var cmd = new SqlCommand(sql, connection);
                    await cmd.ExecuteNonQueryAsync(cancellationToken);

                    result.Success = true;
                    Serilog.Log.Information("Restore successful for database {Database}", dbName);

                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.ErrorMessage = ex.Message;
                    Serilog.Log.Error(ex, "Restore failed for database {Database}", dbName);

                }

                progress.Report(result);
            }
        }
    }
}
