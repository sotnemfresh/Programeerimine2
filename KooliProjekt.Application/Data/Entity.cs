using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    // 28.11
    // Baasklass kõikidele klassidele, mille jaoks on
    // ApplicationDbContextis oma DbSet
    public abstract class Entity
    {
        public int Id { get; set; }
    }
}
