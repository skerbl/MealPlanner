using System.Collections.Generic;

namespace MPData
{
    public class MealListViewModel : BaseNotificationClass
    {
        #region Public Properties

        public List<MealViewModel> Meals { get; set; }

        #endregion

        #region Constructor

        public MealListViewModel()
        {
            Meals = new List<MealViewModel>()
            {
                new MealViewModel() { Weekday = "Montag" },
                new MealViewModel() { Weekday = "Dienstag" },
                new MealViewModel() { Weekday = "Mittwoch" },
                new MealViewModel() { Weekday = "Donnerstag" },
                new MealViewModel() { Weekday = "Freitag" },
                new MealViewModel() { Weekday = "Samstag" },
            };
        }

        #endregion
    }
}
