using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
[ExcludeFromCodeCoverage]
    public class TellimuseRidaListDto
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public string ToodeName { get; set; }
        public decimal LineTotal { get; set; }
        public int TellimusId { get; set; }
    }
}