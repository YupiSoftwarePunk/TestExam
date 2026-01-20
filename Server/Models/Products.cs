using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Products
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public int MaterialId { get; set; }
        public int TotalProducts { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public int ParthnerId { get; set; }
        public DateTime SaleDate { get; set; }
        public string Article { get; set; }
        public double MinPrice { get; set; }

        public Material Material { get; set; }
        public Parthners Parthner { get; set; }
        public ProductType ProductType { get; set; }
    }
}
