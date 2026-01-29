using SQLBackupSchedulerPro.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SQLBackupSchedulerPro.Storage
{
    public class JsonSettingsStorage
    {
        private readonly string _filePath = Path.Combine("Config", "settings.json");

        public AppSettings Load()
        {
            if (!File.Exists(_filePath))
                return new AppSettings();

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<AppSettings>(json)!;
        }

        public void Save(AppSettings settings)
        {
            Directory.CreateDirectory("Config");

            string json = JsonSerializer.Serialize(
                settings,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_filePath, json);
        }
    }
}
