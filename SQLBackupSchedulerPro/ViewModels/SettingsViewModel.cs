using SQLBackupSchedulerPro.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SQLBackupSchedulerPro.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ISettingsService _settings;

        public SettingsViewModel(ISettingsService settings)
        {
            _settings = settings;
            for (int hour = 0; hour < 24; hour++)
            {
                AvailableTimes.Add(new TimeSpan(hour, 0, 0));
                AvailableTimes.Add(new TimeSpan(hour, 30, 0));
            }
        }

        public string ConnectionString
        {
            get => _settings.Current.SqlConnectionString;
            set
            {
                _settings.Current.SqlConnectionString = value;
                OnPropertyChanged();
            }
        }

        public string BackupPath
        {
            get => _settings.Current.BackupRootPath;
            set
            {
                _settings.Current.BackupRootPath = value;
                OnPropertyChanged();
            }
        }

        public async Task TestConnectionAsync()
        {
            bool ok = await _settings.TestConnectionAsync();
            MessageBox.Show(ok ? "Connection successful" : "Connection failed");
        }

        public void Save()
        {
            _settings.Save();
            MessageBox.Show("Settings saved");
        }

        public void SetBackupPath(string path)
        {
            BackupPath = path;
        }


        public List<TimeSpan> AvailableTimes { get; } = new();

        public TimeSpan SelectedBackupTime
        {
            get => _settings.Current.ScheduledBackupTime;
            set
            {
                _settings.Current.ScheduledBackupTime = value;
                OnPropertyChanged();
            }
        }


    }
}
