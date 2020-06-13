using ConsoleAppDataBSela.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krunker.DAL.Repository
{
    public interface IRepository<T> where T : AbstractItem
    {
        /// <summary>
        /// Returns specific item by id
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        T GetItemById(int itemId);
        /// <summary>
        /// Create new item
        /// </summary>
        /// <param name="item"></param>
        void ItemCreate(T item);
        /// <summary>
        /// Deletes specific item by id
        /// </summary>
        /// <param name="ItemId"></param>
        void ItemDelete(int ItemId);
        /// <summary>
        /// Update item amount
        /// </summary>
        /// <param name="Item"></param>
        void ItemUpdate(T Item);
        /// <summary>
        /// return items list as Abstractitem
        /// </summary>
        /// <returns></returns>
        List<AbstractItem> GetItems();
        /// <summary>
        /// Update the amout of all items in recieved list in repository
        /// </summary>
        /// <param name="list"></param>
         void UpdateRepository(IEnumerable<T> list);
    }
}
