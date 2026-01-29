using System.Collections.ObjectModel;
using Serilog.Events;
using SQLBackupSchedulerPro.Models;
using SQLBackupSchedulerPro.Services;

namespace SQLBackupSchedulerPro.ViewModels;

public class LogsViewModel : BaseViewModel
{
    public ObservableCollection<LogEntry> Logs { get; }
        = new();

    public int TotalLogs => Logs.Count;

    public int InfoCount => Logs.Count(l =>
        l.Level.Equals("Information", StringComparison.OrdinalIgnoreCase) ||
        l.Level.Equals("Info", StringComparison.OrdinalIgnoreCase));

    public int WarningCount => Logs.Count(l =>
        l.Level.Equals("Warning", StringComparison.OrdinalIgnoreCase));

    public int ErrorCount => Logs.Count(l =>
        l.Level.Equals("Error", StringComparison.OrdinalIgnoreCase));


    public LogsViewModel()
    {
        foreach (var log in LogHub.LogEvents)
            AddLog(log);

        LogHub.LogEvents.CollectionChanged += (_, e) =>
        {
            if (e.NewItems == null) return;

            foreach (LogEvent log in e.NewItems)
                AddLog(log);
        };
    }

    public void ClearLogs()
    {
        Logs.Clear();

        OnPropertyChanged(nameof(TotalLogs));
        OnPropertyChanged(nameof(InfoCount));
        OnPropertyChanged(nameof(WarningCount));
        OnPropertyChanged(nameof(ErrorCount));
    }

    public void Refresh()
    {
        OnPropertyChanged(nameof(Logs));
        OnPropertyChanged(nameof(TotalLogs));
        OnPropertyChanged(nameof(InfoCount));
        OnPropertyChanged(nameof(WarningCount));
        OnPropertyChanged(nameof(ErrorCount));
    }


    private void AddLog(LogEvent log)
    {
        Logs.Add(new LogEntry
        {
            Timestamp = log.Timestamp.LocalDateTime,
            Level = log.Level.ToString(),
            Message = log.RenderMessage()
        });

        // 🔄 Notify counters
        OnPropertyChanged(nameof(TotalLogs));
        OnPropertyChanged(nameof(InfoCount));
        OnPropertyChanged(nameof(WarningCount));
        OnPropertyChanged(nameof(ErrorCount));
    }

}
