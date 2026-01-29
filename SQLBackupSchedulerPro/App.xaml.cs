using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SQLBackupSchedulerPro.Services;
using SQLBackupSchedulerPro.Services.Interfaces;
using SQLBackupSchedulerPro.Storage;
using SQLBackupSchedulerPro.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;
using Serilog;
using Serilog.Events;

namespace SQLBackupSchedulerPro
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public App()
        {
            Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File(
            path: "logs/log-.txt",
            rollingInterval: RollingInterval.Day)
        .WriteTo.Sink(new UiLogSink())
        .CreateLogger();

            AppHost = Host.CreateDefaultBuilder()
                .UseSerilog()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<JsonSettingsStorage>();
                    services.AddSingleton<ISettingsService, SettingsService>();
                    services.AddSingleton<SettingsViewModel>();
                    services.AddSingleton<IBackupService, BackupService>();
                    services.AddSingleton<BackupViewModel>();
                    services.AddSingleton<ISchedulerService, SchedulerService>();
                    services.AddSingleton<DashboardViewModel>();
                    services.AddSingleton<IRestoreService, RestoreService>();
                    services.AddSingleton<RestoreViewModel>();
                    services.AddSingleton<LogsViewModel>();
                    services.AddSingleton<DiskSpaceService>();



                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();

            // 🔹 Get required services from DI
            var scheduler = AppHost.Services.GetService<ISchedulerService>();
            var backupService = AppHost.Services.GetService<IBackupService>();
            var settingsService = AppHost.Services.GetService<ISettingsService>();

            // 🔹 Start the scheduler
            scheduler?.StartScheduler(async () =>
            {
                try
                {
                    var progress = new Progress<Models.DatabaseBackupResult>(_ => { });

                    await backupService!.RunBackupAsync(
                        progress,
                        CancellationToken.None);

                    settingsService!.Current.LastRunTime = DateTime.Now;
                    settingsService.Current.LastRunStatus = "Success";
                    settingsService.Save();
                }
                catch (Exception)
                {
                    settingsService!.Current.LastRunTime = DateTime.Now;
                    settingsService.Current.LastRunStatus = "Failed";
                    settingsService.Save();
                }
            });

            base.OnStartup(e);
        }

    }

}
