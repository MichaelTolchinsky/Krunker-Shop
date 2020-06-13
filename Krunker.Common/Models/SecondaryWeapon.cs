using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleAppDataBSela.Model
{
    public class SecondaryWeapon : Weapon
    {
        public int KnifeLength { get; set; }
        public KnifeType TypeKnife { get; set; }

        public SecondaryWeapon(int Id,int knifeLength, KnifeType typeKnife, string creator, int year, int firePower, bool automatic, string name, Uri uriImg, double price, int currentAmout, int starterAmount)
            : base(Id,creator, year, firePower, automatic, name, uriImg, price, currentAmout, starterAmount)
        {
            KnifeLength = knifeLength;
            TypeKnife = typeKnife;
        }
        public enum KnifeType
        {
            Commando,
            Leatherman,
        }

    }
}
