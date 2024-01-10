using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Models
{
    public class Items
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int? CartNumber { get; set; }
        public string Name { get; set; }
        
        public float  Price { get; set; }

        public List<AddIns>? AddIns { get; set; } = new List<AddIns>();

        public ItemCategory ItemCategory { get; set; }
    }

}
