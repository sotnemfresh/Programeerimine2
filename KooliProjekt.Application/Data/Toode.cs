﻿using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class Toode
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string FotoURL { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue)]
        public decimal StockQuantity { get; set; } // Lisatud: Laos olev kogus
    }
}