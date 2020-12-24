using ConsoleAppDataBSela.Model;
using System.Collections.Generic;
using System.Linq;

namespace Krunker.Common
{
    // Class that creates the items for the Report
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
            this.items = Items;
            ID = Id++;
        }

        public override string ToString()
        {
            string names = "";
            foreach (var item in items)
                names += $"{item.Name}\n";
            names += "Total Sum";

            return names;
        }
    }
}
