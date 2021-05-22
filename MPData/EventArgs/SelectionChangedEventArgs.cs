namespace MPData
{
    public class SelectionChangedEventArgs : System.EventArgs
    {
        public string SelectedItem { get; set; }
        public DishType SelectedType { get; set; }

        public SelectionChangedEventArgs(string selectedItem, DishType selectedType)
        {
            SelectedItem = selectedItem;
            SelectedType = selectedType;
        }
    }
}
