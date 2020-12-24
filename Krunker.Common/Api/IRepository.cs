using ConsoleAppDataBSela.Model;
using System.Collections.Generic;

namespace Krunker.Common.Api
{
    public interface IRepository<T> where T : AbstractItem
    {
        // Returns specific item by id
        T GetItemById(int itemId);
        // Create new item
        void ItemCreate(T item);
        // Deletes specific item by id
        void ItemDelete(int ItemId);
        // Update item amount
        void ItemUpdate(T Item);
        // return items list as Abstractitem
        List<AbstractItem> GetItems();
        // Update the amout of all items in recieved list in repository
        void UpdateRepository(IEnumerable<T> list);
    }
}
