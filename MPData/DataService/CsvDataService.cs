using System;
using System.Collections.Generic;
using System.IO;

namespace MPData
{
    public class CsvDataService : IDishItemDataService
    {
        private List<string> _starters;
        private List<string> _mainDishes;
        private List<string> _sideDishes;

        public event EventHandler<MessageEventArgs> OnErrorMessageRaised;

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

                switch (dishType)
                {
                    case DishType.Starters:
                        _starters = itemList;
                        break;
                    case DishType.MainDishes:
                        _mainDishes = itemList;
                        break;
                    case DishType.SideDishes:
                        _sideDishes = itemList;
                        break;
                }

                foreach (var item in itemList)
                {
                    yield return item;
                }
            }
        }

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

        public void OpenConnection()
        {
            throw new NotImplementedException();
        }

        public void CloseConnection()
        {
            throw new NotImplementedException();
        }

        private void RaiseMessage(string message)
        {
            OnErrorMessageRaised?.Invoke(this, new MessageEventArgs(message));
        }
    }
}
