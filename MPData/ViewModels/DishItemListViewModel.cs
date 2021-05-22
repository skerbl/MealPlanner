using System.Collections.ObjectModel;

namespace MPData
{
    public class DishItemListViewModel : BaseNotificationClass
    {
        #region Public Properties

        public ObservableCollection<string> Starters { get; set; }
        public ObservableCollection<string> MainDishes { get; set; }
        public ObservableCollection<string> SideDishes { get; set; }

        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public DishType SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private Members

        private string _selectedItem;
        private DishType _selectedType; 

        #endregion
    }
}
