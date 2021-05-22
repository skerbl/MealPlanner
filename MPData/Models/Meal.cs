namespace MPData
{
    public class Meal : BaseNotificationClass
    {
        #region Public Properties

        public string Starter { get; set; }
        public string MainDish { get; set; }
        public string SideDish { get; set; }

        #endregion

        public Meal(string starter, string mainDish, string sideDish)
        {
            Starter = starter;
            MainDish = mainDish;
            SideDish = sideDish;
        }

        public Meal()
        {
            Starter = "";
            MainDish = "";
            SideDish = "";
        }
    }
}
