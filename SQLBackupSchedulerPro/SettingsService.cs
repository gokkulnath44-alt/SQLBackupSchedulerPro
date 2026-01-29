using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SQLBackupSchedulerPro.Models;
using SQLBackupSchedulerPro.Services.Interfaces;
using SQLBackupSchedulerPro.Storage;

namespace SQLBackupSchedulerPro
{
    public class SettingsService : ISettingsService
    {
        private readonly JsonSettingsStorage _storage;

        public AppSettings Current { get; private set; }

        public SettingsService(JsonSettingsStorage storage)
        {
            _storage = storage;
            Current = _storage.Load();
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var con = new SqlConnection(Current.SqlConnectionString);
                await con.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Save()
        {
            _storage.Save(Current);
        }
    }
}
