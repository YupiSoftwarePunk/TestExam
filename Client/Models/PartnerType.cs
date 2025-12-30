using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class PartnerType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public List<Parthners> Partners { get; set; }
    }
}
