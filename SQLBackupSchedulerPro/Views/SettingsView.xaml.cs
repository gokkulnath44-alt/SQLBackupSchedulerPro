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
using Microsoft.Win32;




namespace SQLBackupSchedulerPro.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetService(typeof(SettingsViewModel));
        }

        private void BrowseFolder(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select Backup Folder"
            };

            if (dialog.ShowDialog() == true)
            {
                ((SettingsViewModel)DataContext)
                    .SetBackupPath(dialog.FolderName);
            }
        }



        private async void TestConnection(object sender, RoutedEventArgs e)
        {
            await ((SettingsViewModel)DataContext).TestConnectionAsync();
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            ((SettingsViewModel)DataContext).Save();
        }
    }
}
