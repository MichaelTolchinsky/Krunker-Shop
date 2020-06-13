using ConsoleAppDataBSela.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krunker.Common
{
    /// <summary>
    /// Class that creates the items for the Report
    /// </summary>
    public class ShoppingCartItems
    {
        private static int Id = 1;
        public int ID{get;set;}  
        public List<AbstractItem> items { get; }
        public string Names => ToString();
        public string Prices
        {
            get
            {
                string str = "\n";
                items.ForEach(it => str += $"{it.FinalPrice:C}\n");
                str += $"{items.Sum(x => x.FinalPrice):C}";

                return str;
            }
        }

        public ShoppingCartItems(List<AbstractItem> Items)
        {
            items = Items;
            ID = Id++;
        }

        public override string ToString()
        {
            string names = "";
            foreach (var item in items)
                names += $"{item.Name}\n";

            return names;
        }
    }
}
