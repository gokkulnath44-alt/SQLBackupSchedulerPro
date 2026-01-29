using Microsoft.Win32;
using SQLBackupSchedulerPro.ViewModels;
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

namespace SQLBackupSchedulerPro.Views
{
    /// <summary>
    /// Interaction logic for RestoreView.xaml
    /// </summary>
    public partial class RestoreView : UserControl
    {
        public RestoreView()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetService(typeof(RestoreViewModel));
        }

        private async void StartRestore(object sender, RoutedEventArgs e)
        {
            await ((RestoreViewModel)DataContext).StartRestoreAsync();
        }

        private void BrowseRestoreFolder(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select Backup Folder to Restore"
            };

            if (dialog.ShowDialog() == true)
            {
                ((RestoreViewModel)DataContext)
                    .SetBackupFolder(dialog.FolderName);
            }
        }


        private void CancelRestore(object sender, RoutedEventArgs e)
        {
            ((RestoreViewModel)DataContext).CancelRestore();
        }
    }
}
