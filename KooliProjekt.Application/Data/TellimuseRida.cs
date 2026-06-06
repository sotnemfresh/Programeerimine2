using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class TellimuseRida
    {
        public int Id { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }

        [Range(0, 1)] // 0.20 tähistab 20%
        public decimal VatRate { get; set; }
        public decimal VatAmount { get; set; }

        // Viide Toode (One-to-Many)
        public Toode Toode { get; set; }
        [Required]
        public int ToodeId { get; set; }

        // Viide Tellimus (One-to-Many)
        public Tellimus Tellimus { get; set; }
        [Required]
        public int TellimusId { get; set; }
    }
}