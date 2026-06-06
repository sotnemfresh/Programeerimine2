using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Arve
    {
        public int Id { get; set; }
        public string LineItem { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal VatRate { get; set; }
        public decimal Total { get; set; }

        public int KlientId { get; set; }
        public Klient Klient { get; set; }

        public List<Tellimus> Tellimused { get; set; }
    }
}