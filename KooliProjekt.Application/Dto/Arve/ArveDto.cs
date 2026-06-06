using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    // Kasutatakse detailvaates (GetQuery)
    public class ArveDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingTotal { get; set; }
        public decimal Discount { get; set; }

        // Seotud andmed
        public KlientDto Klient { get; set; }

        // MUUDETUD: Et vältida tsükleid ja vastata GetArveQueryHandler select-ile.
        // TellimusDto sisaldab TellimuseRidasi, TellimusListDto mitte.
        public TellimusListDto Tellimus { get; set; }
    }
}