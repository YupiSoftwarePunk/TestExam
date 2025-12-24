using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Server.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double DefectivePercent { get; set; }

        public List<Products> ProductEntities { get; set; }
    }
}
