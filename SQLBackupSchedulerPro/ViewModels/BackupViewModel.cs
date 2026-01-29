using SQLBackupSchedulerPro.Models;
using SQLBackupSchedulerPro.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SQLBackupSchedulerPro.ViewModels
{
    public class BackupViewModel : BaseViewModel
    {
        private readonly IBackupService _backupService;

        public ObservableCollection<DatabaseBackupResult> Results { get; }
            = new();

        private CancellationTokenSource? _cts;

        public BackupViewModel(IBackupService backupService)
        {
            _backupService = backupService;
        }

        public async Task StartBackupAsync()
        {
            Results.Clear();
            _cts = new CancellationTokenSource();

            var progress = new Progress<DatabaseBackupResult>(result =>
            {
                Results.Add(result);
            });

            try
            {
                await _backupService.RunBackupAsync(progress, _cts.Token);
                MessageBox.Show("Backup completed");
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Backup canceled");
            }
        }

        public void CancelBackup()
        {
            _cts?.Cancel();
        }
    }
}
