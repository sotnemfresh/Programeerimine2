using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class Arve : Entity
    {
        [Required]
        [MaxLength(50)]
        public string InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }

        [MaxLength(20)]
        public string Status { get; set; }

        [Range(0, double.MaxValue)]
        public decimal SubTotal { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ShippingTotal { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Discount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal GrandTotal { get; set; }

        public Klient Klient { get; set; }
        public int KlientId { get; set; }

        public Tellimus Tellimus { get; set; }
        [Required]
        public int TellimusId { get; set; }

        public IList<TellimuseRida> TellimuseRead { get; set; } = new List<TellimuseRida>();
    }
}