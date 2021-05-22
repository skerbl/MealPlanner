using System;
using System.Collections.Generic;

namespace MPData
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
