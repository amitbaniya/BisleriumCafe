using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Models
{
    public class Members
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        public float DiscountPercent {  get; set; }

        public bool Redemption { get; set; } = false;

        public DateTime? DicountStartDate { get; set; }

        public DateTime? DicountEndDate { get; set;}


       
    }
}
