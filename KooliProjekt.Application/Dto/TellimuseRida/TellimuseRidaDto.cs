namespace KooliProjekt.Application.Dto
{
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