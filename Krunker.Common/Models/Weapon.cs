using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krunker.Common.Models
{

   public class Weapon : AbstractItem
    {
        public string Creator { get; set; } 
        public int Year { get; set; }
        public int FirePower { get; set; }
        public bool Automatic { get; set; }
        
        public Weapon(int Id,string creator, int year, int firePower, bool automatic,string name, Uri uriImg, double price, int currentAmout, int starterAmount)
            : base(Id,name, uriImg, price, currentAmout, starterAmount)
        {
            Creator = creator;
            Year = year;
            FirePower = firePower;
            Automatic = automatic;
        }

    }
}
