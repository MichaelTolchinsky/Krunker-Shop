using System;

namespace Krunker.Common.Models
{
    public class PrimaryWeapon : Weapon
    {
        public int AmountBalls { get; set; }
        public double ShootingRange { get; set; }
       
        public PrimaryWeapon(int Id,int amountBalls, double shootingRange ,string creator, int year, int firePower, bool automatic, string name, Uri uriImg, double price, int currentAmout, int starterAmount)
            : base(Id,creator,year,firePower,automatic,name, uriImg, price, currentAmout, starterAmount)
        {
            AmountBalls = amountBalls;
            ShootingRange = shootingRange;
        }
    }
}
