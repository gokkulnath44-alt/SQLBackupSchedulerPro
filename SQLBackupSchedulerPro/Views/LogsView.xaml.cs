using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SQLBackupSchedulerPro.ViewModels;

namespace SQLBackupSchedulerPro.Views
{
    /// <summary>
    /// Interaction logic for LogsView.xaml
    /// </summary>
    public partial class LogsView : UserControl
    {
        public LogsView()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetService(typeof(LogsViewModel));
        }

        private void ClearLogs_Click(object sender, RoutedEventArgs e)
        {
            ((LogsViewModel)DataContext).ClearLogs();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ((LogsViewModel)DataContext).Refresh();
        }


    }
}
