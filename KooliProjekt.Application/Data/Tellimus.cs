﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
namespace KooliProjekt.Application.Data
{
[ExcludeFromCodeCoverage]
    public class Tellimus
    {
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } // Lisatud oleku väli

        // Seos Kliendiga (One-to-Many)
        public Klient Klient { get; set; }
        [Required]
        public int KlientId { get; set; }

        // Seos Arvega (One-to-One)
        public Arve Arve { get; set; }

        // Seos Tellimuse Ridadega (One-to-Many)
        public IList<TellimuseRida> TellimuseRead { get; set; } = new List<TellimuseRida>();
    }
}