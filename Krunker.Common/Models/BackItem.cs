using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krunker.Common.Models
{
    public class BackItem : AbstractItem
    {
        public BackItem(int Id,string name, Uri uriImg, double price, int currentAmout, int starterAmount)
            : base(Id,name, uriImg, price, currentAmout, starterAmount)
        {

        }

    }
}