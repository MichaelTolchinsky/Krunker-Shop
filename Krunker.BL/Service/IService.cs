using ConsoleAppDataBSela.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krunker.BL.Service
{
    public interface IService
    {
        /// <summary>
        /// Return the list of all items in Repository
        /// </summary>
        /// <returns></returns>
        List<AbstractItem> GetAllItems();
        /// <summary>
        /// return the list of chosen type from Repository
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<AbstractItem> GetItems<T>() where T : AbstractItem;
        void AddItem(AbstractItem item);
        /// <summary>
        /// Add the item that the user have chosen to the ShoppingCart
        /// </summary>
        /// <param name="item"></param>
        void AddToCart(AbstractItem item);
        /// <summary>
        /// String that contains all the items in the current cart
        /// </summary>
        /// <returns></returns>
        string GetCartItems();
        /// <summary>
        /// Calculate current cart's price
        /// </summary>
        /// <returns></returns>
        double CalculateCart();
        /// <summary>
        /// Removes the current cart items from inventory as user pays and transfers the cart to the report
        /// </summary>
        void RemoveCartItems();
        /// <summary>
        /// on page load updates the cart and removes the already bought items
        /// </summary>
        /// <param name="newlist"></param>
        void ResetInventory(List<AbstractItem> newlist);
    }
}
