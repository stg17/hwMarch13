using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hwMarch13.Data
{
    public class Ad
    {
        public int Id { get; set; }
        public DateTime date { get; set; }
        public int PhoneNumber { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
}
