using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Diagnostics;

namespace MPData
{
    public class MainViewModel : BaseNotificationClass
    {
        #region Private Members

        private IDishItemDataService _dishItemDataService;
        private IFileWriter _excelFileWriter;
        private string _fromDate;
        private string _fileName;
        private string _selectedItem;
        private int _selectedTabIndex;
        private DishType _selectedType;

        #endregion

        #region Public Events

        public event EventHandler<MessageEventArgs> OnErrorMessageRaised;
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        #endregion

        #region Public Properties

        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public UserSettings Settings { get; set; }
        public ObservableCollection<string> Starters { get; set; }
        public ObservableCollection<string> MainDishes { get; set; }
        public ObservableCollection<string> SideDishes { get; set; }

        /*
        public MealListViewModel MealList1 { get; set; }
        public MealListViewModel MealList2 { get; set; }
        */

        public List<MealViewModel> MealList1 { get; set; }
        public List<MealViewModel> MealList2 { get; set; }

        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(_selectedItem, _selectedType));
                OnPropertyChanged();
            }
        }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set 
            { 
                _selectedTabIndex = value;
                SelectedType = (DishType)value;
            }
        }


        public DishType SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(_selectedItem, _selectedType));
                OnPropertyChanged();
            }
        }

        public string FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                FileName = _fromDate;
                OnPropertyChanged();
            }
        }

        public string ToDate { get; set; }

        public string NewItem { get; set; }

        public string FileName
        {
            get => _fileName;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                _fileName = value;

                if (!value.EndsWith(".xlsx"))
                {
                    _fileName = value + ".xlsx";
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructor

        public MainViewModel(IDishItemDataService dishItemDataService, IFileWriter excelWriter)
        {
            SaveCommand = new RelayCommand(SaveFiles);

            _dishItemDataService = dishItemDataService;
            _excelFileWriter = excelWriter;
            Settings = UserSettingsXml.Deserialize();
            _excelFileWriter.TemplatePath = Settings.TemplatePath;

            Starters = new ObservableCollection<string>();
            MainDishes = new ObservableCollection<string>();
            SideDishes = new ObservableCollection<string>();

            InitializeViewModels();
            LoadDishes();
        }

        #endregion

        #region Public Methods

        public void AddNewDishItem(string tabItemName)
        {
            if (string.IsNullOrWhiteSpace(NewItem))
            {
                return;
            }

            IEnumerable<string> newList = default;

            switch (tabItemName)
            {
                case "starters":
                    newList = _dishItemDataService.AddItem(NewItem, DishType.Starters);
                    Starters = new ObservableCollection<string>(newList);
                    break;
                case "mainDishes":
                    newList = _dishItemDataService.AddItem(NewItem, DishType.MainDishes);
                    MainDishes = new ObservableCollection<string>(newList);
                    break;
                case "sideDishes":
                    newList = _dishItemDataService.AddItem(NewItem, DishType.SideDishes);
                    SideDishes = new ObservableCollection<string>(newList);
                    break;
                default:
                    break;
            }

            NewItem = "";
        }

        #endregion

        #region Private Methods

        private void LoadDishes()
        {
            //_dishItemDataService.OpenConnection();

            var soups = _dishItemDataService.GetAll(DishType.Starters);
            Starters.Clear();
            foreach (var soup in soups)
            {
                Starters.Add(soup);
            }

            var mainDishes = _dishItemDataService.GetAll(DishType.MainDishes);
            MainDishes.Clear();
            foreach (var main in mainDishes)
            {
                MainDishes.Add(main);
            }

            var sideDishes = _dishItemDataService.GetAll(DishType.SideDishes);
            SideDishes.Clear();
            foreach (var side in sideDishes)
            {
                SideDishes.Add(side);
            }

            //_dishItemDataService.CloseConnection();
        }

        private void SaveFiles()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                RaiseMessage("Bitte geben Sie einen Dateinamen an.");
                return;
            }

            if (!IsValidFilename(FileName))
            {
                RaiseMessage("Der Dateiname ist ungültig.");
                FileName = "";
                return;
            }

            string combinedPath = Path.Combine(Settings.ExportPath, Path.GetFileName(FileName));
            FileInfo file = new FileInfo(combinedPath);

            SaveAsExcel(file);

            if (Settings.SaveAsPdf)
            {
                if (!file.Exists)
                {
                    return;
                }

                FileInfo officeToPdf = new FileInfo("OfficeToPDF.exe");
                if (officeToPdf.Exists)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = @"OfficeToPDF.exe";
                    startInfo.Arguments = file.FullName + " " + Path.ChangeExtension(file.FullName, ".pdf") + " /excel_active_sheet";
                    Process.Start(startInfo);
                }
                else
                {
                    RaiseMessage("Konnte nicht als PDF speichern.\nDatei \"OfficeToPDF.exe\" nicht gefunden.");
                }


                // TODO: Save as PDF in another way?
                //SaveAsPdf(file);
            }
        }

        private void SaveAsExcel(FileInfo file)
        {
            file.Directory.Create();

            List<Meal> meals1 = new List<Meal>();
            List<Meal> meals2 = new List<Meal>();

            foreach (var meal in MealList1)
            {
                meals1.Add(meal.ConvertToMeal());
            }

            foreach (var meal in MealList2)
            {
                meals2.Add(meal.ConvertToMeal());
            }

            _excelFileWriter.WriteToFile(meals1, meals2, FromDate, ToDate, file.FullName);
        }

        private void SaveAsPdf(FileInfo file)
        {
            //Workbook workbook = new Workbook();
            //workbook.LoadFromFile(file.FullName);
            //
            //Worksheet sheet = workbook.Worksheets[0];
            //
            //sheet.PageSetup.PaperSize = PaperSizeType.PaperA4;
            //sheet.PageSetup.FitToPagesWide = 1;
            //sheet.PageSetup.FitToPagesTall = 1;
            //sheet.PageSetup.TopMargin = 0.5;
            //sheet.PageSetup.LeftMargin = 0.5;
            //sheet.PageSetup.RightMargin = 0.5;
            //sheet.PageSetup.BottomMargin = 0.5;
            //
            //sheet.SaveToPdf(Path.ChangeExtension(file.FullName, ".pdf"));
        }

        private void InitializeViewModels()
        {
            MealList1 = new List<MealViewModel>()
            {
                new MealViewModel() { Weekday = "Montag" },
                new MealViewModel() { Weekday = "Dienstag" },
                new MealViewModel() { Weekday = "Mittwoch" },
                new MealViewModel() { Weekday = "Donnerstag" },
                new MealViewModel() { Weekday = "Freitag" },
                new MealViewModel() { Weekday = "Samstag" }
            };

            MealList2 = new List<MealViewModel>()
            {
                new MealViewModel() { Weekday = "Montag" },
                new MealViewModel() { Weekday = "Dienstag" },
                new MealViewModel() { Weekday = "Mittwoch" },
                new MealViewModel() { Weekday = "Donnerstag" },
                new MealViewModel() { Weekday = "Freitag" }
            };

            foreach (var meal in MealList1)
            {
                SelectionChanged += meal.OnSelectionChanged;
            }

            //MealList2.Meals.RemoveAll(x => x.Weekday == "Samstag"); 
            foreach (var meal in MealList2)
            {
                SelectionChanged += meal.OnSelectionChanged;
            }
        }

        private void CloseApplication()
        {

        }

        private bool IsValidFilename(string testName)
        {
            string strTheseAreInvalidFileNameChars = new string(Path.GetInvalidFileNameChars());
            Regex regInvalidFileName = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");

            if (regInvalidFileName.IsMatch(testName))
            {
                return false;
            }

            return true;
        }

        private void RaiseMessage(string message)
        {
            OnErrorMessageRaised?.Invoke(this, new MessageEventArgs(message));
        } 

        #endregion
    }
}
