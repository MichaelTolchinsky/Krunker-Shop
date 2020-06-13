using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleAppDataBSela.Model
{
    public abstract class AbstractItem : IEquatable<AbstractItem>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get;set; }
        public double FinalPrice { get{return Price - (Price * Discount/100); } }
        public double Discount { get; set; }
        public int CurrentAmout { get; set; }
        public int StarterAmount { get; set; }
        public Uri Uri { get; set; }
        public string Details { get { return $"Name: {Name}, Price: {FinalPrice:C}"; } }
        public string FullDetails { get { return $"ID: {Id},Type: {GetType().Name}, Price: {FinalPrice:C},Current Amount: {CurrentAmout}"; } }

        public AbstractItem(int id, string name, Uri uriImg, double price, int currentAmout, int starterAmount)
        {
            Id = id;
            Name = name;
            Uri = uriImg;
            Price = price;
            CurrentAmout = currentAmout;
            StarterAmount = starterAmount;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals(obj as AbstractItem);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(AbstractItem other) => Id == other.Id;
    }
}
