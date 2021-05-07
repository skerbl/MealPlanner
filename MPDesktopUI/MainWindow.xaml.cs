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

        private void OnClick_AddDishToMeal(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            TabItem ti = selectionTabs.SelectedItem as TabItem;
            _mainViewModel.AddDishToMeal(btn.Name, ti.Name);
        }

        private void OnClick_AddNewDishItem(object sender, RoutedEventArgs e)
        {
            TabItem ti = selectionTabs.SelectedItem as TabItem;
            _mainViewModel.AddNewDishItem(ti.Name);
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
