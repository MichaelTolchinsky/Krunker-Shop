using ConsoleAppDataBSela.Model;
using Krunker.Common.Api;
using System;
using System.Collections.Generic;

namespace Krunker.DAL.Repository
{
    public class HeadItemRepository : IRepository<HeadItem>
    {
        List<AbstractItem> headItems;

        public HeadItemRepository()
        {
            headItems = new List<AbstractItem>();
            headItems.Add(new HeadItem(17, "Bandit", new Uri(@"ms-appx:///Assets/Bandit.png"), 15, 3, 3));
            headItems.Add(new HeadItem(18, "Clown", new Uri(@"ms-appx:///Assets/Clown.png"), 12, 3, 3));
            headItems.Add(new HeadItem(19, "Diuer Goggles", new Uri(@"ms-appx:///Assets/Diuer Goggles.png"), 13, 3, 3));
            headItems.Add(new HeadItem(20, "Jack o' Lantern", new Uri(@"ms-appx:///Assets/Jack o' Lantern.png"), 10, 3, 3));
            headItems.Add(new HeadItem(21, "Mad Cap", new Uri(@"ms-appx:///Assets/Mad Cap.png"), 8, 3, 3));
            headItems.Add(new HeadItem(22, "Madman", new Uri(@"ms-appx:///Assets/Madman.png"), 7, 3, 3));
            headItems.Add(new HeadItem(23, "Pig Head", new Uri(@"ms-appx:///Assets/Pig Head.png"), 20, 3, 3));
            headItems.Add(new HeadItem(24, "Afro", new Uri(@"ms-appx:///Assets/Rfro.png"), 19, 3, 3));
            headItems.Add(new HeadItem(25, "Sun Glasses", new Uri(@"ms-appx:///Assets/Sun Glasses.png"), 25, 3, 3));
            headItems.Add(new HeadItem(26, "Zombie Head", new Uri(@"ms-appx:///Assets/Zombie Head.png"), 30, 3, 3));
        }

        public HeadItem GetItemById(int itemId)
        {
            return (HeadItem)headItems.Find(x => x.Id == itemId);
        }

        public List<AbstractItem> GetItems()
        {
            return headItems;
        }

        public void ItemCreate(HeadItem item)
        {
            headItems.Add(item);
        }

        public void ItemDelete(int ItemId)
        {
            headItems.RemoveAll(w => w.Id == ItemId);
        }

        public void ItemUpdate(HeadItem Item)
        {
            int ind = headItems.FindIndex(w => w.Id == Item.Id);
            headItems[ind] = Item;
        }

        public void UpdateRepository(IEnumerable<HeadItem> list)
        {
            headItems.Clear();
            headItems.AddRange(list);
        }
    }
}
