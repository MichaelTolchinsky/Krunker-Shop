using ConsoleAppDataBSela.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Krunker.DAL.Repository
{
    public class PrimaryWeaponsRepository : IRepository<PrimaryWeapon>
    {
        List<AbstractItem> weapons;

        public PrimaryWeaponsRepository()
        {
            weapons = new List<AbstractItem>();
            weapons.Add(new PrimaryWeapon(1,14,55,"michael",1766,19,true,"Cherry Blossom",new Uri(@"ms-appx:///Assets/Cherry Blossom.png"),25,5,5));
            weapons.Add(new PrimaryWeapon(2,9,30,"michael",1942,17,true,"Commo",new Uri(@"ms-appx:///Assets/Commo.png"),19,5,5));
            weapons.Add(new PrimaryWeapon(3,10,44,"michael",1767,20,false,"Dijon",new Uri(@"ms-appx:///Assets/Dijon.png"),27,5,5));
            weapons.Add(new PrimaryWeapon(4,12,55,"michael",1768,30,false,"Interuention Ul",new Uri(@"ms-appx:///Assets/Interuention Ul.png"),29,5,5));
            weapons.Add(new PrimaryWeapon(5,16,66,"michael",1769,25,true,"LMG Wanderer",new Uri(@"ms-appx:///Assets/LMG Wanderer.png"),17,5,5));
            weapons.Add(new PrimaryWeapon(6,8,28,"michael",1821,22,false,"Puma RR",new Uri(@"ms-appx:///Assets/Puma RR.png"),15,5,5));
            weapons.Add(new PrimaryWeapon(7,7,31,"michael",1991,24,false,"Aqua",new Uri(@"ms-appx:///Assets/Rqua.png"),30,5,5));
            weapons.Add(new PrimaryWeapon(8,22,50,"michael",1453,26,false,"Spectrum",new Uri(@"ms-appx:///Assets/Spectrum.png"),28,5,5));
            weapons.Add(new PrimaryWeapon(9,29,60,"michael",1333,50,false,"Theta",new Uri(@"ms-appx:///Assets/Theta.png"),45,5,5));
        }

        public PrimaryWeapon GetItemById(int itemId)
        {
            return (PrimaryWeapon)weapons.Find(x => x.Id == itemId);
        }

        public List<AbstractItem> GetItems()
        {
            return weapons;
        }

        public void ItemCreate(PrimaryWeapon item)
        {
            weapons.Add(item);
        }

        public void ItemDelete(int ItemId)
        {
            weapons.RemoveAll(w => w.Id == ItemId);
        }

        public void ItemUpdate(PrimaryWeapon Item)
        {
           int ind= weapons.FindIndex(w => w.Id == Item.Id);
           weapons[ind] = Item;
        }

        public void UpdateRepository(IEnumerable<PrimaryWeapon> list)
        {
            weapons.Clear();
            weapons.AddRange(list);
        }

    }
}
