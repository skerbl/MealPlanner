using PropertyChanged;

namespace MPData
{
    public class MealViewModel : BaseNotificationClass
    {
        #region Public Properties

        public string Weekday { get; set; }
        public string Starter { get; set; } = "";
        public string MainDish { get; set; } = "";
        public string SideDish { get; set; } = "";

        #endregion

        #region Private Members

        private string _selectedItem;
        private DishType _selectedType;

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
