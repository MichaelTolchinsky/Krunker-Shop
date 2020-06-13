using ConsoleAppDataBSela.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleAppDataBSela.Model.SecondaryWeapon;

namespace Krunker.DAL.Repository
{
    public class SecondaryWeaponRepository : IRepository<SecondaryWeapon>
    {
        List<AbstractItem> weapons;

        public SecondaryWeaponRepository()
        {
            weapons = new List<AbstractItem>();
            weapons.Add(new SecondaryWeapon(10, 14, KnifeType.Commando, "michael", 1777, 5, false, "FlameFang", new Uri(@"ms-appx:///Assets/FlameFang.png"), 8, 5, 5));
            weapons.Add(new SecondaryWeapon(11, 10, KnifeType.Commando, "michael", 1944, 6, false, "Raynbow", new Uri(@"ms-appx:///Assets/Raynbow.png"), 9, 5, 5));
            weapons.Add(new SecondaryWeapon(12, 11, KnifeType.Commando, "michael", 1766, 4, false, "Rutumn Puthon", new Uri(@"ms-appx:///Assets/Rutumn Puthon.png"), 5, 5, 5));
            weapons.Add(new SecondaryWeapon(13, 17, KnifeType.Commando, "michael", 1755, 3, false, "Soul Fang", new Uri(@"ms-appx:///Assets/Soul Fang.png"), 6, 5, 5));
            weapons.Add(new SecondaryWeapon(14, 8, KnifeType.Leatherman, "michael", 1555, 7, false, "Tiger Puthon", new Uri(@"ms-appx:///Assets/Tiger Puthon.png"), 7, 5, 5));
            weapons.Add(new SecondaryWeapon(15, 3, KnifeType.Commando, "michael", 1666, 8, false, "Uenomous", new Uri(@"ms-appx:///Assets/Uenomous.png"), 8, 5, 5));
            weapons.Add(new SecondaryWeapon(16, 5, KnifeType.Commando, "michael", 1999, 9, false, "Uolt Fang", new Uri(@"ms-appx:///Assets/Uolt Fang.png"), 9, 5, 5));
        }

        public SecondaryWeapon GetItemById(int itemId)
        {
            return (SecondaryWeapon)weapons.Find(x => x.Id == itemId);
        }

        public List<AbstractItem> GetItems()
        {
            return weapons;
        }

        public void ItemCreate(SecondaryWeapon item)
        {
            weapons.Add(item);
        }

        public void ItemDelete(int ItemId)
        {
            weapons.RemoveAll(w => w.Id == ItemId);
        }

        public void ItemUpdate(SecondaryWeapon Item)
        {
            int ind = weapons.FindIndex(w => w.Id == Item.Id);
            weapons[ind] = Item;
        }

         public void UpdateRepository(IEnumerable<SecondaryWeapon> list)
        {
            weapons.Clear();
            weapons.AddRange(list);
        }
    }
}
