using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
[ExcludeFromCodeCoverage]
    public class TellimuseRidaDto
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }

        public ToodeListDto Toode { get; set; }
        public int TellimusId { get; set; }
    }
}