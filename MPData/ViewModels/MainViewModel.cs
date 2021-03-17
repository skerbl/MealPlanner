using MPData.DataService;
using MPData.EventArgs;
using MPData.Models;
using MPData.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace MPData.ViewModels
{
    public class MainViewModel : BaseNotificationClass
    {
        private IDishItemDataService _dishItemDataService;
        private IFileWriter _excelFileWriter;

        public event EventHandler<MessageEventArgs> OnErrorMessageRaised;

        public ICommand SaveCommand { get; set; }

        public UserSettings Settings { get; set; }
        public ObservableCollection<string> Starters { get; set; }
        public ObservableCollection<string> MainDishes { get; set; }
        public ObservableCollection<string> SideDishes { get; set; }

        public Dictionary<string, Meal> Meals { get; set; }


        public string SelectedItem { get; set; }

        private string _fromDate;

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

        private string _fileName;

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


        public MainViewModel(IDishItemDataService dishItemDataService, IFileWriter excelWriter)
        {
            this.SaveCommand = new RelayCommand(SaveAsExcel);

            _dishItemDataService = dishItemDataService;
            _excelFileWriter = excelWriter;
            Settings = UserSettingsXml.Deserialize();
            _excelFileWriter.TemplatePath = Settings.TemplatePath;

            Starters = new ObservableCollection<string>();
            MainDishes = new ObservableCollection<string>();
            SideDishes = new ObservableCollection<string>();

            Meals = new Dictionary<string, Meal>();

            InitializeMeals();
            LoadDishes();
        }

        public void AddDishToMeal(string buttonName, string tabItemName)
        {
            switch (tabItemName)
            {
                case "starters":
                    Meals[buttonName].Starter = SelectedItem;
                    break;
                case "mainDishes":
                    Meals[buttonName].MainDish = SelectedItem;
                    break;
                case "sideDishes":
                    Meals[buttonName].SideDish = SelectedItem;
                    break;
                default:
                    break;
            }
        }

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

        private void SaveAsExcel()
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
            file.Directory.Create();

            _excelFileWriter.WriteToFile(Meals, FromDate, ToDate, file.FullName);
        }

        private void InitializeMeals()
        {
            Meals.Add("Monday_1", new Meal());
            Meals.Add("Monday_2", new Meal());
            Meals.Add("Tuesday_1", new Meal());
            Meals.Add("Tuesday_2", new Meal());
            Meals.Add("Wednesday_1", new Meal());
            Meals.Add("Wednesday_2", new Meal());
            Meals.Add("Thursday_1", new Meal());
            Meals.Add("Thursday_2", new Meal());
            Meals.Add("Friday_1", new Meal());
            Meals.Add("Friday_2", new Meal());
            Meals.Add("Saturday", new Meal());
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
    }
}
