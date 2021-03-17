namespace MPData.Models
{
    public class Meal : BaseNotificationClass
    {
        public string Starter { get; set; }
        public string MainDish { get; set; }
        public string SideDish { get; set; }

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
