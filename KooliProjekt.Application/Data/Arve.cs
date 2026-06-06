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
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }

        public decimal SubTotal { get; set; }
        public decimal ShippingTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }

        public int KlientId { get; set; }
        public Klient Klient { get; set; }

        public List<Tellimus> Tellimused { get; set; } = new List<Tellimus>();
    }
}