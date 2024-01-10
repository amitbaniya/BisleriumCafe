using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? Description { get; set; }

        public double TotalPrice { get; set; }

        public List<Items> Items { get; set; } = new List<Items>();

        
    }
}
