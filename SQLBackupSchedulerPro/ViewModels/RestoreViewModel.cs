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
    public class RestoreViewModel : BaseViewModel
    {
        private readonly IRestoreService _restoreService;

        public ObservableCollection<DatabaseRestoreResult> Results { get; }
            = new();

        private CancellationTokenSource? _cts;

        public string TargetConnectionString { get; set; } = "";
        public string BackupFolder { get; set; } = "";
        public bool OverwriteExisting { get; set; }

        public RestoreViewModel(IRestoreService restoreService)
        {
            _restoreService = restoreService;
        }

        public async Task StartRestoreAsync()
        {
            Results.Clear();
            _cts = new CancellationTokenSource();

            var progress = new Progress<DatabaseRestoreResult>(r =>
            {
                Results.Add(r);
            });

            try
            {
                await _restoreService.RestoreAsync(
                    TargetConnectionString,
                    BackupFolder,
                    OverwriteExisting,
                    progress,
                    _cts.Token);

                MessageBox.Show("Restore completed");
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Restore canceled");
            }
        }

        public void SetBackupFolder(string path)
        {
            BackupFolder = path;
            OnPropertyChanged(nameof(BackupFolder));
        }


        public void CancelRestore()
        {
            _cts?.Cancel();
        }
    }
}
