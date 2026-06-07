﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Application.Data
{
    public class Klient : Entity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Column("FirstName")] 
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        [Column("LastName")]
        public string LastName { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Range(0, 100)]
        public decimal Discount { get; set; }

        public IList<Tellimus> Tellimused { get; set; } = new List<Tellimus>();
        public IList<Arve> Arved { get; set; } = new List<Arve>();
    }
}