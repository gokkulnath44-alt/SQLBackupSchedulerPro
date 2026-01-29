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
using SQLBackupSchedulerPro.ViewModels;
using System.Windows.Shapes;

namespace SQLBackupSchedulerPro.Views
{
    /// <summary>
    /// Interaction logic for BackupView.xaml
    /// </summary>
    public partial class BackupView : UserControl
    {
        public BackupView()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetService(typeof(BackupViewModel));
        }

        private async void StartBackup(object sender, RoutedEventArgs e)
        {
            await ((BackupViewModel)DataContext).StartBackupAsync();
        }

        private void CancelBackup(object sender, RoutedEventArgs e)
        {
            ((BackupViewModel)DataContext).CancelBackup();
        }
    }
}
