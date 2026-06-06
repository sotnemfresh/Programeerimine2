using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Tellimus
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }

        // VAT
        public decimal VatRate { get; set; }
        public decimal VatAmount { get; set; }

        // viide Toode
        public int ToodeId { get; set; }
        public Toode Toode { get; set; }

        public int ArveId { get; set; }
        public Arve Arve { get; set; }
    }
}