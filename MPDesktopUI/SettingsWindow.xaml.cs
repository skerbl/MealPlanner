using MPData.ViewModels;
using MPData.Settings;
using System.Windows;
using System.Windows.Controls;
using Ookii.Dialogs;

namespace MPDesktopUI
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly MainViewModel _mainViewModel;
        private UserSettings _oldSettings;

        public SettingsWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();

            _mainViewModel = mainViewModel;
            _oldSettings = new UserSettings 
            { 
                ExportPath = _mainViewModel.Settings.ExportPath, 
                TemplatePath = _mainViewModel.Settings.TemplatePath 
            };
        }

        private void OnClick_Browse_Export(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                _mainViewModel.Settings.ExportPath = dialog.SelectedPath;
            }
        }

        private void OnClick_Browse_Template(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                _mainViewModel.Settings.TemplatePath = dialog.SelectedPath;
            }
        }

        private void OnClick_Cancel(object sender, RoutedEventArgs e)
        {
            _mainViewModel.Settings = _oldSettings;
            this.Close();
        }

        private void OnClick_Ok(object sender, RoutedEventArgs e)
        {
            UserSettingsXml.Serialize(_mainViewModel.Settings);
            this.Close();
        }
    }
}
