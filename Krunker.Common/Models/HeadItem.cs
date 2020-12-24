using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krunker.Common.Models
{
   public  class HeadItem : AbstractItem
    {
        public HeadItem(int Id, string name, Uri uriImg, double price, int currentAmout, int starterAmount)
            : base(Id,name, uriImg, price, currentAmout, starterAmount)
        {

        }
    }
}
