using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Parthners
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string DirectorFullName { get; set; }
        public string Email { get; set; }
        public int Rating { get; set; }
        public string ComapnyName { get; set; }

        public List<Products> ProductEntities { get; set; }
    }
}
