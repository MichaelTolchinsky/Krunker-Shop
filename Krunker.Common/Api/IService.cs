using ConsoleAppDataBSela.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krunker.Common.Api
{
    public interface IService
    {
        // Return the list of all items in Repository
        List<AbstractItem> GetAllItems();
        // return the list of chosen type from Repository
        List<AbstractItem> GetItems<T>() where T : AbstractItem;
        void AddItem(AbstractItem item);
        // Add the item that the user have chosen to the ShoppingCart
        void AddToCart(AbstractItem item);
        // String that contains all the items in the current cart
        string GetCartItems();
        // Calculate current cart's price
        double CalculateCart();
        // Removes the current cart items from inventory as user pays and transfers the cart to the report
        void CartCheckout();
        // on page load updates the cart and removes the already bought items
        void ResetInventory(List<AbstractItem> newlist);
    }
}
