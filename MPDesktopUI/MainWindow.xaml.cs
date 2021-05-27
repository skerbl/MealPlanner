using MPData;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace MPDesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDishItemDataService _dishItemDataService;
        private readonly IFileWriter _excelFileWriter;
        private readonly MainViewModel _mainViewModel;

        public MainWindow()
        {
            InitializeComponent();

            _dishItemDataService = new CsvDataService();
            _excelFileWriter = new EpplusFileWriter();
            _mainViewModel = new MainViewModel(_dishItemDataService, _excelFileWriter);

            _mainViewModel.OnErrorMessageRaised += OnErrorMessageRaised;
            _dishItemDataService.OnErrorMessageRaised += OnErrorMessageRaised;

            DataContext = _mainViewModel;
        }

        private void OnClick_AddNewDishItem(object sender, RoutedEventArgs e)
        {
            TabItem ti = selectionTabs.SelectedItem as TabItem;
            _mainViewModel.AddNewDishItem(ti.Name);
        }

        private void OnClick_SaveAsPdf(object sender, RoutedEventArgs e)
        {
            //Set up the WPF Control to be printed
            XamlToPdfTest controlToPrint;
            controlToPrint = new XamlToPdfTest
            {
                DataContext = _mainViewModel
            };

            FixedDocument fixedDoc = new FixedDocument();
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();

            //Create first page of document
            fixedPage.Children.Add(controlToPrint);
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);

            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "XamlToPdfTest"; // Default file name
            dlg.DefaultExt = ".xps"; // Default file extension
            dlg.Filter = "XPS Documents (.xps)|*.xps"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;

                XpsDocument xpsd = new XpsDocument(filename, FileAccess.ReadWrite);
                System.Windows.Xps.XpsDocumentWriter xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
                xw.Write(fixedDoc);
                xpsd.Close();
            }
        }

        private void Menu_Settings(object sender, RoutedEventArgs e)
        {
            Window window = new SettingsWindow(_mainViewModel);
            window.DataContext = _mainViewModel;
            window.Show();
        }
        private void Menu_Close(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void OnErrorMessageRaised(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

    }
}
