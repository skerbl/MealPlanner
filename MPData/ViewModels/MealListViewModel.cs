using System;
using System.Collections.Generic;

namespace MPData
{
    public class MealListViewModel
    {
        #region Public Properties

        public List<MealViewModel> Meals { get; set; }

        #endregion

        #region Constructor

        public MealListViewModel(MainViewModel mainViewModel)
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

            foreach (var meal in Meals)
            {
                mainViewModel.SelectionChanged += meal.OnSelectionChanged;
            }
        }

        #endregion
    }
}
