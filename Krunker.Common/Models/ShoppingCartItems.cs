using System.Collections.Generic;
using System.Linq;

namespace Krunker.Common.Models
{
    /// <summary>
    /// Class that creates the items for the Report
    /// </summary>
    public class ShoppingCartItems
    {
        private static int Id = 1;
        public int ID{get;set;}  
        public List<AbstractItem> Items { get; }
        public string Names => ToString();
        public string Prices
        {
            get
            {
                string str = "\n";
                Items.ForEach(it => str += $"{it.FinalPrice:C}\n");
                str += $"{Items.Sum(x => x.FinalPrice):C}";

                return str;
            }
        }

        public ShoppingCartItems(List<AbstractItem> Items)
        {
            this.Items = Items;
            ID = Id++;
        }

        public override string ToString()
        {
            string names = "";
            foreach (var item in Items)
                names += $"{item.Name}\n";

            return names;
        }
    }
}
