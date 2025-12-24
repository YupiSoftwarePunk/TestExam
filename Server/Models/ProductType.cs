using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public double Coefficient { get; set; }

        public List<Products> ProductEntities { get; set; }
    }
}
