// KooliProjekt.Application/Dto/ArveListDto.cs

using System;

namespace KooliProjekt.Application.Dto
{
    public class ArveListDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal GrandTotal { get; set; }

        // LISATUD: Vajalik listivaate jaoks
        public string Status { get; set; }

        // Vajalik, et saada Kliendi info List-vaates
        public KlientListDto Klient { get; set; }
    }
}