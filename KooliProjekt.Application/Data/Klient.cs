using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Klient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set;}
        public string Phone { get; set; }
        public decimal Discount { get; set; }

        public List<Arve> Arved { get; set; } = new List<Arve>();
    }
}