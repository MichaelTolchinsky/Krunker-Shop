using Krunker.Common.Api;
using Krunker.Common.Models;
using System;
using System.Collections.Generic;

namespace Krunker.DAL.Repository
{
    public class BackItemRepository : IRepository<BackItem>
    {
        readonly List<AbstractItem> backItems;

        public BackItemRepository()
        {
            backItems = new List<AbstractItem>
            {
                new BackItem(27, "Diablo Wings", new Uri(@"ms-appx:///Assets/Diablo Wings.png"), 12, 5, 5),
                new BackItem(28, "Panda Body", new Uri(@"ms-appx:///Assets/Panda Body.png"), 11, 5, 5),
                new BackItem(29, "Angeilc Wings", new Uri(@"ms-appx:///Assets/Rngeilc Wings.png"), 9, 5, 5),
                new BackItem(30, "Diablo Wings", new Uri(@"ms-appx:///Assets/Samurai Blades.png"), 22, 5, 5)
            };
        }

        public BackItem GetItemById(int itemId)
        {
            return (BackItem)backItems.Find(x => x.Id == itemId);
        }

        public List<AbstractItem> GetItems()
        {
            return backItems;
        }

        public void ItemCreate(BackItem item)
        {
            backItems.Add(item);
        }

        public void ItemDelete(int ItemId)
        {
            backItems.RemoveAll(w => w.Id == ItemId);
        }

        public void ItemUpdate(BackItem Item)
        {
            int ind = backItems.FindIndex(w => w.Id == Item.Id);
            backItems[ind] = Item;
        }
        public void UpdateRepository(IEnumerable<BackItem> list)
        {
            backItems.Clear();
            backItems.AddRange(list);
        }
    }
}
