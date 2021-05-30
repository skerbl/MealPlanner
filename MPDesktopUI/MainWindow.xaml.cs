using MPData;
using System.Windows;
using System.Windows.Controls;

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
        private readonly XamlToPdf _xamlToPdfWriter;

        public MainWindow()
        {
            InitializeComponent();

            _dishItemDataService = new CsvDataService();
            _excelFileWriter = new EpplusFileWriter();
            _mainViewModel = new MainViewModel(_dishItemDataService, _excelFileWriter);
            _xamlToPdfWriter = new XamlToPdf(_mainViewModel);

            _mainViewModel.OnErrorMessageRaised += OnErrorMessageRaised;
            _dishItemDataService.OnErrorMessageRaised += OnErrorMessageRaised;
            _xamlToPdfWriter.OnErrorMessageRaised += OnErrorMessageRaised;

            DataContext = _mainViewModel;
        }

        private void OnClick_AddNewDishItem(object sender, RoutedEventArgs e)
        {
            TabItem ti = selectionTabs.SelectedItem as TabItem;
            _mainViewModel.AddNewDishItem(ti.Name);
        }

        private void OnClick_SaveAsPdf(object sender, RoutedEventArgs e)
        {
            _xamlToPdfWriter.SaveAsPdf();
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
