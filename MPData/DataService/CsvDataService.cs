using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MPData
{
    public class CsvDataService : IDishItemDataService
    {
        #region Private Members

        private List<string> _starters;
        private List<string> _mainDishes;
        private List<string> _sideDishes; 

        #endregion

        public event EventHandler<MessageEventArgs> OnErrorMessageRaised;

        #region Public Methods

        /// <summary>
        /// Gets all dish items of a dish type
        /// </summary>
        /// <param name="dishType">The type of dish</param>
        /// <returns>A list of dish items</returns>
        public IEnumerable<string> GetAll(DishType dishType)
        {
            string fileName = dishType.ToString().ToLower() + ".csv";

            using (StreamReader sr = new StreamReader(File.OpenRead(fileName)))
            {
                List<string> itemList = new List<string>();

                while (!sr.EndOfStream)
                {
                    itemList.Add(sr.ReadLine());
                }

                if (!Utils.IsAscendingOrder(itemList))
                {
                    itemList.Sort();
                }

                // Remove duplicates just in case
                List<string> uniqueList = itemList.Distinct().ToList();

                switch (dishType)
                {
                    case DishType.Starters:
                        _starters = uniqueList;
                        break;
                    case DishType.MainDishes:
                        _mainDishes = uniqueList;
                        break;
                    case DishType.SideDishes:
                        _sideDishes = uniqueList;
                        break;
                }

                foreach (var item in uniqueList)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Adds an item to the respective item type's list.
        /// Writes the new list back to the respective csv file.
        /// </summary>
        /// <param name="newItem">The new dish item</param>
        /// <param name="dishType">The type of the dish item</param>
        /// <returns>The new list</returns>
        public IEnumerable<string> AddItem(string newItem, DishType dishType)
        {
            string fileName = dishType.ToString().ToLower() + ".csv";
            List<string> itemList = new List<string>();

            switch (dishType)
            {
                case DishType.Starters:
                    itemList = _starters;
                    break;
                case DishType.MainDishes:
                    itemList = _mainDishes;
                    break;
                case DishType.SideDishes:
                    itemList = _sideDishes;
                    break;
            }

            if (!itemList.Contains(newItem))
            {
                itemList.Add(newItem);
                itemList.Sort();

                using (var file = File.CreateText(fileName))
                {
                    foreach (var item in itemList)
                    {
                        if (string.IsNullOrEmpty(item)) continue;
                        file.WriteLine(item);
                    }
                }
            }
            else
            {
                RaiseMessage("Der Eintrag existiert bereits.");
            }

            foreach (var item in itemList)
            {
                yield return item;
            }
        } 

        /// <summary>
        /// Remove?
        /// </summary>
        public void OpenConnection()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove?
        /// </summary>
        public void CloseConnection()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private void RaiseMessage(string message)
        {
            OnErrorMessageRaised?.Invoke(this, new MessageEventArgs(message));
        } 

        #endregion
    }
}
