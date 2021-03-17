using MPData.EventArgs;
using MPData.Models;
using System;
using System.Collections.Generic;

namespace MPData.DataService
{
    public interface IDishItemDataService
    {
        event EventHandler<MessageEventArgs> OnErrorMessageRaised;

        void OpenConnection();
        void CloseConnection();

        IEnumerable<string> GetAll(DishType dishType);

        IEnumerable<string> AddItem(string newItem, DishType dishType);
    }
}
