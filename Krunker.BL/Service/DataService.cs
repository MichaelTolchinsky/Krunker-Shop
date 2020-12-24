using Krunker.Common.Api;
using Krunker.Common;
using Krunker.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krunker.Common.Models;

namespace Krunker.BL.Service
{
    public class DataService : IDataService
    {
        public event EventHandler<AbstractItem> OutOfStockEventHandler;
        public List<ShoppingCartItems> CartItems { get; }

        private readonly BackItemRepository bags;
        private readonly HeadItemRepository hats;
        private readonly PrimaryWeaponsRepository primaryWeapons;
        private readonly SecondaryWeaponRepository secondaryWeapons;

        private readonly Dictionary<Type, AbstractItem> shoppingCart;
        private readonly List<AbstractItem> items;


        // singelton Service
        private static DataService instance = null;
        public static DataService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataService();
                }
                return instance;
            }
        }

        // Ctor Creates repositories and adds items to items list
        private DataService()
        {
            items = new List<AbstractItem>();
            shoppingCart = new Dictionary<Type, AbstractItem>();

            CartItems = new List<ShoppingCartItems>();

            
            primaryWeapons = new PrimaryWeaponsRepository();
            secondaryWeapons = new SecondaryWeaponRepository();
            hats = new HeadItemRepository();
            bags = new BackItemRepository();

            items.AddRange(primaryWeapons.GetItems());
            items.AddRange(secondaryWeapons.GetItems());
            items.AddRange(hats.GetItems());
            items.AddRange(bags.GetItems());
        }

        //return all items
        public List<AbstractItem> GetAllItems() => items;
        //returns items by type
        public List<AbstractItem> GetItems<T>() where T : AbstractItem
        {

            var FuncByType = new Dictionary<Type, Func<List<AbstractItem>>> {
              { typeof(HeadItem), () => hats.GetItems() },
              { typeof(BackItem), () => bags.GetItems() },
              { typeof(PrimaryWeapon), () => primaryWeapons.GetItems() },
              { typeof(SecondaryWeapon), () => secondaryWeapons.GetItems() }
             };
            if (FuncByType.ContainsKey(typeof(T)))
                return FuncByType[typeof(T)]();
            else
                throw new ArgumentException("No such type");
        }
        //add item to cart
        public void AddToCart(AbstractItem item)
        {
            shoppingCart[item.GetType()] = item;
        }
        //return cart items
        public string GetCartItems()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in shoppingCart)
                sb.Append($"{item.Value.Name}, ");

            if (sb.Length > 2)
                sb.Replace(",", ".", sb.Length - 2, 1);

            return sb.ToString();
        }
        // calculates cart total sum
        public double CalculateCart()
        {
            double sum = 0;
            foreach (var item in shoppingCart)
                sum += item.Value.FinalPrice;

            return sum;
        }
        
        public void CartCheckout()
        {
            var FuncByType = new Dictionary<Type, Action<AbstractItem>> {
              { typeof(HeadItem), (ab) => hats.ItemUpdate((HeadItem)ab) },
              { typeof(BackItem), (ab) => bags.ItemUpdate((BackItem)ab) },
              { typeof(PrimaryWeapon), (ab) => primaryWeapons.ItemUpdate((PrimaryWeapon)ab) },
              { typeof(SecondaryWeapon), (ab) => secondaryWeapons.ItemUpdate((SecondaryWeapon)ab) }
             };

            List<AbstractItem> cart = new List<AbstractItem>();
            foreach (var item in shoppingCart)
            {
                AbstractItem it = item.Value;
                cart.Add(it);
                it.CurrentAmout--;
               if (FuncByType.ContainsKey(it.GetType()))
                    FuncByType[it.GetType()].Invoke(it);
                if (it.CurrentAmout == 0)
                    OutOfStockEventHandler?.Invoke(this, it);
            }
            CartItems.Add(new ShoppingCartItems(cart));
            shoppingCart.Clear();
        }

        public void ResetInventory(List<AbstractItem> newlist)
        {
            items.Clear();
            items.AddRange(newlist);

            primaryWeapons.UpdateRepository(newlist.Where(x => x is PrimaryWeapon).Cast<PrimaryWeapon>());
            secondaryWeapons.UpdateRepository(newlist.Where(x => x is SecondaryWeapon).Cast<SecondaryWeapon>());
            bags.UpdateRepository(newlist.Where(x => x is BackItem).Cast<BackItem>());
            hats.UpdateRepository(newlist.Where(x => x is HeadItem).Cast<HeadItem>());
        }
    }
}
