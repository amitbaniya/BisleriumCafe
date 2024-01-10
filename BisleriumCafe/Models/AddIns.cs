using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Models
{
    public class AddIns
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public float Price { get; set; }

        public int? Quantity {  get; set; }
    }
}
