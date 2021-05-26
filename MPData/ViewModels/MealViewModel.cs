using PropertyChanged;
using System.Windows.Input;

namespace MPData
{
    public class MealViewModel : BaseNotificationClass
    {
        #region Public Properties

        public string Weekday { get; set; }
        public string Starter { get; set; } = "";
        public string MainDish { get; set; } = "";
        public string SideDish { get; set; } = "";
        public ICommand AddItemCommand { get; set; }

        #endregion

        #region Private Members

        private string _selectedItem;
        private DishType _selectedType;

        #endregion

        #region Constructor

        public MealViewModel()
        {
            AddItemCommand = new RelayCommand(AddItem);
        }

        #endregion

        #region Private Methods

        private void AddItem()
        {
            switch (_selectedType)
            {
                case DishType.Starters:
                    Starter = _selectedItem;
                    break;

                case DishType.MainDishes:
                    MainDish = _selectedItem;
                    break;

                case DishType.SideDishes:
                    SideDish = _selectedItem;
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Public Methods

        public Meal ConvertToMeal()
        {
            return new Meal(Starter, MainDish, SideDish);
        }

        #endregion

        #region Event Handlers

        [SuppressPropertyChangedWarnings]
        internal void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedItem = e.SelectedItem;
            _selectedType = e.SelectedType;
        }

        #endregion
    }
}
