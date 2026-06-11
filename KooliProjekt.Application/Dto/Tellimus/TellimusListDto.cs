using System;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
[ExcludeFromCodeCoverage]
    public class TellimusListDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string KlientFirstName { get; set; }
        public string KlientLastName { get; set; }
        public int RidadeArv { get; set; }
    }
}