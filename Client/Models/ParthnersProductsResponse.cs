using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class ParthnersProductsResponse
    {
        public List<Parthners> Parthners { get; set; }
        public List<Products> Products { get; set; }
    }
}
