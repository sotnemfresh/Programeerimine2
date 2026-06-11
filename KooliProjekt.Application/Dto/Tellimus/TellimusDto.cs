using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
[ExcludeFromCodeCoverage]
    // Kasutatakse detailvaates (GetQuery)
    public class TellimusDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        public KlientListDto Klient { get; set; }

        // Vältimaks tsüklit (Arve viitab Tellimusele, Tellimus viitab Arvele),
        // kasutame siin kergemat TellimusListDto tüüpi.
        public ArveListDto Arve { get; set; }

        // TellimusDto peab sisaldama ridu
        public IList<TellimuseRidaDto> TellimuseRead { get; set; }
    }
}