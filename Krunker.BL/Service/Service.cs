using ConsoleAppDataBSela.Model;
using Krunker.Common;
using Krunker.DAL.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;

namespace Krunker.BL.Service
{
    public class Service : IService
    {

        public event EventHandler<AbstractItem> OutOfStockEventHandler;

        private BackItemRepository bags;
        private HeadItemRepository hats;
        private PrimaryWeaponsRepository primaryWeapons;
        private SecondaryWeaponRepository secondaryWeapons;

        private Dictionary<Type, AbstractItem> shoppingCart;

        private List<AbstractItem> items;
        /// <summary>
        /// Carts list for the report
        /// </summary>
        public List<ShoppingCartItems> cartItems { get; }

        /// <summary>
        /// singelton Service
        /// </summary>
        private static Service instance = null;
        public static Service Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Service();
                }
                return instance;
            }
        }

        /// <summary>
        ///Ctor Creates repositories and adds items to items list
        /// </summary>
        private Service()
        {
            items = new List<AbstractItem>();
            shoppingCart = new Dictionary<Type, AbstractItem>();

            cartItems = new List<ShoppingCartItems>();

            
            primaryWeapons = new PrimaryWeaponsRepository();
            secondaryWeapons = new SecondaryWeaponRepository();
            hats = new HeadItemRepository();
            bags = new BackItemRepository();

            items.AddRange(primaryWeapons.GetItems());
            items.AddRange(secondaryWeapons.GetItems());
            items.AddRange(hats.GetItems());
            items.AddRange(bags.GetItems());
        }

        public List<AbstractItem> GetAllItems() => items;

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

        public void AddItem(AbstractItem item)
        {
            Dictionary<Type, Action> methodType = new Dictionary<Type, Action>()
            {
                {typeof(PrimaryWeapon),() => primaryWeapons.ItemCreate((PrimaryWeapon)item) },
                {typeof(SecondaryWeapon),() => secondaryWeapons.ItemCreate((SecondaryWeapon)item) },
                {typeof(HeadItem),() => hats.ItemCreate((HeadItem)item) },
                {typeof(BackItem),() => bags.ItemCreate((BackItem)item) },
            };
            methodType[item.GetType()]();
        }
        /// <summary>
        /// Indexer to get specific item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AbstractItem this[int id]
        {
            get
            {
                return items.FirstOrDefault(x => x.Id == id);
            }
        }

        public void AddToCart(AbstractItem item)
        {
            shoppingCart[item.GetType()] = item;
        }

        public string GetCartItems()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in shoppingCart)
                sb.Append($"{item.Value.Name}, ");

            if (sb.Length > 2)
                sb.Replace(",", ".", sb.Length - 2, 1);

            return sb.ToString();
        }

        public double CalculateCart()
        {
            double sum = 0;
            foreach (var item in shoppingCart)
                sum += item.Value.FinalPrice;

            return sum;
        }

        public void RemoveCartItems()
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
            cartItems.Add(new ShoppingCartItems(cart));
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
